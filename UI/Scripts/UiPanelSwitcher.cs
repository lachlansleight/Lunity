using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class UiPanelSwitcher : MonoBehaviour
    {
        public int StartPanelIndex = 0;
        [Range(0.01f, 1f)] public float FadeTime = 0.35f;
        public bool ControlGameObjectActivity = true;
        
        [ReadOnly]
        public int CurrentPanelIndex = -1;
        [ReadOnly]
        public int TargetPanelIndex = -1;

        private List<CanvasGroup> _panels;
        private Dictionary<string, int> _panelNames;

        private Coroutine _switchRoutine;
        private int _cachedNextPanel;

        private bool _initialized;

        public void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            
            _panels = new List<CanvasGroup>();
            _panelNames = new Dictionary<string, int>();
            _cachedNextPanel = -1;
            CurrentPanelIndex = -1;
            TargetPanelIndex = -1;
            
            for (var i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i).gameObject;
                var cg = child.GetComponent<CanvasGroup>();
                if (cg == null) cg = child.AddComponent<CanvasGroup>();
                if (!_panelNames.ContainsKey(child.name)) {
                    _panelNames.Add(child.name, i);
                }
                else {
                    Debug.LogWarning("Warning - panels already has a child called '" + child.name +
                                     "' - additional children with that name will not be accessible by name", this);
                }
                _panels.Add(cg);
            }

            if (_panels.Count == 0) {
                Debug.LogError("UI Panel switcher has no children, and so has no panels! Disabling!", this);
                enabled = false;
                return;
            }
            SetPanelImmediately(StartPanelIndex);

            _initialized = true;
        }

        /// Sets the panel immediately, with no fading - and ensures that all panels have the correct state
        public void SetPanelImmediately(string id)
        {
            if (!_initialized) Initialize();
            
            if (_panelNames.ContainsKey(id)) SetPanelImmediately(_panelNames[id]);
            else Debug.LogError("No panel with name " + id + " found", this);
        }

        /// Sets the panel immediately, with no fading - and ensures that all panels have the correct state
        public void SetPanelImmediately(int id)
        {
            if (!_initialized) Initialize();
            
            id = EnforceId(id);

            //If the target panel matches our current one, ignore the set operation
            if (id == CurrentPanelIndex) return;

            //Stop fading if we were already
            if (_switchRoutine != null) StopCoroutine(_switchRoutine);
            
            //Force all the panels into their correct states
            for (var i = 0; i < _panels.Count; i++) {
                var panel = _panels[i];
                panel.alpha = i == id ? 1 : 0;
                panel.blocksRaycasts = i == id;
                if (ControlGameObjectActivity) panel.gameObject.SetActive(i == id);
            }

            CurrentPanelIndex = id;
            TargetPanelIndex = id;
        }

        /// Fades the current panel out and the new panel in.
        /// If an animation is already in progress, queues the switch up to happen once the animation completes.
        public void SetPanel(int id)
        {
            if (!_initialized) Initialize();
            
            id = EnforceId(id);

            if (_switchRoutine != null) {
                _cachedNextPanel = EnforceId(id);
                //If we set the panel to go to its current target, then we can ignore this set operation
                if (_cachedNextPanel == TargetPanelIndex) _cachedNextPanel = -1;
                return;
            }

            //If the target panel matches our current one, ignore the set operation
            if (id == CurrentPanelIndex) return;
            
            _switchRoutine = StartCoroutine(SwitchPanelRoutine(id));
        }

        /// Fades the current panel out and the new panel in.
        /// If an animation is already in progress, queues the switch up to happen once the animation completes.
        public void SetPanel(string name)
        {
            if (!_initialized) Initialize();
            
            if (_panelNames.ContainsKey(name)) SetPanel(_panelNames[name]);
            else Debug.LogError("No panel with name " + name + " found", this);
        }

        private IEnumerator SwitchPanelRoutine(int targetPanelIndex)
        {
            TargetPanelIndex = targetPanelIndex;

            var currentPanel = _panels[CurrentPanelIndex];
            currentPanel.blocksRaycasts = false;
            for (var i = 0f; i < 1f; i += Time.deltaTime / FadeTime) {
                currentPanel.alpha = 1f - i;
                yield return null;
            }
            currentPanel.alpha = 0f;
            if (ControlGameObjectActivity) currentPanel.gameObject.SetActive(false);

            var targetPanel = _panels[TargetPanelIndex];
            if (ControlGameObjectActivity) targetPanel.gameObject.SetActive(false);
            for (var i = 0f; i < 1f; i += Time.deltaTime / FadeTime) {
                targetPanel.alpha = i;
                if (i > 0.5f) targetPanel.blocksRaycasts = true;
                yield return null;
            }
            targetPanel.alpha = 1f;

            CurrentPanelIndex = TargetPanelIndex;

            if (_cachedNextPanel >= 0) {
                _switchRoutine = StartCoroutine(SwitchPanelRoutine(_cachedNextPanel));
                _cachedNextPanel = -1;
            } else {
                _switchRoutine = null;
            }
        }

        private int EnforceId(int id)
        {
            if (id < 0) return 0;
            if (id >= _panels.Count) return _panels.Count - 1;
            return id;
        }
    }
}