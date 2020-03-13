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
    }
}