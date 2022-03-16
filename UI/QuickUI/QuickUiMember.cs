using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum QuickUiMemberType
{
	Field,
	Property,
	Method
}

public abstract class QuickUiMember
{
	public string Name;
	public Type Type;

	public virtual IEnumerable<CustomAttributeData> GetAttributes()
	{
		return null;
	}
}

public class QuickUiValueMember : QuickUiMember
{
	public virtual object GetValue(object obj)
	{
		return null;
	}

	public virtual void SetValue(object obj, object value) {}
}

public class QuickUiFieldMember : QuickUiValueMember
{

	private FieldInfo _field;

	public QuickUiFieldMember(object obj, string name)
	{
		Name = name;
		_field = obj.GetType().GetField(Name);
		if (_field == null) {
			Debug.LogError("Field " + Name + " not found on object " + obj);
		} else {
			Type = _field.FieldType;
		}
	}
	
	public override object GetValue(object obj)
	{
		return _field.GetValue(obj);
	}

	public override void SetValue(object obj, object value)
	{
		_field.SetValue(obj, value);
	}
	
	public override IEnumerable<CustomAttributeData> GetAttributes()
	{
		return _field.CustomAttributes;
	}
}

public class QuickUiPropertyMember : QuickUiValueMember
{
	private PropertyInfo _property;

	public QuickUiPropertyMember(object obj, string name)
	{
		Name = name;
		_property = obj.GetType().GetProperty(Name);
		if (_property == null) {
			Debug.LogError("Property " + Name + " not found on object " + obj);
		} else {
			Type = _property.PropertyType;
		}
	}

	public override object GetValue(object obj)
	{
		return _property.GetValue(obj);
	}

	public override void SetValue(object obj, object value)
	{
		_property.SetValue(obj, value);
	}

	public override IEnumerable<CustomAttributeData> GetAttributes()
	{
		return _property.CustomAttributes;
	}
}

public class QuickUiMethodMember : QuickUiMember
{
	private MethodInfo _method;

	public QuickUiMethodMember(object obj, string name)
	{
		Name = name;
		_method = obj.GetType().GetMethod(Name);
		if (_method == null) {
			Debug.LogError("Method " + Name + " not found on object " + obj);
		} else {
			Type = _method.ReturnType;
		}
	}
	
	public void Invoke(object obj, object[] parameters)
	{
		_method.Invoke(obj, parameters);
	}
	
	public override IEnumerable<CustomAttributeData> GetAttributes()
	{
		return _method.CustomAttributes;
	}
}
