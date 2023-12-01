using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunity
{
    [Serializable]
    public struct MinMaxRange
    {
        public float min;
        public float max;
        public float range => max - min;
        public float center => min + (max - min) * 0.5f;

        public MinMaxRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetRandom()
        {
            return Random.Range(min, max);
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(min, max, t);
        }

        public float LerpWithCenter(float t, float customCenter)
        {
            if (t < 0.5f) return Mathf.Lerp(min, customCenter, t * 2f);
            return Mathf.Lerp(customCenter, max, (t - 0.5f) * 2f);
        }

        public float InverseLerp(float value)
        {
            return Mathf.InverseLerp(min, max, value);
        }

        public float InverseLerpWithCenter(float value, float customCenter)
        {
            if (value < customCenter) return Mathf.InverseLerp(min, customCenter, value) * 0.5f;
            return Mathf.InverseLerp(customCenter, max, value) * 0.5f + 0.5f;
        }

        public void ExpandToFit(float value)
        {
            min = Mathf.Min(min, value);
            max = Mathf.Max(max, value);
        }

        public bool Contains(float value)
        {
            return value >= min && value <= max;
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static MinMaxRange MaxValue => new MinMaxRange(-float.MaxValue, float.MaxValue);
        public static MinMaxRange InverseMaxValue => new MinMaxRange(float.MaxValue, -float.MaxValue);
        public static MinMaxRange Zero => new MinMaxRange(0f, 0f);
        public static MinMaxRange One = new MinMaxRange(0f, 1f);

        public static MinMaxRange Lerp(MinMaxRange a, MinMaxRange b, float t)
        {
            return new MinMaxRange(Mathf.Lerp(a.min, b.min, t), Mathf.Lerp(a.max, b.max, t));
        }

        public override string ToString()
        {
            return "(min: " + min + ", max: " + max + ")";
        }
    }
}