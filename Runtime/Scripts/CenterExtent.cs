using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunity
{
	[Serializable]
	public class CenterExtent
	{
		public float center;
		public float extent;
		/// center - range
		public float min => center - extent;
		
		/// center + range
		public float max => center + extent;
		
		/// 2 x extent
		public float range => extent * 2;

		public CenterExtent(float center, float extent)
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

		public float InverseLerp(float value)
		{
			return Mathf.InverseLerp(min, max, value);
		}

		public void ExpandToFit(float value)
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

		public static CenterExtent MaxValue => new CenterExtent(0, float.MaxValue);
		public static CenterExtent Zero => new CenterExtent(0f, 0f);
		public static CenterExtent One = new CenterExtent(0f, 1f);

		public override string ToString()
		{
			return "(center: " + center + ", range: " + extent + ")";
		}
	}
}