using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
	/// <summary>
	/// Stick this on a method
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class EditorButtonAttribute : PropertyAttribute
	{
	}
}