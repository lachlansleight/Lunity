using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(QuickUiControl))]
public class QuickUiControlDrawer : PropertyDrawer
{
   
   private SerializedProperty _targetObject;
   private SerializedProperty _hideControl;
   private SerializedProperty _targetComponentName;
   private SerializedProperty _targetMemberType;
   private SerializedProperty _targetMemberName;

   private void GetProperties(SerializedProperty property)
   {
      _targetObject = property.FindPropertyRelative("TargetObject");
      _hideControl = property.FindPropertyRelative("HideControl");
      _targetComponentName = property.FindPropertyRelative("TargetComponentName");
      _targetMemberType = property.FindPropertyRelative("TargetMemberType");
      _targetMemberName = property.FindPropertyRelative("TargetMemberName");
   }
   
   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
   {
      GetProperties(property);
      
      EditorGUI.BeginProperty(position, label, property);

      if (!DrawGameObjectUi(out var targetGameObject, out var componentNames, out var componentIndices)) {
         EditorGUI.EndProperty();
         return;
      }

      if (!DrawComponentUi(targetGameObject, componentNames, componentIndices, out var memberNames, out var memberIndices)) {
         EditorGUI.EndProperty();
         return;
      }

      if (!DrawMemberUi(memberNames, memberIndices)) {
         EditorGUI.EndProperty();
         return;
      }

      EditorGUILayout.PropertyField(_hideControl, new GUIContent("Hide Control"));
      EditorGUILayout.LabelField(" ");

      EditorGUI.EndProperty();
   }

   private bool DrawGameObjectUi(out GameObject targetObject, out string[] componentNames, out int[] componentIndices)
   {
      EditorGUILayout.PropertyField(_targetObject, new GUIContent("Target GameObject"));
      
      if (_targetObject.objectReferenceValue == null) {
         _targetComponentName.stringValue = "";
         _targetMemberName.stringValue = "";
         componentNames = null;
         componentIndices = null;
         targetObject = null;
         EditorGUILayout.HelpBox("Assign a GameObject to configure this control", MessageType.Info);
         return false;
      }

      targetObject = (GameObject) _targetObject.objectReferenceValue;
      if (targetObject == null) {
         _targetComponentName.stringValue = "";
         _targetMemberName.stringValue = "";
         componentNames = null;
         componentIndices = null;
         EditorGUILayout.HelpBox("The provided object is null or invalid", MessageType.Error);
         return false;
      }

      if (QuickUiControl.GetComponentEnum(targetObject, out componentNames, out componentIndices)) return true;
      
      _targetComponentName.stringValue = "";
      _targetMemberName.stringValue = "";
      EditorGUILayout.HelpBox("Failed to get component list for provided Object", MessageType.Error);

      return false;
   }

   private bool DrawComponentUi(GameObject targetGameObject, string[] componentNames, int[] componentIndices, out string[] memberNames, out int[] memberIndices)
   {
      EditorGUI.BeginChangeCheck();
      var newIndex = EditorGUILayout.IntPopup(GetSelectedIndex(_targetComponentName.stringValue, componentNames), componentNames, componentIndices);
      if (EditorGUI.EndChangeCheck()) {
         _targetComponentName.stringValue = componentNames[newIndex];
      }
      if (_targetComponentName.stringValue == "") {
         _targetMemberName.stringValue = "";
         memberNames = null;
         memberIndices = null;
         return false;
      }
      var targetComponent = QuickUiControl.GetTargetComponent(targetGameObject, _targetComponentName.stringValue);
      if (targetComponent == null) {
         _targetMemberName.stringValue = "";
         EditorGUILayout.HelpBox("Failed to find component named " + _targetComponentName.stringValue + " on provided object", MessageType.Error);
         memberNames = null;
         memberIndices = null;
         return false;
      }
      
      EditorGUILayout.PropertyField(_targetMemberType, new GUIContent("Member Type"));

      if(!QuickUiControl.GetMemberNameOptions(targetComponent.GetType(), (QuickUiMemberType)_targetMemberType.enumValueIndex, out memberNames, out memberIndices)) {
         _targetMemberName.stringValue = "";
         EditorGUILayout.HelpBox("Failed to get member name list for provided Object/Component", MessageType.Error);
         return false;
      }

      if (memberNames.Length != 0) return true;
      
      _targetMemberName.stringValue = "";
      EditorGUILayout.HelpBox("No members of type " + ((QuickUiMemberType)_targetMemberType.enumValueIndex) + " found on component " + targetComponent.name, MessageType.Warning);
      return false;
   }

   private bool DrawMemberUi(string[] memberNames, int[] memberIndices)
   {
      var newIndex = EditorGUILayout.IntPopup(GetSelectedIndex(_targetMemberName.stringValue, memberNames), memberNames, memberIndices);
      if (EditorGUI.EndChangeCheck()) {
         _targetMemberName.stringValue = memberNames[newIndex];
      }

      return true;
   }

   private int GetSelectedIndex(string value, IReadOnlyList<string> options)
   {
      if (options == null) return 0;
      
      for (var i = 0; i < options.Count; i++) {
         if (options[i] == value) return i;
      }

      return 0;
   }
}
