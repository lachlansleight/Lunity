using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BasicConsole : Singleton<BasicConsole>
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

    public Text OutputText;
    private RectTransform _outputTextParent;
    private List<LogEntry> _logEntries;
    private int _lines;
    private int _rowCount;
    private int _columnCount;
    private StringBuilder _stringBuilder;

    private void Start()
    {
        if (Instance == null || OutputText == null) return;
        
        CalculateMaxWidthAndHeight();
        _stringBuilder = new StringBuilder(_rowCount * _columnCount);
        
        //recalculate in case something has already logged before we were ready
        if (_logEntries != null) {
            foreach (var entry in _logEntries) entry.RecalculateLineCount(_columnCount);
            RepaintText();
        }
    }

    public static void Log(string log, string color = "#FFFFFFFF")
    {
        if (Instance == null) return;
        if (Instance._logEntries == null) Instance._logEntries = new List<LogEntry>();
        
        Instance._logEntries.Add(new LogEntry(log, color, Instance._columnCount));
        Instance._lines += Instance._logEntries[Instance._logEntries.Count - 1].LineCount;
        
        while(Instance._lines > Instance._rowCount) {
            Instance._lines -= Instance._logEntries[0].LineCount;
            Instance._logEntries.RemoveAt(0);
        }

        RepaintText();
    }

    public static void Log(string log, Color color)
    {
        if (Instance == null) return;
        var hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
        Log(log, hexColor);
    }

    private static void RepaintText()
    {
        var sb = Instance._stringBuilder;
        if (sb == null) return;

        sb.Clear();
        foreach (var log in Instance._logEntries) {
            if (sb.Length > 0) sb.Append("\n");
            if (!log.Color.Equals("#FFFFFFFF")) sb.Append($"<color=\"{log.Color}\">");
            sb.Append(log.Log);
            if (!log.Color.Equals("#FFFFFFFF")) sb.Append("</color>");
        }

        Instance.OutputText.text = sb.ToString();
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
