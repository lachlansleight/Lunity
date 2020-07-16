using Lunity;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxRange))]
public class MinMaxRangePropertyDrawer : PropertyDrawer
{
    const float CELL_HEIGHT = 16;

    Rect position;
    SerializedProperty property;
    GUIContent label;

    // Draw the property inside the given rect
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent lab)
    {
        position = pos;
        property = prop;
        label = lab;

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var minRect = new Rect(pos.x + EditorGUIUtility.labelWidth, pos.y, 80f, pos.height);
        var maxRect = new Rect(pos.x + EditorGUIUtility.labelWidth + 95f, pos.y, 100f, pos.height);

        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

}