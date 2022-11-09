using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    public class LoadingBar : MonoBehaviour
    {
        public float MinValue = 0f;
        public float MaxValue = 1f;
        
        private Text _displayText;
        private Image _fillImage;
        private float _value;

        private void Awake()
        {
            GetReferences();
            SetValueAndText(MinValue, "");
        }

        private void GetReferences()
        {
            _displayText = GetComponentInChildren<Text>();
            _fillImage = transform.Find("FillImage").GetComponent<Image>();
        }

        /// <summary>
        /// Sets the display value for the progress bar
        /// </summary>
        /// <param name="value">The new value to display</param>
        public void SetValue(float value)
        {
            if(_fillImage == null) GetReferences();
            _fillImage.fillAmount = Mathf.InverseLerp(MinValue, MaxValue, value);
            _value = value;
        }

        /// <summary>
        /// Sets the display text for the progress bar
        /// </summary>
        /// <param name="text">The text to display</param>
        public void SetText(string text)
        {
            if(_displayText == null) GetReferences();
            _displayText.text = text;
        }

        /// <summary>
        /// Sets both the value and text for the progress bar
        /// </summary>
        /// <param name="value">The new value to display</param>
        /// <param name="text">The new text to display</param>
        public void SetValueAndText(float value, string text)
        {
            if(_fillImage == null || _displayText == null) GetReferences();
            _fillImage.fillAmount = Mathf.InverseLerp(MinValue, MaxValue, value);
            _displayText.text = text;
            _value = value;
        }

        /// <summary>
        /// Sets the value at which the progress bar will be completely empty
        /// </summary>
        /// <param name="min">The new minimum value</param>
        public void SetMinValue(float min)
        {
            MinValue = min;
            SetValue(_value);
        }

        /// <summary>
        /// Sets the value at which the progress bar will be completely full
        /// </summary>
        /// <param name="max">The new maximum value</param>
        public void SetMaxValue(float max)
        {
            MaxValue = max;
            SetValue(_value);
        }

        /// <summary>
        /// Sets the boundary values for the progress bar
        /// </summary>
        /// <param name="min">The value at which the progress bar will be completely empty</param>
        /// <param name="max">The value at which the progress bar will be completely full</param>
        public void SetMinMaxValues(float min, float max)
        {
            MinValue = min;
            MaxValue = max;
            SetValue(_value);
        }
    }
}