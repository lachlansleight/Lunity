using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
	public static class LDebug
	{
		public static void Log(string message)
		{
			Debug.Log(message);
		}
		public static void Log(string message, Color color)
		{
			var colorString = "#" + ColorUtility.ToHtmlStringRGBA(color);
			Debug.Log($"<color=\"{colorString}\">{message}</color>");
		}
		public static void Log(string message, Object context)
		{
			Debug.Log(message, context);
		}
		public static void Log(string message, Object context, Color color)
		{
			var colorString = "#" + ColorUtility.ToHtmlStringRGBA(color);
			Debug.Log($"<color=\"{colorString}\">{message}</color>", context);
		}
		
		
		public static void LogWarning(string message)
		{
			Debug.LogWarning(message);
		}
		public static void LogWarning(string message, Color color)
		{
			var colorString = "#" + ColorUtility.ToHtmlStringRGBA(color);
			Debug.LogWarning($"<color=\"{colorString}\">{message}</color>");
		}
		public static void LogWarning(string message, Object context)
		{
			Debug.LogWarning(message, context);
		}
		public static void LogWarning(string message, Object context, Color color)
		{
			var colorString = "#" + ColorUtility.ToHtmlStringRGBA(color);
			Debug.LogWarning($"<color=\"{colorString}\">{message}</color>", context);
		}
		
		
		public static void LogError(string message)
		{
			Debug.LogError(message);
		}
		public static void LogError(string message, Color color)
		{
			var colorString = "#" + ColorUtility.ToHtmlStringRGBA(color);
			Debug.LogError($"<color=\"{colorString}\">{message}</color>");
		}
		public static void LogError(string message, Object context)
		{
			Debug.LogError(message, context);
		}
		public static void LogError(string message, Object context, Color color)
		{
			var colorString = "#" + ColorUtility.ToHtmlStringRGBA(color);
			Debug.LogError($"<color=\"{colorString}\">{message}</color>", context);
		}
	}
}