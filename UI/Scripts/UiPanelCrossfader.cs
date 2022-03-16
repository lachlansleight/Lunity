using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    [ExecuteAlways]
    public class UiPanelCrossfader : MonoBehaviour
    {
        public enum CrossfadeModeOption {
            Alpha,
            Overlay,
        }
    
        public int PanelA = 0;
        public int PanelB = 1;
        [Range(-1f, 1f)] public float Crossfade = -1f;
        
        public bool ControlGameObjectActivity = true;
        public CrossfadeModeOption CrossfadeMode = CrossfadeModeOption.Alpha;
        public Color OverlayColor = Color.black;
        

        private List<CanvasGroup> _panels;
        private UiCrossfaderOverlay _overlay;
        
        private bool _initialized;

        public void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            
            _panels = new List<CanvasGroup>();
            
            for (var i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i).gameObject;
                if (child.GetComponent<UiCrossfaderOverlay>() != null) continue;
                
                var cg = child.GetComponent<CanvasGroup>();
                if (cg == null) cg = child.AddComponent<CanvasGroup>();
                _panels.Add(cg);
            }

            if (_panels.Count == 0) {
                Debug.LogError("UI Panel switcher has no children, and so has no panels! Disabling!", this);
                enabled = false;
                return;
            }

            if (_overlay != null) {
                _overlay.transform.SetAsLastSibling();
            }

            _initialized = true;
        }

        public void Update()
        {
            if (!_initialized) Initialize();
            if (CrossfadeMode == CrossfadeModeOption.Overlay && _overlay == null) {
                GetOrCreateOverlay();
            }
            
            for (var i = 0; i < _panels.Count; i++) {
                if (i == PanelA || i == PanelB) {
                    if (ControlGameObjectActivity) _panels[i].gameObject.SetActive(true);
                    if (CrossfadeMode == CrossfadeModeOption.Alpha) {   
                        _panels[i].alpha = Mathf.Clamp01(Crossfade * (i == PanelA ? -1 : 1));
                        if (_overlay != null) _overlay.Color = new Color(0f, 0f, 0f, 0f);
                    } else {
                        _panels[i].alpha = i == PanelA
                            ? Crossfade < 0 ? 1f : 0f
                            : Crossfade > 0 ? 1f : 0f;

                        if (_overlay == null) GetOrCreateOverlay();
                        
                        var c = OverlayColor;
                        c.a = 1f - Mathf.Abs(Crossfade);
                        _overlay.Color = c;
                    }
                    _panels[i].blocksRaycasts = i == PanelA ? Crossfade < 0f : Crossfade > 0f;
                    continue;
                }

                _panels[i].alpha = 0f;
                _panels[i].blocksRaycasts = false;
                if (ControlGameObjectActivity) _panels[i].gameObject.SetActive(false);
            }
        }

        private void GetOrCreateOverlay()
        {
            for (var i = 0; i < transform.childCount; i++) {
                _overlay = transform.GetChild(i).GetComponent<UiCrossfaderOverlay>();
            }

            if (_overlay == null) {
                var newObj = new GameObject("Overlay");
                newObj.AddComponent<RectTransform>();
                newObj.SetUIParent(transform, true);
                _overlay = newObj.AddComponent<UiCrossfaderOverlay>();
            }
        }

        [EditorButton]
        public void RefreshPanels()
        {
            _initialized = false;
            PanelA = 0;
            PanelB = 1;
            
            Initialize();
        }

        public int GetPanelCount()
        {
            return _panels.Count;
        }
    }
}