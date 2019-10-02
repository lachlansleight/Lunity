using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    [RequireComponent(typeof(Text))]
    public class FpsText : MonoBehaviour
    {

        [Range(1, 200)] public int AverageCount = 5;

        private Text _text;

        private List<float> _fpsValues;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _fpsValues = new List<float>();
        }

        private void Update()
        {
            var newFps = (1f / Time.unscaledDeltaTime);

            _fpsValues.Add(newFps);
            while (_fpsValues.Count > AverageCount) _fpsValues.RemoveAt(0);

            var sum = 0f;
            foreach (var value in _fpsValues) {
                sum += value;
            }

            sum /= _fpsValues.Count;

            _text.text = sum.ToString("0.0");
        }
    }
}