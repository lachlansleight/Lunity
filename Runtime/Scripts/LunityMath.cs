using UnityEngine;

public class LunityMath
{
	
	// Why is there no TWOPI in Unity :/
	public const float TAU = 6.283185307179586476925286766559f;
	public const float TWOPI = 6.283185307179586476925286766559f;
	public const float PHI = 1.6180339887498948482045868343656f;
	public const float GOLDENRATIO = 1.6180339887498948482045868343656f;
	
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
	
	public static float MapFromDouble(double fromMin, double fromMax, float toMin, float toMax, double t)
	{
		var inverseLerp = Mathf.Clamp01((float) ((t - fromMin) / (fromMax - fromMin)));
		return Mathf.Lerp(toMin, toMax, inverseLerp);
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
	
	/// Get evenly-distributed points in a circle
	// sunflower distribution, from https://stackoverflow.com/questions/28567166/uniformly-distribute-x-points-inside-a-circle
	public static Vector2[] GetDistributedCirclePoints(int count, float radius, float boundaryEvenness = 0f, bool geodesic = false)
	{
		var points = new Vector2[count];

		var angleStride = geodesic ? 360f * GOLDENRATIO : TWOPI / (PHI * PHI);
		//angleStride *= Mathf.Deg2Rad;
		var boundaryPointCount = boundaryEvenness <= 0f ? 0 : Mathf.RoundToInt(boundaryEvenness * Mathf.Sqrt(count));

		for (var i = 0; i < count; i++) {
			var r = 1f;
			if (i <= count - boundaryPointCount - 1) {
				r = Mathf.Sqrt(i + 0.5f) / Mathf.Sqrt(count - boundaryPointCount + 1 / 2f);
			}
			var t = i * angleStride;
			points[i] = r * radius * new Vector2(Mathf.Cos(t), Mathf.Sin(t));
		}
		
		/*
		var offset = 2f / count;
		var increment = Mathf.PI * (3f - Mathf.Sqrt(5f));
		for (var i = 0; i < count; i++)
		{
			var y = i * offset - 1f + (offset / 2f);
			var r = Mathf.Sqrt(1f - y * y);
			var phi = i * increment;
			points[i] = new Vector2(Mathf.Cos(phi) * r, y) * radius;
		}
		*/

		return points;
	}
	
	/// Get evenly-distributed points in a circle - non-allocating
	// sunflower distribution, from https://stackoverflow.com/questions/28567166/uniformly-distribute-x-points-inside-a-circle
	public static void GetDistributedCirclePoints(ref Vector2[] points, float radius, float boundaryEvenness = 0f, bool geodesic = false)
	{
		/*
		var n = points.Length;
        
		var offset = 2f / n;
		var increment = Mathf.PI * (3f - Mathf.Sqrt(5f));
		for (var i = 0; i < n; i++)
		{
			var y = i * offset - 1f + (offset / 2f);
			var r = Mathf.Sqrt(1f - y * y);
			var phi = i * increment;
			points[i] = new Vector2(Mathf.Cos(phi) * r, y) * radius;
		}
		*/
		var count = points.Length;
		var angleStride = geodesic ? 360f * GOLDENRATIO : TWOPI / (PHI * PHI);
		//angleStride *= Mathf.Deg2Rad;
		var boundaryPointCount = boundaryEvenness <= 0f ? 0 : Mathf.RoundToInt(boundaryEvenness * Mathf.Sqrt(count));

		for (var i = 0; i < count; i++) {
			var r = 1f;
			if (i <= count - boundaryPointCount - 1) {
				r = Mathf.Sqrt(i + 0.5f) / Mathf.Sqrt(count - boundaryPointCount + 1 / 2f);
			}
			var t = i * angleStride;
			points[i] = r * radius * new Vector2(Mathf.Cos(t), Mathf.Sin(t));
		}
	}

	/// Returns a vector where x = radius, y = theta and z = phi
	public static Vector3 CartesianToSpherical(Vector3 cartesian)
	{
		return CartesianToSpherical(cartesian.x, cartesian.y, cartesian.z);
	}
	
	/// Returns a vector where x = radius, y = theta and z = phi
	public static Vector3 CartesianToSpherical(float x, float y, float z)
	{
		var xx = x * x;
		var yy = y * y;
		var zz = z * z;
		return new Vector3(Mathf.Sqrt(xx + yy + zz), Mathf.Atan2(Mathf.Sqrt(xx + zz), y), Mathf.Atan2(x, z));
	}

	/// Returns a spherical-coordinate vector (where x = radius, y = theta and z = phi) back into a normal cartesian vector
	public static Vector3 SphericalToCartesian(Vector3 spherical)
	{
		return SphericalToCartesian(spherical.x, spherical.y, spherical.z);
	}
	
	/// Converts a set of spherical coordinates to a cartesian vector
	public static Vector3 SphericalToCartesian(float radius, float theta, float phi)
	{
		var sP = Mathf.Sin(phi);
		return radius * new Vector3(Mathf.Sin(phi) * sP, Mathf.Cos(theta), Mathf.Cos(phi) * sP);
	}
	
	// <3 freya holmer https://www.youtube.com/watch?v=LSNQuFEDOyQ

	/// Lerp factor for frame-rate independent lerp smoothing. Use this as the third parameter in Mathf.Lerp.
	/// Low values of decay result in slow smoothing and vice-versa. Useful range is around 1 to 20.
	public static float FreyaLerpFactor(float decay)
	{
		return Mathf.Exp(-decay * Time.deltaTime);
	}
	
	/// Exponential-decay based, framerate-independent lerp. Decay tends to be in the 1 (very slow) to 20 (very fast) range.
	public static float FreyaLerp(float current, float target, float decay)
	{
		return target + (current - target) * Mathf.Exp(-decay * Time.deltaTime);
	}
	
	/// Exponential-decay based, framerate-independent lerp. Decay tends to be in the 1 (very slow) to 20 (very fast) range.
	/// This version doesn't go crazy when wrapping between 359° and 0°
	public static float FreyaLerpAngle(float current, float target, float decay)
	{
		if(target < 90f && current > 270f)
			current -= 360f;
		if (target > 270f && current < 90f)
			current += 360f;
		return target + (current - target) * Mathf.Exp(-decay * Time.deltaTime);
	}
	
	/// Exponential-decay based, framerate-independent lerp. Decay tends to be in the 1 (very slow) to 20 (very fast) range.
	public static Vector2 FreyaLerpVector2(Vector2 current, Vector2 target, float decay)
	{
		var e = Mathf.Exp(-decay * Time.deltaTime);
		return new Vector2(
			target.y + (current.y - target.y) * e,
			target.y + (current.y - target.y) * e);
	}
	
	/// Exponential-decay based, framerate-independent lerp. Decay tends to be in the 1 (very slow) to 20 (very fast) range.
	public static Vector3 FreyaLerpVector3(Vector3 current, Vector3 target, float decay)
	{
		var e = Mathf.Exp(-decay * Time.deltaTime);
		return new Vector3(
			target.x + (current.x - target.x) * e,
			target.y + (current.y - target.y) * e,
			target.z + (current.z - target.z) * e);
	}
	
	/// Exponential-decay based, framerate-independent lerp. Decay tends to be in the 1 (very slow) to 20 (very fast) range.
	public static Vector4 FreyaLerpVector4(Vector4 current, Vector4 target, float decay)
	{
		var e = Mathf.Exp(-decay * Time.deltaTime);
		return new Vector4(
			target.x + (current.x - target.x) * e,
			target.y + (current.y - target.y) * e,
			target.z + (current.z - target.z) * e,
			target.w + (current.w - target.w) * e);
	}
	
	/// Exponential-decay based, framerate-independent lerp. Decay tends to be in the 1 (very slow) to 20 (very fast) range.
	public static Quaternion FreyaLerpQuaternion(Quaternion current, Quaternion target, float decay)
	{
		var e = Mathf.Exp(-decay * Time.deltaTime);
		return Quaternion.Lerp(target, current, e);
	}
}