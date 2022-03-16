using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    [ExecuteAlways]
    public class SetTransformPartial : MonoBehaviour
    {

        public bool Enabled = false;
        [Range(0f, 1f)] public float Crossfade = 0f;
        
        [Header("A")]
        public Vector3 PositionA;
        public Vector3 RotationA;
        public Vector3 ScaleA;
        
        [Header("B")]
        public Vector3 PositionB;
        public Vector3 RotationB;
        public Vector3 ScaleB;

        public void Update()
        {
            if (!Enabled) return;
            
            transform.localPosition = new Vector3(
                Mathf.Lerp(PositionA.x, PositionB.x, Crossfade),
                Mathf.Lerp(PositionA.y, PositionB.y, Crossfade),
                Mathf.Lerp(PositionA.z, PositionB.z, Crossfade)
            );

            transform.localEulerAngles = new Vector3(
                Mathf.Lerp(RotationA.x, RotationB.x, Crossfade),
                Mathf.Lerp(RotationA.y, RotationB.y, Crossfade),
                Mathf.Lerp(RotationA.z, RotationB.z, Crossfade)
            );

            transform.localScale = new Vector3(
                Mathf.Lerp(ScaleA.x, ScaleB.x, Crossfade),
                Mathf.Lerp(ScaleA.y, ScaleB.y, Crossfade),
                Mathf.Lerp(ScaleA.z, ScaleB.z, Crossfade)
            );
        }
    }
}