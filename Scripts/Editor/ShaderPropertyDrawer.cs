using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShaderProperty))]
public class ShaderPropertyDrawer : PropertyDrawer
{

    public int DisplayType;

    private const float Padding = 6f;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var displayType = property.FindPropertyRelative("Type").enumValueIndex;

        EditorGUI.HelpBox(position, "", MessageType.None);

        var innerRect = new Rect(position.x, position.y + Padding, position.width, position.height - (2f * Padding));
        
        var lineOne =   new Rect(innerRect.x,                           innerRect.y,       innerRect.width,         16f);
        var lineOneA =  new Rect(innerRect.x,                           innerRect.y,       innerRect.width * 0.55f, 16f);
        var lineOneB =  new Rect(innerRect.x + innerRect.width * 0.55f, innerRect.y,       innerRect.width * 0.45f, 16f);
        var lineTwo =   new Rect(innerRect.x,                           innerRect.y + 16f, innerRect.width,         16f);
        var lineThree = new Rect(innerRect.x,                           innerRect.y + 32f, innerRect.width,         16f);
        if (displayType == 4) {
            lineThree.height = 17f * 16f;
        }
        
        if (displayType == 5) {
            EditorGUI.PropertyField(lineOneA, property.FindPropertyRelative("Type"), new GUIContent("Type"));
            EditorGUI.PropertyField(lineOneB, property.FindPropertyRelative("KernelIndex"), new GUIContent("KernelIndex"));
        } else {
            EditorGUI.PropertyField(lineOne, property.FindPropertyRelative("Type"), new GUIContent("Type"));
        }

        EditorGUI.PropertyField(lineTwo, property.FindPropertyRelative("Name"), new GUIContent("Property Name"));
        
        switch (displayType) {
            case 0:
                EditorGUI.PropertyField(lineThree, property.FindPropertyRelative("FloatValue"), new GUIContent("Value"));
                break;
            case 1:
                EditorGUI.PropertyField(lineThree, property.FindPropertyRelative("IntValue"), new GUIContent("Value"));
                break;
            case 2:
                EditorGUI.PropertyField(lineThree, property.FindPropertyRelative("VectorValue"), new GUIContent("Value"));
                break;
            case 3:
                EditorGUI.PropertyField(lineThree, property.FindPropertyRelative("BoolValue"), new GUIContent("Value"));
                break;
            case 4:
                EditorGUI.PropertyField(lineThree, property.FindPropertyRelative("MatrixValue"), new GUIContent("Value"));
                
                break;
            case 5:
                EditorGUI.PropertyField(lineThree, property.FindPropertyRelative("TextureValue"), new GUIContent("Value"));
                break;
        }
            
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var displayType = property.FindPropertyRelative("Type").enumValueIndex;
        if (displayType == 4) {
            return (5f * 16f) + (2f * Padding) + base.GetPropertyHeight(property, label);
        } else {
            return (2f * 16f) + (2f * Padding) + base.GetPropertyHeight(property, label);    
        }
    }
//    
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //if (Event.current.type == EventType.Repaint)
//        //    return;
//        
//        //Debug.Log("Event type: " + Event.current.type);
//
//        if (Event.current.type == EventType.Layout) {
//            _displayType = property.FindPropertyRelative("Type").enumValueIndex;
//        }
//        
//        EditorGUI.BeginProperty(position, label, property);
//        
//        try {
//            GUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
//    
//            EditorGUILayout.BeginHorizontal();
//                EditorGUILayout.PropertyField(property.FindPropertyRelative("Type"), new GUIContent("Type"));
//            
//            
//            if (_displayType == 5) {
//                EditorGUILayout.PropertyField(property.FindPropertyRelative("KernelIndex"), new GUIContent("KernelIndex"));
//            }
//            //EditorGUILayout.EndHorizontal();
//            EditorGUILayout.PropertyField(property.FindPropertyRelative("Name"), new GUIContent("Name"));
//            switch (_displayType) {
//                case 0:
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("FloatValue"), new GUIContent("Value"));
//                    break;
//                case 1:
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("IntValue"), new GUIContent("Value"));
//                    break;
//                case 2:
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("VectorValue"), new GUIContent("Value"));
//                    break;
//                case 3:
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("BoolValue"), new GUIContent("Value"));
//                    break;
//                case 4:
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("MatrixValue"), new GUIContent("Value"));
//                    break;
//                case 5:
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("TextureValue"), new GUIContent("Value"));
//                    break;
//            }
//            
//            EditorGUILayout.EndVertical();
//        }
//        catch (Exception e) {
//            Debug.LogError(e.Message);
//        }
//        
//        EditorGUI.EndProperty();
//    }
}
