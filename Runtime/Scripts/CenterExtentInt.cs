using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunity
{
	[Serializable]
	public class CenterExtentInt
	{
		public int center;
		public int extent;
		/// center - range
		public int min => center - extent;
		
		/// center + range
		public int max => center + extent;

		/// 2 x extent
		public int size => extent * 2;

		public CenterExtentInt(int center, int extent)
		{
			this.center = center;
			this.extent = extent;
		}

		public float GetRandom()
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
			if (value < min) extent = Mathf.Abs(value - center);
			else if (value > max) extent = Mathf.Abs(value - center);
		}

		public bool Contains(float value)
		{
			return value >= min && value <= max;
		}

		public float Clamp(float value)
		{
			return Mathf.Clamp(value, min, max);
		}

		public static CenterExtentInt MaxValue => new CenterExtentInt(0, int.MaxValue);
		public static CenterExtentInt Zero => new CenterExtentInt(0, 0);
		public static CenterExtentInt One = new CenterExtentInt(0, 1);

		public override string ToString()
		{
			return "(center: " + center + ", range: " + extent + ")";
		}
	}
}