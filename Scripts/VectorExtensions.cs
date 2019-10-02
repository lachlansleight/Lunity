using UnityEngine;

namespace Lunity
{
    public static class VectorExtensions
    {
        public static void SetX(this Vector2 v, float newX)
        {
            var u = v;
            u.x = newX;
            v = u;
        }
        
        public static void IncrementX(this Vector2 v, float increment)
        {
            var u = v;
            u.x += increment;
            v = u;
        }
        
        public static void SetY(this Vector2 v, float newY)
        {
            var u = v;
            u.y = newY;
            v = u;
        }
        
        public static void IncrementY(this Vector2 v, float increment)
        {
            var u = v;
            u.y += increment;
            v = u;
        }
        
        public static void SetX(this Vector3 v, float newX)
        {
            var u = v;
            u.x = newX;
            v = u;
        }
        
        public static void IncrementX(this Vector3 v, float increment)
        {
            var u = v;
            u.x += increment;
            v = u;
        }
        
        public static void SetY(this Vector3 v, float newY)
        {
            var u = v;
            u.y = newY;
            v = u;
        }
        
        public static void IncrementY(this Vector3 v, float increment)
        {
            var u = v;
            u.y += increment;
            v = u;
        }
        
        public static void SetZ(this Vector3 v, float newZ)
        {
            var u = v;
            u.z = newZ;
            v = u;
        }
        
        public static void IncrementZ(this Vector3 v, float increment)
        {
            var u = v;
            u.z += increment;
            v = u;
        }
        
        public static void SetX(this Vector4 v, float newX)
        {
            var u = v;
            u.x = newX;
            v = u;
        }
        
        public static void IncrementX(this Vector4 v, float increment)
        {
            var u = v;
            u.x += increment;
            v = u;
        }
        
        public static void SetY(this Vector4 v, float newY)
        {
            var u = v;
            u.y = newY;
            v = u;
        }
        
        public static void IncrementY(this Vector4 v, float increment)
        {
            var u = v;
            u.y += increment;
            v = u;
        }
        
        public static void SetZ(this Vector4 v, float newZ)
        {
            var u = v;
            u.z = newZ;
            v = u;
        }
        
        public static void IncrementZ(this Vector4 v, float increment)
        {
            var u = v;
            u.z += increment;
            v = u;
        }
        
        public static void SetW(this Vector4 v, float newW)
        {
            var u = v;
            u.w = newW;
            v = u;
        }
        
        public static void IncrementW(this Vector4 v, float increment)
        {
            var u = v;
            u.w += increment;
            v = u;
        }
    }
}