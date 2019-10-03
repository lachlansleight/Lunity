using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShaderProperty))]
public class ShaderPropertyDrawer : PropertyDrawer
{
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return -2f;
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        GUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Type"), new GUIContent("Type"));
        var index = property.FindPropertyRelative("Type").enumValueIndex;
        if (index == 5) {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("KernelIndex"), new GUIContent("KernelIndex"));
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Name"), new GUIContent("Name"));
        switch (index) {
            case 0:
                EditorGUILayout.PropertyField(property.FindPropertyRelative("FloatValue"), new GUIContent("Value"));
                break;
            case 1:
                EditorGUILayout.PropertyField(property.FindPropertyRelative("IntValue"), new GUIContent("Value"));
                break;
            case 2:
                EditorGUILayout.PropertyField(property.FindPropertyRelative("VectorValue"), new GUIContent("Value"));
                break;
            case 3:
                EditorGUILayout.PropertyField(property.FindPropertyRelative("BoolValue"), new GUIContent("Value"));
                break;
            case 4:
                EditorGUILayout.PropertyField(property.FindPropertyRelative("MatrixValue"), new GUIContent("Value"));
                break;
            case 5:
                EditorGUILayout.PropertyField(property.FindPropertyRelative("TextureValue"), new GUIContent("Value"));
                break;
        }
        
        EditorGUILayout.EndVertical();
        
        EditorGUI.EndProperty();
    }
}
