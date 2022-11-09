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
            
            if (control.TargetMember.Type == typeof(int)) _slider.wholeNumbers = true;
            _slider.minValue = (float)attribute.ConstructorArguments[0].Value;
            _slider.maxValue = (float)attribute.ConstructorArguments[1].Value;
            hasRange = true;
        }

        if (hasRange) {
            if (control.TargetMember.Type == typeof(int)) _slider.value = (int) _control.GetValue();
            else _slider.value = (float) _control.GetValue();
            _slider.onValueChanged.AddListener(val =>
            {
                if (control.TargetMember.Type == typeof(int)) {
                    _control.SetValue((int)val);
                    _slider.value = val;
                } else {
                    _control.SetValue(val);
                    _slider.value = val;
                }
                
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
            if (control.TargetMember.Type == typeof(int)) {
                var val = int.Parse(newVal);
                _control.SetValue(val);
                _slider.value = val;
            } else {
                var val = float.Parse(newVal);
                _control.SetValue(val);
                _slider.value = val;
            }
        });

        _value.text = _control.GetValue().ToString();
    }
    
    public override void RefreshValue()
    {
        base.RefreshValue();
        if (_control.TargetMember.Type == typeof(int)) _slider.value = (int) _control.GetValue();
        else _slider.value = (float) _control.GetValue();
        _value.text = _control.GetValue().ToString();
    }
}
