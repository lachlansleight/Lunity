using UnityEngine;
using UnityEditor;

//https://gist.github.com/elaberge/36e43c1f459ee36cde64dc35bf54c312
[CustomPropertyDrawer(typeof(Matrix4x4))]
public class MatrixPropertyDrawer : PropertyDrawer
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

        Matrix4x4 matrix = Matrix4x4.identity;

        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                DrawCell(c, r);
                matrix[r, c] = property.FindPropertyRelative("e" + r + c).floatValue;
            }
        }

        pos.y += 5 * CELL_HEIGHT;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    void DrawCell(int column, int row)
    {
        Vector2 cellPos = position.position;
        cellPos.x += position.width * column / 4;
        cellPos.y += CELL_HEIGHT * row;
        EditorGUI.PropertyField(
            new Rect(cellPos, new Vector2(position.width / 4, CELL_HEIGHT)),
            property.FindPropertyRelative("e" + row + column),
            GUIContent.none
        );
    }

    static Vector4 QuaternionToVector4(Quaternion rot)
    {
        return new Vector4(rot.x, rot.y, rot.z, rot.w);
    }

    static Quaternion Vector4ToQuaternion(Vector4 v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }

}