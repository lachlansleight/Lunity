using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiSlider : QuickUiSceneControl
{

    private Slider _slider;
    private InputField _value;

    protected override void GetReferences()
    {
        base.GetReferences();
        _slider = transform.Find("Slider").GetComponent<Slider>();
        _value = transform.Find("Value").GetComponent<InputField>();
    }

    public override void Initialize(QuickUiControl control)
    {
        base.Initialize(control);

        var hasRange = false;
        var attributes = control.TargetMember.GetAttributes();
        foreach (var attribute in attributes) {
            if (attribute.AttributeType != typeof(RangeAttribute)) continue;
            
            _slider.minValue = (float)attribute.ConstructorArguments[0].Value;
            _slider.maxValue = (float)attribute.ConstructorArguments[1].Value;
            hasRange = true;
        }

        if (hasRange) {
            _slider.value = (float) _control.GetValue();
            _slider.onValueChanged.AddListener(val =>
            {
                _control.SetValue(val);
                _value.text = val.ToString(CultureInfo.InvariantCulture);
            });
        } else {
            var valueRt = _value.GetComponent<RectTransform>();
            valueRt.anchorMin = new Vector2(0.35f, 0f);
            valueRt.offsetMin = Vector2.zero;
            _slider.gameObject.SetActive(false);
        }
        
        _value.onValueChanged.AddListener(newVal =>
        {
            var val = float.Parse(newVal);
            _control.SetValue(val);
            _slider.value = val;
        });

        _value.text = _control.GetValue().ToString();
    }
    
    public override void RefreshValue()
    {
        base.RefreshValue();
        _slider.value = (float) _control.GetValue();
        _value.text = _control.GetValue().ToString();
    }
}
