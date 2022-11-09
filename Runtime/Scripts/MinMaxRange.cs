using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunity
{
    [Serializable]
    public class MinMaxRange
    {
        public float min;
        public float max;
        public float range => max - min;

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

        public float InverseLerp(float value)
        {
            return Mathf.InverseLerp(min, max, value);
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

        public override string ToString()
        {
            return "(min: " + min + ", max: " + max + ")";
        }
    }
}