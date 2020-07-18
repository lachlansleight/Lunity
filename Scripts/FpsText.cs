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
            _stringBuilder.Append((int) FpsValue);
            _stringBuilder.Append(".");
            _stringBuilder.Append((int) (FpsValue % 1f * 10f));
            _text.text = _stringBuilder.ToString();
        }
    }
}