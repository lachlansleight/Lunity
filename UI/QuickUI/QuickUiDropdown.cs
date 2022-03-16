using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiDropdown : QuickUiSceneControl
{

	private Dropdown _dropdown;

	private string[] _names;
	private int[] _values;

	protected override void GetReferences()
	{
		base.GetReferences();
		
		_dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
		
		_control.GetEnumLists(out var names, out var values);
		_names = new string[names.Length];
		_values = new int[names.Length];
		var options = new List<Dropdown.OptionData>();
		for (var i = 0; i < names.Length; i++) {
			_names[i] = names[i];
			_values[i] = values[i];
			options.Add(new Dropdown.OptionData(_names[i]));
		}

		_dropdown.options = options;
		_dropdown.value = (int)_control.GetValue();
		_dropdown.RefreshShownValue();
	}

	public override void Initialize(QuickUiControl control)
	{
		base.Initialize(control);
		
		_dropdown.value = (int) _control.GetValue();
		_dropdown.onValueChanged.AddListener(val =>
		{
			_control.SetValue(_values[val]);
		});
	}

	public override void RefreshValue()
	{
		base.RefreshValue();
		_dropdown.value = (int)_control.GetValue();
		_dropdown.RefreshShownValue();
	}
}