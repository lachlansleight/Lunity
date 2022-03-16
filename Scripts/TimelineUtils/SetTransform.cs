using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    [ExecuteAlways]
    public class SetTransform : MonoBehaviour
    {

        public bool Active;
        public Space TransformSpace;

        [Space(10)]
        public Vector3 Position;

        public bool PositionX;
        public bool PositionY;
        public bool PositionZ;

        [Space(10)]
        public Vector3 Rotation;

        public bool RotationX;
        public bool RotationY;
        public bool RotationZ;

        [Space(10)]
        public Vector3 Scale;

        public bool ScaleX;
        public bool ScaleY;
        public bool ScaleZ;

        public void Update()
        {
            if (Active) {
                if (TransformSpace == Space.Self) {
                    transform.localPosition = new Vector3(
                        PositionX ? Position.x : transform.localPosition.x,
                        PositionY ? Position.y : transform.localPosition.y,
                        PositionZ ? Position.z : transform.localPosition.z
                    );
                } else {
                    transform.position = new Vector3(
                        PositionX ? Position.x : transform.position.x,
                        PositionY ? Position.y : transform.position.y,
                        PositionZ ? Position.z : transform.position.z
                    );
                }
            } else Position = TransformSpace == Space.Self ? transform.localPosition : transform.position;

            if (Active) {
                if (TransformSpace == Space.Self) {
                    transform.localEulerAngles = new Vector3(
                        RotationX ? Rotation.x : transform.localEulerAngles.x,
                        RotationY ? Rotation.y : transform.localEulerAngles.y,
                        RotationZ ? Rotation.z : transform.localEulerAngles.z
                    );
                } else {
                    transform.eulerAngles = new Vector3(
                        RotationX ? Rotation.x : transform.eulerAngles.x,
                        RotationY ? Rotation.y : transform.eulerAngles.y,
                        RotationZ ? Rotation.z : transform.eulerAngles.z
                    );
                }
            } else Rotation = TransformSpace == Space.Self ? transform.localEulerAngles : transform.eulerAngles;

            if (Active) {
                transform.localScale = new Vector3(
                    ScaleX ? Scale.x : transform.localScale.x,
                    ScaleY ? Scale.y : transform.localScale.y,
                    ScaleZ ? Scale.z : transform.localScale.z
                );
            } else Scale = transform.localScale;
        }
    }
}