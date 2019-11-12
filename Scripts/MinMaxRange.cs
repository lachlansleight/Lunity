using System;

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
    }
}