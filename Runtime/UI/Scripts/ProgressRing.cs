using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    public class ProgressRing : MonoBehaviour
    {
        public enum FillOriginMode
        {
            Bottom = 0,
            Right = 1,
            Top = 2,
            Left = 3
        }
        

        [Header("Settings")] 
        public MinMaxRange Values = new MinMaxRange(0f, 1f);

        [Space(10)] 
        public Image.FillMethod FillMethod = Image.FillMethod.Radial360;
        public FillOriginMode FillOrigin = FillOriginMode.Top;
        public bool Clockwise = true;
        public bool PreserveAspect = true;

        [Space(10)]
        public bool UpdateText;
        public string EmptyText;
        public string FillingTextPrefix;
        public bool ShowFillingValue;
        public string FillingValueFormat;
        public string FillingTextSuffix;
        public string FullText;

        [Header("Resources")] 
        public Sprite EmptySprite;
        public Sprite FillingSprite;
        public Sprite FullSprite;

        private Image _fillImage;
        private Text _text;

        private StringBuilder _sb;        
        
        private void Awake()
        {
            _fillImage = GetComponentInChildren<Image>(true);
            _text = GetComponentInChildren<Text>(true);
            
            if(_fillImage == null && _text == null) 
                Debug.LogError("Error - Lunity Progress Ring has neither image nor text - no visual change will occur on progress change!");

            _fillImage.fillMethod = FillMethod;
            _fillImage.fillClockwise = Clockwise;
            _fillImage.fillOrigin = (int)FillOrigin;
            _fillImage.preserveAspect = PreserveAspect;

            _sb = new StringBuilder(128);
        }

        /// <summary> Sets the stored value of this progress ring, and updates the text if required</summary>
        public void SetValue(float value)
        {
            var valueLerp = Values.InverseLerp(value);
            if (valueLerp <= 0f) {
                if (UpdateText && _text != null && !string.IsNullOrEmpty(EmptyText)) _text.text = EmptyText;
                if (_fillImage != null) {
                    if (EmptySprite != null) _fillImage.sprite = EmptySprite;
                    else if (FillingSprite != null) {
                        _fillImage.sprite = FillingSprite;
                    }
                }
            } else if (valueLerp >= 1f) {
                if (UpdateText && _text != null && !string.IsNullOrEmpty(FullText)) _text.text = FullText;
                if (_fillImage != null) {
                    if (FullSprite != null) _fillImage.sprite = FullSprite;
                    else if (FillingSprite != null) {
                        _fillImage.sprite = FillingSprite;
                    }
                }
            } else {
                if (UpdateText && _text != null) {
                    _sb.Clear();
                    if (!string.IsNullOrEmpty(FillingTextPrefix)) _sb.Append(FillingTextPrefix);
                    if (ShowFillingValue) {
                        if (FillingValueFormat.Contains("%")) {
                            _sb.Append(valueLerp.ToString(FillingValueFormat));
                        } else {
                            _sb.Append(value.ToString(FillingValueFormat));
                        }
                    }

                    if (!string.IsNullOrEmpty(FillingTextSuffix)) _sb.Append(FillingTextSuffix);

                    var s = _sb.ToString();
                    if(!string.IsNullOrEmpty(s)) _text.text = _sb.ToString();
                }

                if (_fillImage != null && FillingSprite != null) _fillImage.sprite = FillingSprite;
            }

            if (_fillImage != null) _fillImage.fillAmount = valueLerp;
        }

        /// <summary>For subscribing to .NET EventHandler<float> events</summary>
        public void HandleValueChanged(object sender, float value)
        {
            SetValue(value);
        }
    }
}