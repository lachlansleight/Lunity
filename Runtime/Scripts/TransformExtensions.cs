using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lunity
{
    public static class TransformExtensions
    {
        public static void SetPositionX(this Transform tr, float newX, Space space = Space.World)
        {
            var u = space == Space.World ? tr.position : tr.localPosition;
            u.x = newX;
            if(space == Space.World) tr.position = u;
            else tr.localPosition = u;
        }
        
        public static void IncrementPositionX(this Transform tr, float increment, Space space = Space.World)
        {
            var u = space == Space.World ? tr.position : tr.localPosition;
            u.x += increment;
            if(space == Space.World) tr.position = u;
            else tr.localPosition = u;
        }
        
        public static void SetPositionY(this Transform tr, float newY, Space space = Space.World)
        {
            var u = space == Space.World ? tr.position : tr.localPosition;
            u.y = newY;
            if(space == Space.World) tr.position = u;
            else tr.localPosition = u;
        }
        
        public static void IncrementPositionY(this Transform tr, float increment, Space space = Space.World)
        {
            var u = space == Space.World ? tr.position : tr.localPosition;
            u.y += increment;
            if(space == Space.World) tr.position = u;
            else tr.localPosition = u;
        }
        
        public static void SetPositionZ(this Transform tr, float newZ, Space space = Space.World)
        {
            var u = space == Space.World ? tr.position : tr.localPosition;
            u.z = newZ;
            if(space == Space.World) tr.position = u;
            else tr.localPosition = u;
        }
        
        public static void IncrementPositionZ(this Transform tr, float increment, Space space = Space.World)
        {
            var u = space == Space.World ? tr.position : tr.localPosition;
            u.z += increment;
            if(space == Space.World) tr.position = u;
            else tr.localPosition = u;
        }

        public static void FlatLookAt(this Transform tr, Vector3 position)
        {
            var offset = position - tr.position;
            offset.y = 0f;
            offset.Normalize();
            tr.rotation = Quaternion.LookRotation(offset, Vector3.up);
        }

        public static void FlatLookAtCheap(this Transform tr, Vector3 position)
        {
            var offset = position - tr.position;
            var angle = Mathf.Atan2(offset.z, offset.x);
            tr.eulerAngles = Vector3.up * angle;
        }

        public static void SetParentNeutral(this Transform tr, Transform parent)
        {
            tr.SetParent(parent);
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
        
#if UNITY_EDITOR
        [MenuItem("CONTEXT/Transform/Randomize Y Rotation")]
        private static void RandomizeRotationY(MenuCommand command)
        {
            var t = (Transform) command.context;
            t.localRotation = LRandom.FlatRotation();
        }
#endif
    }
}