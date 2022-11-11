using Lunity;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CenterExtentInt))]
public class CenterExtentIntPropertyDrawer : PropertyDrawer
{
    private const float SubLabelSpacing = 4;
    private const float BottomSpacing = 2;

    Rect position;
    SerializedProperty property;
    GUIContent label;

    // Draw the property inside the given rect
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent lab)
    {
        pos.height -= BottomSpacing;
        
        label = lab;
        label = EditorGUI.BeginProperty(pos, label, prop);
        
        var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
        var labels      = new[] {new GUIContent("center"), new GUIContent("extent")};
        var properties  = new[] {prop.FindPropertyRelative("center"), prop.FindPropertyRelative("extent")};
        DrawMultiplePropertyFields(contentRect, labels, properties);
 
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) + BottomSpacing;
    }
    
    //Thanks, https://forum.unity.com/threads/making-a-proper-drawer-similar-to-vector3-how.385532/
    private static void DrawMultiplePropertyFields(Rect pos, GUIContent[] subLabels, SerializedProperty[] props) {
        // backup gui settings
        var indent     = EditorGUI.indentLevel;
        var labelWidth = EditorGUIUtility.labelWidth;
     
        // draw properties
        var propsCount = props.Length;
        var width      = (pos.width - (propsCount - 1) * SubLabelSpacing) / propsCount;
        var contentPos = new Rect(pos.x, pos.y, width, pos.height);
        EditorGUI.indentLevel = 0;
        for (var i = 0; i < propsCount; i++) {
            EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x;
            EditorGUI.PropertyField(contentPos, props[i], subLabels[i]);
            contentPos.x += width + 4;
        }
 
        // restore gui settings
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.indentLevel       = indent;
    }

}