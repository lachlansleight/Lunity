using UnityEngine;

public class LunityMath
{
	
	// Why is there no TWOPI in Unity :/
	public const float TAU = 6.283185307179586476925286766559f;
	public const float TWOPI = 6.283185307179586476925286766559f;
	
	//from https://blog.dakwamine.fr/?p=1943
	/// Returns the intersection between two infinite lines, each defined by two points (if one exists)
	public static bool Get2DIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 point)
	{
		var tmp = (b2.x - b1.x) * (a2.y - a1.y) - (b2.y - b1.y) * (a2.x - a1.x);

		if (tmp == 0) {
			// No solution!
			point = Vector2.zero;
			return false;
		}
 
		var mu = ((a1.x - b1.x) * (a2.y - a1.y) - (a1.y - b1.y) * (a2.x - a1.x)) / tmp;
 
		point = new Vector2(
			b1.x + (b2.x - b1.x) * mu,
			b1.y + (b2.y - b1.y) * mu
		);
		return true;
	}
	
	/// Returns the intersection between two discrete lines, each defined by two points
	public static bool Get2DIntersectionBounded(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 point)
	{
		point = Vector2.zero;
		
		var doIntersect = Get2DIntersection(a1, a2, b1, b2, out var p);
		if (!doIntersect) {
			return false;
		}

		var interpAx = (p.x - a1.x) / (a2.x - a1.x);
		if (interpAx < 0 || interpAx > 1) return false;
		var interpAy = (p.y - a1.y) / (a2.y - a1.y);
		if (interpAy < 0 || interpAy > 1) return false;
		var interpBx = (p.x - b1.x) / (b2.x - b1.x);
		if (interpBx < 0 || interpBx > 1) return false;
		var interpBy = (p.y - b1.y) / (b2.y - b1.y);
		if (interpBy < 0 || interpBy > 1) return false;

		point = p;
		return true;
	}

	/// Returns an equal-power crossfade value for A or B based on an input t value from -1 to 1
	/// At -1, a will be 1 and b will be 0
	/// At 1, a will be 0 and b will be 1
	public static float GetCrossfade(float t, bool a)
	{
		return a 
			? Mathf.Sqrt(0.5f * (1f - t)) 
			: Mathf.Sqrt(0.5f * (1f + t));
	}

	/// A more useful sine function - returns values from zero to one, and has a period of 1.0
	public static float Sin01(float t)
	{
		var sine = Mathf.Sin(t * Mathf.PI * 2f);
		return sine * 0.5f + 0.5f;
	}

	/// Convert a value from 0-1 into a value suitable for Mixer sliders (i.e. 0 -> 1 becomes -80dB -> 0dB)
	public static float VolumePower(float linearValue)
	{
		return Mathf.Log10(Mathf.Max(linearValue, 0.0001f)) * 20f;
	}

	/// Remaps an input float from one range to another
	public static float Map(float fromMin, float fromMax, float toMin, float toMax, float t)
	{
		return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, t));
	}

	/// Returns the result of a cubic bezier interpolation
	public static Vector3 CubicBezier(float t, Vector3 start, Vector3 startHandle, Vector3 endHandle, Vector3 end)
	{
		var t3 = t * t * t;
		var t2 = t * t;
		var f0 = -t3 + 3f * t2 - 3f * t + 1f;
		var f1 = 3f * t3 - 6f * t2 + 3f * t;
		var f2 = -3f * t3 + 3f * t2;
		var f3 = t3;
		return start * f0 + startHandle * f1 + endHandle * f2 + end * f3;
	}
}