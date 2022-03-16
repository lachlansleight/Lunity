using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

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

		public static void DrawBounds(Vector3 center, Vector3 size)
		{
			DrawBounds(center, size, Color.white);
		}
		
		public static void DrawBounds(Vector3 center, Vector3 size, Color color, float duration = 0f)
		{
			var min = center - size;
			var max = center + size;
			var points = new[]
			{
				new Vector3(min.x, min.y, min.z),
				new Vector3(min.x, min.y, max.z),
				new Vector3(max.x, min.y, max.z),
				new Vector3(max.x, min.y, min.z),
				new Vector3(min.x, max.y, min.z),
				new Vector3(min.x, max.y, max.z),
				new Vector3(max.x, max.y, max.z),
				new Vector3(max.x, max.y, min.z),
			};
			if (duration > 0f) {
				//bottom
				Debug.DrawLine(points[0], points[1], color, duration);
				Debug.DrawLine(points[1], points[2], color, duration);
				Debug.DrawLine(points[2], points[3], color, duration);
				Debug.DrawLine(points[3], points[0], color, duration);
				
				//sides
				Debug.DrawLine(points[0], points[4], color, duration);
				Debug.DrawLine(points[1], points[5], color, duration);
				Debug.DrawLine(points[2], points[6], color, duration);
				Debug.DrawLine(points[3], points[7], color, duration);
				
				//top
				Debug.DrawLine(points[4], points[5], color, duration);
				Debug.DrawLine(points[5], points[6], color, duration);
				Debug.DrawLine(points[6], points[7], color, duration);
				Debug.DrawLine(points[7], points[4], color, duration);
			} else {
				//bottom
				Debug.DrawLine(points[0], points[1], color);
				Debug.DrawLine(points[1], points[2], color);
				Debug.DrawLine(points[2], points[3], color);
				Debug.DrawLine(points[3], points[0], color);
				
				//sides
				Debug.DrawLine(points[0], points[4], color);
				Debug.DrawLine(points[1], points[5], color);
				Debug.DrawLine(points[2], points[6], color);
				Debug.DrawLine(points[3], points[7], color);
				
				//top
				Debug.DrawLine(points[4], points[5], color);
				Debug.DrawLine(points[5], points[6], color);
				Debug.DrawLine(points[6], points[7], color);
				Debug.DrawLine(points[7], points[4], color);
			}
		}

		private static Shader _unlitShader;
		private static Mesh _cylinderMesh;
		public static void DrawWorldLine(Vector3 start, Vector3 end)
		{
			DrawWorldLine(start, end, Color.white);
		}
		public static void DrawWorldLine(Vector3 start, Vector3 end, Color color)
		{
			if (_unlitShader == null) _unlitShader = Shader.Find("Unlit/Color");
			var mat = new Material(_unlitShader);
			mat.color = color;

			if (_cylinderMesh == null) {
				var temp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				_cylinderMesh = temp.GetComponent<MeshFilter>().sharedMesh;
				if (Application.isPlaying) Object.Destroy(temp);
				else Object.DestroyImmediate(temp);
			}

			Graphics.DrawMesh(_cylinderMesh, Matrix4x4.TRS(
				(start + end) * 0.5f, 
				Quaternion.LookRotation(end - start, Vector3.up) * Quaternion.Euler(90f, 0f, 0f), 
				new Vector3(0.005f, (end - start).magnitude * 0.5f, 0.005f)), mat, 0);
		}

		public static void LogObject(string message = "", object var = null)
		{
			var output = message;
			if (var == null) {
				Debug.Log(output);
				return;
			}

			Debug.Log(output + " " + GetObjectString(var));
		}

		public static string GetObjectString(object var = null, int indent = 0)
		{
			try {
				var tabs = "";
				for (var i = 0; i < indent; i++) tabs += "\t";

				if (var == null) {
					return tabs + "null";
				}

				var type = var.GetType();

				if (type.IsPrimitive) {
					return tabs + var.ToString();
				}

				if (type.IsArray) {
					var arr = (Array) var;
					var arrayOutput = "[";
					foreach (var item in arr) {
						arrayOutput += "\n" + tabs + "\t" + GetObjectString(item, 1) + ",";
					}

					arrayOutput += tabs + "]";
					return arrayOutput;
				}

				if (type == typeof(Dictionary<object, object>)) {
					var dict = (Dictionary<object, object>) var;
					var dictOutput = "[";
					foreach (var pair in dict) {
						dictOutput += "\n" + tabs + "\t" + pair.Key + ": " + GetObjectString(pair.Value, indent + 1) +
						              ",";
					}

					dictOutput += tabs + "]";
					return dictOutput;
				}

				var fields = type.GetFields();
				var properties = type.GetProperties();
				if (fields.Length == 0 && properties.Length == 0) return "{}";

				var output = "{";
				foreach (var field in fields) {
					output += "\n" + tabs + "\t" + field.Name + ": " +
					          GetObjectString(field.GetValue(var), indent + 1) + ",";
				}
				foreach (var property in properties) {
					output += "\n" + tabs + "\t" + property.Name + ": " +
					          GetObjectString(property.GetValue(var), indent + 1) + ",";
				}

				output += tabs + "}";
				return output;
			} catch {
				return "Failed to stringify object of type " + (var == null ? "null" : var.GetType().Name);
			}
		}
	}
}