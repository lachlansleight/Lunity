using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiTextbox : QuickUiSceneControl
{

    private InputField _value;

    protected override void GetReferences()
    {
        base.GetReferences();
        _value = transform.Find("Value").GetComponent<InputField>();
    }

    public override void Initialize(QuickUiControl control)
    {
        base.Initialize(control);

        //var attributes = control.TargetMember.GetAttributes();
        //foreach (var attribute in attributes) {
        //}
        
        _value.onValueChanged.AddListener(newVal =>
        {
            var val = newVal;
            _control.SetValue(val);
        });

        _value.text = _control.GetValue().ToString();
    }
    
    public override void RefreshValue()
    {
        base.RefreshValue();
        _value.text = _control.GetValue().ToString();
    }
}
