using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Lunity
{
    [DefaultExecutionOrder(-9999)]
    public class WorldSpaceConsole : MonoBehaviour
    {

        private class LogEntry
        {
            public string Log;
            public string Color;
            public int LineCount;

            public LogEntry(string log, string color)
            {
                Log = log;
                Color = color;
                LineCount = 0;
            }

            public LogEntry(string log, string color, int columnCount)
            {
                Log = log;
                Color = color;
                var lines = log.Split('\n');
                foreach (var line in lines) {
                    LineCount += Mathf.CeilToInt(line.Length / (float) columnCount);
                }
            }

            public void RecalculateLineCount(int columnCount)
            {
                var lines = Log.Split('\n');
                foreach (var line in lines) {
                    LineCount += Mathf.CeilToInt(line.Length / (float) columnCount);
                }
            }
        }


        [ReadOnly] public int RowCount;
        [ReadOnly] public int ColumnCount;

        private Text _outputText;

        private List<LogEntry> _logEntries;
        private int _lines;
        private StringBuilder _stringBuilder;
        private bool _readyToPaint;

        private BasicConsoleReactor[] _reactors;

        private void Awake() {
            _logEntries = new List<LogEntry>();

            Application.logMessageReceived += HandleLog;
        }

        private void Start()
        {
            StartCoroutine(WaitForRepaint());
        }

        private IEnumerator WaitForRepaint() {
            yield return new WaitForSeconds(0.5f);
            
            _stringBuilder = new StringBuilder(RowCount * ColumnCount);

            _reactors = GetComponents<BasicConsoleReactor>();

            _outputText = transform.Find("Content/Body/ConsoleText").GetComponent<Text>();
            CalculateMaxWidthAndHeight();

            //recalculate in case something has already logged before we were ready
            if (_logEntries.Count > 0) {
                _lines = 0;
                for(var i = 0; i < _logEntries.Count; i++) {
                    _logEntries[i].RecalculateLineCount(ColumnCount);
                    _lines += _logEntries[i].LineCount;

                    while (_lines > RowCount) {
                        _lines -= _logEntries[0].LineCount;
                        _logEntries.RemoveAt(0);
                        i--;
                    }
                }
                RepaintText();
            }

            _readyToPaint = true;
        }

        private void HandleLog(string condition, string stacktrace, LogType type)
        {
            switch (type) {
                case LogType.Log:
                    Log(condition);
                    break;
                case LogType.Warning:
                    Log(condition, "#ffdc5c");
                    break;
                case LogType.Exception:
                case LogType.Error:
                case LogType.Assert:
                    Log(condition, "#ff615e");
                    break;
            }
        }

        public void Log(string log, string color = "#FFFFFFFF")
        {
            if (_logEntries == null) _logEntries = new List<LogEntry>();

            if(!_readyToPaint) {
                //cache to be logged later
                _logEntries.Add(new LogEntry(log, color));
                return;
            }

            _logEntries.Add(new LogEntry(log, color, ColumnCount));
            _lines += _logEntries[_logEntries.Count - 1].LineCount;

            while (_lines > RowCount) {
                _lines -= _logEntries[0].LineCount;
                _logEntries.RemoveAt(0);
            }

            if(_readyToPaint) RepaintText();
        }

        public void Log(string log, Color color)
        {
            var hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
            Log(log, hexColor);
        }

        private void RepaintText()
        {
            var sb = _stringBuilder;
            if (sb == null) return;

            sb.Clear();
            foreach (var log in _logEntries) {
                if (sb.Length > 0) sb.Append("\n");
                if (!log.Color.Equals("#FFFFFFFF")) sb.Append($"<color=\"{log.Color}\">");
                sb.Append(log.Log);
                if (!log.Color.Equals("#FFFFFFFF")) sb.Append("</color>");
            }

            _outputText.text = sb.ToString();
        }

        private void CalculateMaxWidthAndHeight()
        {
            var textGenerator = new TextGenerator();
            var generationSettings = _outputText.GetGenerationSettings(_outputText.rectTransform.rect.size);
            var rowCount = 0;
            var colCount = 0;
            var s = new StringBuilder();
            var attempts = 0;
            while (true) {
                s.Append("\n");
                textGenerator.Populate(s.ToString(), generationSettings);
                var nextLineCount = textGenerator.lineCount;
                if (rowCount == nextLineCount) break;
                rowCount = nextLineCount;
                attempts++;
                if(attempts > 500) {
                    Debug.LogError("CalculateMaxWidthAndHeight failed to calculate the row count");
                    break;
                }
            }

            s.Clear();
            textGenerator.Populate(s.ToString(), generationSettings);
            attempts = 0;
            while (true) {
                s.Append("a");
                textGenerator.Populate(s.ToString(), generationSettings);
                var nextLineCount = textGenerator.lineCount;
                if (nextLineCount > 1) {
                    break;
                }

                colCount++;

                attempts++;
                if(attempts > 500) {
                    Debug.LogError("CalculateMaxWidthAndHeight failed to calculate the column count");
                    break;
                }
            }

            s.Clear();
            textGenerator.Populate(s.ToString(), generationSettings);

            RowCount = rowCount;
            ColumnCount = colCount;
        }
    }
}