using UnityEngine;

namespace Lunity
{
	public static class ObjectExtensions
	{
		public static void DestroyFlexible(this Object obj)
		{
			if (Application.isPlaying) Object.Destroy(obj);
			else Object.DestroyImmediate(obj);
		}
	}

	public static class LObject
	{
		public static void DestroyFlexible(Object obj)
		{
			if (Application.isPlaying) Object.Destroy(obj);
			else Object.DestroyImmediate(obj);
		}
	}
}