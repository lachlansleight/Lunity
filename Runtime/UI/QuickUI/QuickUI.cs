using System.Collections.Generic;
using System.Reflection;
using Lunity;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
public class QuickUI : MonoBehaviour
{
    public bool RefreshControlsOnAwake = true;
    [Tooltip("Only has an effect if RefreshControlsOnAwake is false")] public bool RefreshControlsOnStart = false;
    public RectTransform ControlParent;
    public QuickUiControl[] Controls;

    public QuickUiSceneControl[] SceneControls;

    public void Awake()
    {
        if(RefreshControlsOnAwake) RefreshControls();
    }

    public void Start()
    {
        if (RefreshControlsOnStart && !RefreshControlsOnAwake) RefreshControls();
    }
    
    public void ForceRefreshControls()
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

        CreateControls(true);
    }

    public void CreateControls(bool createObjects = true)
    {
        var controlList = new List<QuickUiSceneControl>();
        if (ControlParent == null) ControlParent = GetComponent<RectTransform>();
        
        foreach (var control in Controls) {
            if (control.HideControl) continue;
            control.Initialize();
            if (control.TargetMember.Type.IsEnum) {
                AddEnumControl(ref controlList, control, !createObjects);
                
                continue;
            }
            switch (control.TargetMember.Type.Name) {
                case "Single":
                case "Int32":
                    AddSliderControl(ref controlList, control, !createObjects);
                    break;
                case "Vector3":
                    AddVector3Control(ref controlList, control, !createObjects);
                    break;
                case "Boolean":
                    AddBooleanControl(ref controlList, control, !createObjects);
                    break;
                case "String":
                    AddTextboxControl(ref controlList, control, !createObjects);
                    break;
                case "Void" :
                    AddButtonControl(ref controlList, control, !createObjects);
                    break;
                default:
                    Debug.LogError("QuickUI does not support type " + control.TargetMember.Type.Name + " for member " + control.TargetMember.Name);
                    break;
            }
        }

        SceneControls = controlList.ToArray();
    }

    [ContextMenu("Refresh Values")]
    public void RefreshValues()
    {
        foreach (var control in SceneControls) {
            control.RefreshValue();
        }
    }

    private void AddSliderControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control, bool alreadyExists = false)
    {
        AddControl<QuickUiSlider>(ref controlList, "QuickUiSlider", control, alreadyExists);
    }
    
    private void AddVector3Control(ref List<QuickUiSceneControl> controlList, QuickUiControl control, bool alreadyExists = false)
    {
        AddControl<QuickUiVector3>(ref controlList, "QuickUiVector3", control, alreadyExists);
    }

    private void AddEnumControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control, bool alreadyExists = false)
    {
        AddControl<QuickUiDropdown>(ref controlList, "QuickUiDropdown", control, alreadyExists);
    }
    
    private void AddBooleanControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control, bool alreadyExists = false)
    {
        AddControl<QuickUiToggle>(ref controlList, "QuickUiToggle", control, alreadyExists);
    }
    
    private void AddTextboxControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control, bool alreadyExists = false)
    {
        AddControl<QuickUiTextbox>(ref controlList, "QuickUiTextbox", control, alreadyExists);
    }

    private void AddButtonControl(ref List<QuickUiSceneControl> controlList, QuickUiControl control, bool alreadyExists = false)
    {
        AddControl<QuickUiButton>(ref controlList, "QuickUiButton", control, alreadyExists);
    }

    private void AddControl<T>(ref List<QuickUiSceneControl> controlList, string prefabName, QuickUiControl control, bool alreadyExists = false) where T : QuickUiSceneControl
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

        if (space != null && !alreadyExists) {
#if UNITY_EDITOR
            AddSpace(ref controlList, "QuickUiSpace", (float)space.ConstructorArguments[0].Value);
#elif UNITY_IOS
            Debug.LogWarning("Attribute constructor arguments are not supported on iOS (required for QuickUiSpace)");
            AddSpace(ref controlList, "QuickUiSpace", 10);
#else
            AddSpace(ref controlList, "QuickUiSpace", (float)space.ConstructorArguments[0].Value);
#endif
        }

        if (header != null && !alreadyExists) {
            if (space != null && !alreadyExists) {
#if UNITY_EDITOR
                AddHeader(ref controlList, "QuickUiHeader", (string)header.ConstructorArguments[0].Value);
#elif UNITY_IOS
            Debug.LogWarning("Attribute constructor arguments are not supported on iOS (required for QuickUiHeader)");
#else
            AddHeader(ref controlList, "QuickUiHeader", (string)header.ConstructorArguments[0].Value);
#endif
            }
        }

        if (alreadyExists) {
            //note - this will break if multiple members have the same name...
            var sceneControl = transform.Find(control.TargetMemberName).GetComponent<T>();
            sceneControl.Initialize(control);
            controlList.Add(sceneControl);
        } else {
#if UNITY_EDITOR
            var newObj = PrefabUtility.InstantiatePrefab(Resources.Load(prefabName)) as GameObject;
#else
            var newObj = Instantiate(Resources.Load(prefabName)) as GameObject;
#endif
        
            if (newObj == null) {
                Debug.LogError("Failed to instantiate QuickUi object " + prefabName);
                return;
            }

            ((RectTransform) newObj.transform).SetParentNeutral(ControlParent);
            newObj.name = control.TargetMemberName;

            var sceneControl = newObj.AddComponent<T>();
            sceneControl.Initialize(control);

            controlList.Add(sceneControl);   
        }
    }

    private void AddSpace(ref List<QuickUiSceneControl> controlList, string prefabName, float height)
    {
        var newObj = Instantiate(Resources.Load(prefabName)) as GameObject;
        if (newObj == null) {
            Debug.LogError("Failed to instantiate QuickUi object " + prefabName);
            return;
        }
        ((RectTransform) newObj.transform).SetParentNeutral(ControlParent);

        var sceneControl = newObj.AddComponent<QuickUiSpace>();
        sceneControl.Initialize(height);
        
        controlList.Add(sceneControl);
    }

    private void AddHeader(ref List<QuickUiSceneControl> controlList, string prefabName, string text)
    {
        var newObj = Instantiate(Resources.Load(prefabName)) as GameObject;
        if (newObj == null) {
            Debug.LogError("Failed to instantiate QuickUi object " + prefabName);
            return;
        }
        ((RectTransform) newObj.transform).SetParentNeutral(ControlParent);

        var sceneControl = newObj.AddComponent<QuickUiHeader>();
        sceneControl.Initialize(text);
        
        controlList.Add(sceneControl);
    }
}
