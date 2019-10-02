using UnityEditor;
using UnityEngine;

namespace Lunity
{
	public static class LineRendererCircleGenerator
	{
		[MenuItem("CONTEXT/LineRenderer/Set Circle (6)")]
		private static void SetCircle6(MenuCommand command)
		{
			SetCircle((LineRenderer) command.context, 6);
		}

		[MenuItem("CONTEXT/LineRenderer/Set Circle (12)")]
		private static void SetCircle12(MenuCommand command)
		{
			SetCircle((LineRenderer) command.context, 12);
		}

		[MenuItem("CONTEXT/LineRenderer/Set Circle (24)")]
		private static void SetCircle24(MenuCommand command)
		{
			SetCircle((LineRenderer) command.context, 24);
		}

		[MenuItem("CONTEXT/LineRenderer/Set Circle (48)")]
		private static void SetCircle48(MenuCommand command)
		{
			SetCircle((LineRenderer) command.context, 48);
		}

		private static void SetCircle(LineRenderer line, int count)
		{
			line.positionCount = count;
			for (var i = 0; i < count; i++) {
				var iT = ((float) i / count) * Mathf.PI * 2f;
				line.SetPosition(i, new Vector3(0.5f * Mathf.Cos(iT), 0.5f * Mathf.Sin(iT), 0f));
			}

			line.loop = true;
		}
	}
}