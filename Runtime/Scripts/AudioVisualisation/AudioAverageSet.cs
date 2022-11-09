using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class AudioAverageSet : MonoBehaviour
    {
        public AudioData Audio;

        [Range(0f, 1f)] public float Momentary;
        [Range(0f, 1f)] public float HalfSecond;
        [Range(0f, 1f)] public float Second;
        [Range(0f, 1f)] public float FiveSecond;
        [Range(0f, 1f)] public float TenSecond;
        [Range(0f, 1f)] public float ThirtySecond;

        [Space(10)]
        [Range(0f, 1f)] public float Flicker;
        [Range(0f, 1f)] public float Pulse;
        [Range(0f, 1f)] public float Vibe;
        
        private TimeAverager _halfSecond;
        private TimeAverager _second;
        private TimeAverager _fiveSecond;
        private TimeAverager _tenSecond;
        private TimeAverager _thirtySecond;

        public void Awake()
        {
            _halfSecond = new TimeAverager(30);
            _second = new TimeAverager(60);
            _fiveSecond = new TimeAverager(300);
            _tenSecond = new TimeAverager(600);
            _thirtySecond = new TimeAverager(1800);
        }

        public void Update()
        {
            Momentary = Audio.avgVol;
            HalfSecond = _halfSecond.Update(Momentary);
            Second = _second.Update(Momentary);
            FiveSecond = _fiveSecond.Update(Momentary);
            TenSecond = _tenSecond.Update(Momentary);
            ThirtySecond = _thirtySecond.Update(Momentary);

            Flicker = Mathf.Clamp01((Momentary / (FiveSecond + 0.0001f)) - 1f);
            Pulse = Mathf.Clamp01((HalfSecond / (FiveSecond + 0.0001f)) - 1f);
            Vibe = Mathf.Clamp01((FiveSecond / (ThirtySecond + 0.0001f)) - 1f);
        }

        public class TimeAverager
        {
            public float Value;
            private float[] _samples;
            private int _index;
            private bool _full;

            public TimeAverager(int count)
            {
                _samples = new float[count];
                _index = 0;
                _full = false;
                Value = 0f;
            }

            public float Update(float sample)
            {
                _samples[_index] = sample;
                _index++;
                if (_index >= _samples.Length) {
                    _full = true;
                    _index = 0;
                }

                var avg = 0f;
                for (var i = 0; i < _samples.Length; i++) {
                    if (i >= _index && !_full) break;
                    avg += _samples[i];
                }

                avg /= _full ? _samples.Length : _index;
                Value = avg;

                return Value;
            }

            public void Reset()
            {
                _index = 0;
                _full = false;
                Value = 0f;
            }
        }
    }
}