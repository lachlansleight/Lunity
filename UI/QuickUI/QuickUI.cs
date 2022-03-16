using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
public class QuickUI : MonoBehaviour
{

    public RectTransform ControlParent;
    public QuickUiControl[] Controls;

    public QuickUiSceneControl[] SceneControls;

    public void Awake()
    {
        RefreshControls();
    }

    [ContextMenu("Force Refresh")]
    public void RefreshControls()
    {
        if (SceneControls != null) {
            foreach (var control in SceneControls) {
                if (control != null && control.gameObject != null) {
                    if(Application.isPlaying) Destroy(control.gameObject);
                    else if(Application.isEditor) DestroyImmediate(control.gameObject);
                }
            }
        }

        CreateControls();
    }

    public void CreateControls()
    {
        var controlList = new List<QuickUiSceneControl>();
        if (ControlParent == null) ControlParent = GetComponent<RectTransform>();
        
        foreach (var control in Controls) {
            if (control.HideControl) continue;
            control.Initialize();
            if (control.TargetMember.Type.IsEnum) {
                AddEnumControl(ref controlList, control);
                
                continue;
            }
            switch (control.TargetMember.Type.Name) {
                case "Single":
                    AddSliderControl(ref controlList, control);
                    break;
                case "Vector3":
                    AddVector3Control(ref controlList, control);
                    break;
                case "Boolean":
                    AddBooleanControl(ref controlList, control);
                    break;
                case "Void" :
                    AddButtonControl(ref controlList, control);
                    break;
                default:
                    Debug.LogError("QuickUI does not support type " + control.TargetMember.Type.Name + " for member " + control.TargetMember.Name);
                    break;
            }
        }

        SceneControls = controlList.ToArray();
    }

    public void RefreshValues()
    {
        foreach (var control in SceneControls) {
            control.RefreshValue();
        }
    }

    private void AddSliderControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control)
    {
        AddControl<QuickUiSlider>(ref controlList, "QuickUiSlider", control);
    }
    
    private void AddVector3Control(ref List<QuickUiSceneControl> controlList, QuickUiControl control)
    {
        AddControl<QuickUiVector3>(ref controlList, "QuickUiVector3", control);
    }

    private void AddEnumControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control)
    {
        AddControl<QuickUiDropdown>(ref controlList, "QuickUiDropdown", control);
    }
    
    private void AddBooleanControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control)
    {
        AddControl<QuickUiToggle>(ref controlList, "QuickUiToggle", control);
    }

    private void AddButtonControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control)
    {
        AddControl<QuickUiButton>(ref controlList, "QuickUiButton", control);
    }

    private void AddControl<T>(ref List<QuickUiSceneControl> controlList, string prefabName, QuickUiControl control) where T : QuickUiSceneControl
    {
        var attributes = control.TargetMember.GetAttributes();
        CustomAttributeData space = null;
        CustomAttributeData header = null;
        foreach (var attribute in attributes) {
            if (attribute.AttributeType == typeof(SpaceAttribute)) {
                space = attribute;
            }
            if (attribute.AttributeType == typeof(HeaderAttribute)) {
                header = attribute;
            }
        }

        if (space != null) {
            AddSpace(ref controlList, "QuickUiSpace", (float)space.ConstructorArguments[0].Value);
        }

        if (header != null) {
            AddHeader(ref controlList, "QuickUiHeader", (string)header.ConstructorArguments[0].Value);
        }
        
        #if UNITY_EDITOR
        var newObj = PrefabUtility.InstantiatePrefab(Resources.Load(prefabName)) as GameObject;
        #else
        var newObj = Instantiate(Resources.Load(prefabName)) as GameObject;
        #endif
        
        if (newObj == null) {
            Debug.LogError("Failed to instantiate QuickUi object " + prefabName);
            return;
        }
        newObj.transform.SetParent(ControlParent);
        newObj.transform.localScale = Vector3.one;
        newObj.name = control.TargetMemberName;

        var sceneControl = newObj.AddComponent<T>();
        sceneControl.Initialize(control);

        controlList.Add(sceneControl);
    }

    private void AddSpace(ref List<QuickUiSceneControl> controlList, string prefabName, float height)
    {
        var newObj = Instantiate(Resources.Load(prefabName)) as GameObject;
        if (newObj == null) {
            Debug.LogError("Failed to instantiate QuickUi object " + prefabName);
            return;
        }
        newObj.transform.SetParent(ControlParent);
        newObj.transform.localScale = Vector3.one;

        var sceneControl = newObj.AddComponent<QuickUiSpace>();
        sceneControl.Initialize(height * 2f);
        
        controlList.Add(sceneControl);
    }

    private void AddHeader(ref List<QuickUiSceneControl> controlList, string prefabName, string text)
    {
        var newObj = Instantiate(Resources.Load(prefabName)) as GameObject;
        if (newObj == null) {
            Debug.LogError("Failed to instantiate QuickUi object " + prefabName);
            return;
        }
        newObj.transform.SetParent(ControlParent);
        newObj.transform.localScale = Vector3.one;

        var sceneControl = newObj.AddComponent<QuickUiHeader>();
        sceneControl.Initialize(text);
        
        controlList.Add(sceneControl);
    }
}
