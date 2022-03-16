using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lunity
{
    public class BasicConsole : SimpleSingleton<BasicConsole>
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
        public Image BackgroundImage;
        public InputField InputText;
        public bool KeyToToggle;
        public bool StartOn = true;
        public KeyCode ToggleKey = KeyCode.BackQuote;
        public bool ToggleRequiresShift;
        public bool ToggleRequiresCtrl;
        public bool ToggleRequiresAlt;
        public bool BindToUnityConsole;

        private RectTransform _outputTextParent;
        private List<LogEntry> _logEntries;
        private int _lines;
        private int _rowCount;
        private int _columnCount;
        private StringBuilder _stringBuilder;
        private bool _bound;
        private CursorLockMode _cachedLockState;
        private bool _cachedCursorVisible;
        private bool _active;

        private BasicConsoleReactor[] _reactors;

        private void Start()
        {
            if (Instance == null || OutputText == null) return;

            CalculateMaxWidthAndHeight();
            _stringBuilder = new StringBuilder(_rowCount * _columnCount);

            _reactors = GetComponents<BasicConsoleReactor>();

            if (InputText == null) InputText = transform.Find("InputText").GetComponent<InputField>();
            if (BackgroundImage == null) BackgroundImage = transform.Find("ConsoleBackground").GetComponent<Image>();
            if (OutputText == null) OutputText = transform.Find("ConsoleText").GetComponent<Text>();

            //recalculate in case something has already logged before we were ready
            if (_logEntries != null) {
                foreach (var entry in _logEntries) entry.RecalculateLineCount(_columnCount);
                RepaintText();
            }

            if (KeyToToggle) {
                SetActive(StartOn);
            }

            if (BindToUnityConsole) {
                Application.logMessageReceived += HandleLog;
                _bound = true;
            }
            
            _cachedLockState = Cursor.lockState;
            _cachedCursorVisible = Cursor.visible;
        }

        private void SetActive(bool newState)
        {
            _active = newState;
            OutputText.gameObject.SetActive(_active);
            BackgroundImage.gameObject.SetActive(_active);
            InputText.gameObject.SetActive(_active);
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

        private void Update()
        {
            if (_bound != BindToUnityConsole) {
                if (BindToUnityConsole) {
                    Application.logMessageReceived += HandleLog;
                    _bound = true;
                } else {
                    Application.logMessageReceived -= HandleLog;
                    _bound = false;
                }
            }

            if (InputText.isFocused) {
                if (KeyToToggle && GetToggle()) {
                    InputText.text = "";
                    if(EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
                }
            }
            
            if (_active && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(InputText.text)) {
                var pieces = InputText.text.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var command = pieces[0];
                if (pieces.Length > 0) {
                    var args = new string[pieces.Length - 1];
                    for (var i = 1; i < pieces.Length; i++) args[i - 1] = pieces[i];
                    foreach (var reactor in _reactors) reactor.ReceiveCommand(command, args);
                } else {
                    foreach (var reactor in _reactors) reactor.ReceiveCommand(command);
                }

                InputText.text = "";
            } 

            if (!KeyToToggle) return;
            if (GetToggle()) {
                SetActive(!_active);

                if (_active) {
                    _cachedLockState = Cursor.lockState;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    if(EventSystem.current != null) EventSystem.current.SetSelectedGameObject(InputText.gameObject);
                } else {
                    Cursor.lockState = _cachedLockState;
                    Cursor.visible = _cachedCursorVisible;
                    
                    InputText.text = "";
                    if(EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        private bool GetToggle()
        {
            if (!Input.GetKeyDown(ToggleKey)) return false;
            if (ToggleRequiresShift && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) return false;
            if (ToggleRequiresAlt && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) return false;
            if (ToggleRequiresCtrl && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) return false;
            return true;
        }

        public static void Log(string log, string color = "#FFFFFFFF")
        {
            if (Instance == null) return;
            if (Instance._logEntries == null) Instance._logEntries = new List<LogEntry>();

            Instance._logEntries.Add(new LogEntry(log, color, Instance._columnCount));
            Instance._lines += Instance._logEntries[Instance._logEntries.Count - 1].LineCount;

            while (Instance._lines > Instance._rowCount) {
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
            while (true) {
                s.Append("\n");
                textGenerator.Populate(s.ToString(), generationSettings);
                var nextLineCount = textGenerator.lineCount;
                if (rowCount == nextLineCount) break;
                rowCount = nextLineCount;
            }

            s.Clear();
            textGenerator.Populate(s.ToString(), generationSettings);
            while (true) {
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
}