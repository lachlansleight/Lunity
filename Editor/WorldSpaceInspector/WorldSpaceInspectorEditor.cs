using UnityEditor;
using UnityEngine;

namespace Lunity
{
    [CustomEditor(typeof(WorldSpaceInspector))]
    public class WorldSpaceInspectorEditor : Editor
    {

        private SerializedProperty _targetObject;
        private SerializedProperty _targetComponentIndex;

        public void OnEnable()
        {
            _targetObject = serializedObject.FindProperty("TargetObject");
            _targetComponentIndex = serializedObject.FindProperty("TargetComponentIndex");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var myTarget = (WorldSpaceInspector) target;

            EditorGUILayout.PropertyField(_targetObject, new GUIContent("Target GameObject"));
            if (_targetObject.objectReferenceValue == null) {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            
            if (myTarget.TargetObject == null) {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            if(!myTarget.GetComponentEnum(out var names, out var indices)) {
                serializedObject.ApplyModifiedProperties();
                return;
            }

            EditorGUILayout.IntPopup(_targetComponentIndex, names, indices, new GUIContent("Target Component"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}