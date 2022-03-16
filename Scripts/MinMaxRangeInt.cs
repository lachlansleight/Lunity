using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunity
{
	[Serializable]
	public class MinMaxRangeInt
	{
		public int min;
		public int max;
		public int range => max - min;

		public MinMaxRangeInt(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public int GetRandom()
		{
			return Random.Range(min, max);
		}

		public float Lerp(float t)
		{
			return Mathf.Lerp(min, max, t);
		}

		public int LerpInt(float t)
		{
			return Mathf.RoundToInt(Lerp(t));
		}

		public float InverseLerp(float value)
		{
			return Mathf.InverseLerp(min, max, value);
		}

		public void ExpandToFit(int value)
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

		public static MinMaxRangeInt MaxValue => new MinMaxRangeInt(-int.MaxValue, int.MaxValue);
		public static MinMaxRangeInt InverseMaxValue => new MinMaxRangeInt(int.MaxValue, -int.MaxValue);
		public static MinMaxRangeInt Zero => new MinMaxRangeInt(0, 0);
		public static MinMaxRangeInt One = new MinMaxRangeInt(0, 1);

		public override string ToString()
		{
			return "(min: " + min + ", max: " + max + ")";
		}
	}
}