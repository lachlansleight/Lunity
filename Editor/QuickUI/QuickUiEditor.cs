using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
	
[CustomEditor(typeof(QuickUI))]
public class QuickUiEditor : Editor
{

	private SerializedProperty _controls;
	private ReorderableList _controlList;

	public void OnEnable()
	{
		_controls = serializedObject.FindProperty("Controls");
		_controlList = new ReorderableList(serializedObject, _controls, true, true, true, true);

		_controlList.drawElementCallback = DrawListItems;
		_controlList.drawHeaderCallback = DrawHeader;
	}
	
	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		serializedObject.Update();
		_controlList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
		if (GUILayout.Button("Refresh Controls")) {
			((QuickUI) serializedObject.targetObject).RefreshControls();			
		}
	}
	
	void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
	{
		var element = _controlList.serializedProperty.GetArrayElementAtIndex(index);
		var rawElement = ((QuickUI) serializedObject.targetObject).Controls[index];
		var label = "No Target";
		if (rawElement.TargetObject != null && rawElement.TargetComponentName != null && rawElement.TargetMemberName != "") {
			label = $"{rawElement.TargetObject.name} -> {rawElement.TargetComponentName}/{rawElement.TargetMemberName}";
		}
		EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), label);
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element);
	}

	private void DrawHeader(Rect rect)
	{
		EditorGUI.LabelField(rect, "Controls");
	}
}
