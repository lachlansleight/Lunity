using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Lunity
{
	public static class MiscExtensions
	{
		//https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
		public static T GetCopyOf<T>(this Component comp, T other) where T : Component
		{
			var type = comp.GetType();
			if (type != other.GetType()) return null; // type mis-match
			var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
			var pinfos = type.GetProperties(flags);
			foreach (var pinfo in pinfos) {
				if (!pinfo.CanWrite) continue;
				var obsolete = false;
				var attrData = pinfo.CustomAttributes;
				foreach (var data in attrData) {
					if (data.AttributeType != typeof(System.ObsoleteAttribute)) continue;
					obsolete = true;
					break;
				}
				if (obsolete) continue;
				
				try {
					pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
				} catch {
					// In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
				}
			}
			var finfos = type.GetFields(flags);
			foreach (var finfo in finfos) {
				finfo.SetValue(comp, finfo.GetValue(other));
			}
			return comp as T;
		}
		
		//https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
		public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
		{
			return go.AddComponent<T>().GetCopyOf(toAdd) as T;
		}
	}
}