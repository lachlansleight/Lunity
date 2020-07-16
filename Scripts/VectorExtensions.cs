using UnityEngine;

namespace Lunity
{
    public static class VectorExtensions
    {
        
        //==============================================================================================================
        //==============================================================================================================
        //==                                        VECTOR 2 EXTENSIONS                                               ==
        //==============================================================================================================
        //==============================================================================================================
        
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

        public static float[] ToArray(this Vector2 v)
        {
            return new [] {v.x, v.y};
        }
        
        //==============================================================================================================
        //==============================================================================================================
        //==                                        VECTOR 3 EXTENSIONS                                               ==
        //==============================================================================================================
        //==============================================================================================================
        
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
        
        public static float[] ToArray(this Vector3 v)
        {
            return new [] {v.x, v.y, v.z};
        }
        
        //==============================================================================================================
        //==============================================================================================================
        //==                                        VECTOR 4 EXTENSIONS                                               ==
        //==============================================================================================================
        //==============================================================================================================
        
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
        
        public static float[] ToArray(this Vector4 v)
        {
            return new [] {v.x, v.y, v.z, v.w};
        }
        
        //==============================================================================================================
        //==============================================================================================================
        //==                                       QUATERNION EXTENSIONS                                              ==
        //==============================================================================================================
        //==============================================================================================================

        public static Vector4 ToVector4(this Quaternion q)
        {
            return new Vector4(q.x, q.y, q.z, q.w);
        }

        public static float[] ToArray(this Quaternion q)
        {
            return new[] {q.x, q.y, q.z, q.w};
        }

        public static float GetMagnitude(this Quaternion q)
        {
            return Mathf.Sqrt(q.GetSqrMagnitude());
        }
        
        public static float GetSqrMagnitude(this Quaternion q)
        {
            return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
        }
    }
}