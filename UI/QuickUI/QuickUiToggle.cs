using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiToggle : QuickUiSceneControl
{

    private Toggle _toggle;

    protected override void GetReferences()
    {
        base.GetReferences();
        _toggle = transform.Find("Toggle").GetComponent<Toggle>();
    }

    public override void Initialize(QuickUiControl control)
    {
        base.Initialize(control);

        var attributes = control.TargetMember.GetAttributes();
        
        _toggle.onValueChanged.AddListener(newVal =>
        {
            _control.SetValue(newVal);
        });
    }
    
    public override void RefreshValue()
    {
        base.RefreshValue();
        _toggle.isOn = (bool) _control.GetValue();
    }
}
