using UnityEngine;

namespace Lunity
{
	public static class RectExtensions
	{
		/// Given a provided world-space direction, returns the point on the boundary of the rect that intersects with that line
		/// If the rect size is zero, returns the rect's center
		public static Vector2 GetPerimeterPoint(this Rect rect, Vector2 direction)
		{
			var mag = rect.size.magnitude * 2f;
			if (mag <= 0) return rect.center;
			
			var line = rect.center + direction.normalized * mag;
			var corners = new[]
			{
				new Vector2(rect.xMin, rect.yMin),
				new Vector2(rect.xMin, rect.yMax),
				new Vector2(rect.xMax, rect.yMax),
				new Vector2(rect.xMax, rect.yMin),
			};
			if (LunityMath.Get2DIntersectionBounded(rect.center, line, corners[0], corners[1], out var left)) {
				return left;
			}
			if (LunityMath.Get2DIntersectionBounded(rect.center, line, corners[1], corners[2], out var up)) {
				return up;
			}
			if (LunityMath.Get2DIntersectionBounded(rect.center, line, corners[2], corners[3], out var right)) {
				return right;
			}
			if (LunityMath.Get2DIntersectionBounded(rect.center, line, corners[3], corners[0], out var down)) {
				return down;
			}

			Debug.LogWarning("Somehow we failed to get a perimeter point for the provided rect?");
			return rect.center;
		}
	}
}