using UnityEngine;

public class LunityMath
{
	//from https://blog.dakwamine.fr/?p=1943
	public static bool Get2DIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 v2, out Vector2 point)
	{
		var tmp = (v2.x - b1.x) * (a2.y - a1.y) - (v2.y - b1.y) * (a2.x - a1.x);

		if (tmp == 0) {
			// No solution!
			point = Vector3.zero;
			return false;
		}
 
		var mu = ((a1.x - b1.x) * (a2.y - a1.y) - (a1.y - b1.y) * (a2.x - a1.x)) / tmp;
 
		point = new Vector2(
			b1.x + (v2.x - b1.x) * mu,
			b1.y + (v2.y - b1.y) * mu
		);
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
}