using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]
public class AutoTransformEditTime : MonoBehaviour
{
    public Vector3 PositionOffsetPerSecond;
    public Vector3 RotationOffsetPerSecond;
    public Vector3 ScaleOffsetPerSecond;

    public Space TransformSpace = Space.Self;

    public void Update()
    {
        transform.Translate(PositionOffsetPerSecond * Time.deltaTime, TransformSpace);
        transform.Rotate(RotationOffsetPerSecond * Time.deltaTime, TransformSpace);
        transform.localScale += ScaleOffsetPerSecond * Time.deltaTime;
    }
}
