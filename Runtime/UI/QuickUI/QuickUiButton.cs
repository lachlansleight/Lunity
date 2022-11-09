using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiButton : QuickUiSceneControl
{

    private Button _button;

    protected override void GetReferences()
    {
        //note - we don't call the base method since it expects Label to be in the top-level
        _label = transform.Find("Button/Label").GetComponent<Text>();
        _button = transform.Find("Button").GetComponent<Button>();
    }

    public override void Initialize(QuickUiControl control)
    {
        base.Initialize(control);
        _button.onClick.AddListener(control.Invoke);
    }
}
