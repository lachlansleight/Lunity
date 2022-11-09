using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class QuickUiControl
{
	//Configuration
	public GameObject TargetObject;
	public string TargetComponentName;
	public QuickUiMemberType TargetMemberType;
	public string TargetMemberName;
	public bool HideControl;
	
	//Runtime (Populated On Awake)
	public Component TargetComponent;
	public QuickUiMember TargetMember;
	
	private const BindingFlags OnlyLookup = BindingFlags.Public | 
	                                        BindingFlags.Instance | 
	                                        BindingFlags.Static | 
	                                        BindingFlags.DeclaredOnly;

	public object GetValue()
	{
		if (TargetMember is QuickUiValueMember valueMember) return valueMember.GetValue(TargetComponent);
		
		return null;
	}

	public void SetValue(object value)
	{
		if (TargetMember is QuickUiValueMember valueMember) {
			valueMember.SetValue(TargetComponent, value);
		}
	}

	public void Invoke()
	{
		if (TargetMember is QuickUiMethodMember methodMember) {
			methodMember.Invoke(TargetComponent, null);
		}
	}
	
	public void Initialize()
	{
		try {
			TargetComponent = GetTargetComponent(TargetComponentName);

			if (TargetComponent == null) {
				Debug.LogError("Couldn't find component with name " + TargetComponentName);
				return;
			}

			switch (TargetMemberType) {
				case QuickUiMemberType.Field:
					TargetMember = new QuickUiFieldMember(TargetComponent, TargetMemberName);
					break;
				case QuickUiMemberType.Property:
					TargetMember = new QuickUiPropertyMember(TargetComponent, TargetMemberName);
					break;
				case QuickUiMemberType.Method:
					TargetMember = new QuickUiMethodMember(TargetComponent, TargetMemberName);
					break;
			}

			if (TargetMember == null) {
				throw new UnityException("Couldn't find member " + TargetMemberName + " on component " + TargetComponent);
			}
		} catch (Exception e) {
			Debug.LogError("Failed to initialize QuickUiControl (member " + TargetMemberName + " on component " + TargetComponent + ")\n" + e.Message, TargetObject);
			throw;
		}
	}

	private Component GetTargetComponent(string componentName)
	{
		var components = TargetObject.GetComponents<Component>();
		return components.FirstOrDefault(c => c.GetType().Name == componentName);
	}

	public static Component GetTargetComponent(GameObject gameObj, string componentName)
	{
		var components = gameObj.GetComponents<Component>();
		return components.FirstOrDefault(c => c.GetType().Name == componentName);
	}

	private FieldInfo GetTargetField(string fieldName)
	{
		var fields = TargetComponent.GetType().GetFields(OnlyLookup);
		return fields.FirstOrDefault(c => c.Name == fieldName);
	}
	
	private PropertyInfo GetTargetProperty(string propertyName)
	{
		var fields = TargetComponent.GetType().GetProperties(OnlyLookup);
		return fields.FirstOrDefault(c => c.Name == propertyName);
	}
	
	public static bool GetComponentEnum(GameObject targetObject, out string[] names, out int[] indices)
	{
		try {
			var componentList = targetObject.GetComponents<Component>();
			names = new string[componentList.Length];
			indices = new int[componentList.Length];
			for (var i = 0; i < componentList.Length; i++) {
				names[i] = componentList[i].GetType().Name;
				indices[i] = i;
			}
		} catch (System.Exception e) {
			Debug.LogError("Failed for some reason: " + e);
			names = new [] {"Error"};
			indices = new [] {-1};
			return false;
		}

		return true;
	}

	public static bool GetFieldsAndPropertiesEnum(Type targetType, out string[] names, out int[] indices)
	{
		try {
			var fields = targetType.GetFields(OnlyLookup);
			var properties = targetType.GetProperties(OnlyLookup);
			var output = new string[fields.Length + properties.Length];
			names = new string[output.Length];
			indices = new int[output.Length];
			for (var i = 0; i < output.Length; i++) {
				names[i] = i < fields.Length ? fields[i].Name : properties[i - fields.Length].Name;
				indices[i] = i;
			}
		} catch (Exception e) {
			Debug.LogError("Failed for some reason: " + e);
			names = new [] {"Error"};
			indices = new [] {-1};
			return false;
		}

		return true;
	}

	public static bool GetMemberNameOptions(Type targetType, QuickUiMemberType memberType, out string[] names, out int[] indices)
	{
		switch (memberType) {
			case QuickUiMemberType.Field:
				var fields = targetType.GetFields(OnlyLookup);
				names = new string[fields.Length];
				indices = new int[fields.Length];
				for (var i = 0; i < names.Length; i++) {
					names[i] = fields[i].Name;
					indices[i] = i;
				}
				break;
			case QuickUiMemberType.Property:
				var properties = targetType.GetProperties(OnlyLookup);
				names = new string[properties.Length];
				indices = new int[properties.Length];
				for (var i = 0; i < names.Length; i++) {
					names[i] = properties[i].Name;
					indices[i] = i;
				}
				break;
			case QuickUiMemberType.Method:
				var methods = targetType.GetMethods(OnlyLookup);
				names = new string[methods.Length];
				indices = new int[methods.Length];
				for (var i = 0; i < names.Length; i++) {
					names[i] = methods[i].Name;
					indices[i] = i;
				}
				break;
			default:
				Debug.LogError("Invalid memberType " + memberType + " provided to GetMemberNameOptions");
				names = new [] {"Error"};
				indices = new [] {-1};
				return false;
		}

		return true;
	}

	public void GetEnumLists(out string[] names, out int[] values)
	{
		if (!TargetMember.Type.IsEnum) {
			Debug.LogError("Error - couldn't get enum lists for type " + TargetMember.Type.Name);
			names = null;
			values = null;
			return;
		}

		names = Enum.GetNames(TargetMember.Type);
		values = (int[])Enum.GetValues(TargetMember.Type);
	}

}
