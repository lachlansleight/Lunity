using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiSceneControl : MonoBehaviour
{

    protected QuickUiControl _control;
    
    protected Text _label;
    protected virtual void GetReferences()
    {
        _label = transform.Find("Label").GetComponent<Text>();
    }
    
    public virtual void Initialize(QuickUiControl control)
    {
        _control = control;
        
        GetReferences();
        _label.text = control.TargetMember.Name;

        gameObject.name = control.TargetMember.Name;
    }

    public virtual void RefreshValue()
    {
        
    }
}
