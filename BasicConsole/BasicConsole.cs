using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BasicConsole : MonoBehaviour
{

    private class LogEntry
    {
        public string Log;
        public string Color;
        public int LineCount;

        public LogEntry(string log, string color, int columnCount)
        {
            Log = log;
            Color = color;
            LineCount = Mathf.CeilToInt(log.Length / (float)columnCount);
        }

        public void RecalculateLineCount(int columnCount)
        {
            LineCount = Mathf.CeilToInt(Log.Length / (float)columnCount);
        }
    }

    public Text OutputText;
    private RectTransform _outputTextParent;
    private List<LogEntry> _logEntries;
    private int _lines;
    private int _rowCount;
    private int _columnCount;
    private StringBuilder _stringBuilder;
    
    private static BasicConsole __instance;

    private static BasicConsole _instance
    {
        get
        {
            if (__instance == null) __instance = FindObjectOfType<BasicConsole>();
            return __instance;
        }
    }

    private void Start()
    {
        CalculateMaxWidthAndHeight();
        _stringBuilder = new StringBuilder(_rowCount * _columnCount);
        
        //recalculate in case something has already logged before we were ready
        if(_logEntries != null) foreach (var entry in _logEntries) entry.RecalculateLineCount(_columnCount);
        RepaintText();
    }

    public static void Log(string log, string color = "#FFFFFFFF")
    {
        if (_instance._logEntries == null) _instance._logEntries = new List<LogEntry>();
        
        _instance._logEntries.Add(new LogEntry(log, color, _instance._columnCount));
        _instance._lines += _instance._logEntries[_instance._logEntries.Count - 1].LineCount;
        
        while(_instance._lines > _instance._rowCount) {
            _instance._lines -= _instance._logEntries[0].LineCount;
            _instance._logEntries.RemoveAt(0);
        }

        RepaintText();
    }

    public static void Log(string log, Color color)
    {
        var hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
        Log(log, hexColor);
    }

    private static void RepaintText()
    {
        var sb = _instance._stringBuilder;
        if (sb == null) return;

        sb.Clear();
        foreach (var log in _instance._logEntries) {
            if (sb.Length > 0) sb.Append("\n");
            if (!log.Color.Equals("#FFFFFFFF")) sb.Append($"<color=\"{log.Color}\">");
            sb.Append(log.Log);
            if (!log.Color.Equals("#FFFFFFFF")) sb.Append("</color>");
        }

        _instance.OutputText.text = sb.ToString();
    }

    private void CalculateMaxWidthAndHeight()
    {
        var textGenerator = new TextGenerator();
        var generationSettings = OutputText.GetGenerationSettings(OutputText.rectTransform.rect.size);
        var rowCount = 0;
        var colCount = 0;
        var s = new StringBuilder();
        while (true)
        {
            s.Append("\n");
            textGenerator.Populate(s.ToString(), generationSettings);
            var nextLineCount = textGenerator.lineCount;
            if (rowCount == nextLineCount) break;
            rowCount = nextLineCount;
        }

        s.Clear();
        textGenerator.Populate(s.ToString(), generationSettings);
        while (true)
        {
            s.Append("a");
            textGenerator.Populate(s.ToString(), generationSettings);
            var nextLineCount = textGenerator.lineCount;
            if (nextLineCount > 1) {
                break;
            }
            colCount++;
        }
        
        s.Clear();
        textGenerator.Populate(s.ToString(), generationSettings);

        _rowCount = rowCount;
        _columnCount = colCount;
    }
}
