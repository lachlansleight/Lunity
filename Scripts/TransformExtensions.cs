using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}