using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiVector3 : QuickUiSceneControl
{
	private InputField[] _values;

	protected override void GetReferences()
	{
		base.GetReferences();
		_values = new[]
		{
			transform.Find("ValueX").GetComponent<InputField>(),
			transform.Find("ValueY").GetComponent<InputField>(),
			transform.Find("ValueZ").GetComponent<InputField>()
		};
	}

	public override void Initialize(QuickUiControl control)
	{
		base.Initialize(control);

		var value = (Vector3) _control.GetValue();
		for (var i = 0; i < 3; i++) {
			_values[i].text = value[i].ToString("0.00");
			var i1 = i;
			_values[i].onValueChanged.AddListener(newVal =>
			{
				var currentValue = (Vector3) _control.GetValue();
				currentValue[i1] = float.Parse(newVal);
				_control.SetValue(currentValue);
			});
		}
	}
	
	public override void RefreshValue()
	{
		base.RefreshValue();
		var value = (Vector3) _control.GetValue();
		for (var i = 0; i < 3; i++) {
			_values[i].text = value[i].ToString("0.00");
		}
	}
}
