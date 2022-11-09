using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    [RequireComponent(typeof(Text))]
    public class FpsText : MonoBehaviour
    {

        [Range(1, 200)] public int AverageCount = 5;
        public float FpsValue;
        public string Prefix;
        public string Suffix;
        public string Format = "0";
        
        private Text _text;

        private float[] _fpsValues;
        private int _fpsValueIndex;

        private StringBuilder _stringBuilder;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _fpsValues = new float[AverageCount];
            
            _stringBuilder = new StringBuilder(16);
        }

        private void Update()
        {
            var newFps = (1f / Time.unscaledDeltaTime);

            _fpsValues[_fpsValueIndex] = newFps;
            _fpsValueIndex = (_fpsValueIndex + 1) % _fpsValues.Length;

            var sum = 0f;
            foreach (var value in _fpsValues) {
                sum += value;
            }

            sum /= _fpsValues.Length;
            
            FpsValue = sum;
            
            _stringBuilder.Clear();
            if (!string.IsNullOrEmpty(Prefix)) _stringBuilder.Append(Prefix);
            _stringBuilder.Append(FpsValue.ToString(Format));
            if (!string.IsNullOrEmpty(Suffix)) _stringBuilder.Append(Suffix);
            _text.text = _stringBuilder.ToString();
        }
    }
}