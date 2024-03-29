﻿using UnityEngine;

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
        
        public static Vector2 ApplyScale(this Vector2 v, float x, float y)
        {
            return new Vector2(v.x * x, v.y * y);
        }

        public static float[] ToArray(this Vector2 v)
        {
            return new [] {v.x, v.y};
        }

        public static Vector3 To3DXY(this Vector2 v, float z = 0f)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector3 To3DXZ(this Vector2 v, float y = 0f)
        {
            return new Vector3(v.x, y, v.y);
        }

        public static Vector3 To3DYZ(this Vector2 v, float x = 0f)
        {
            return new Vector3(x, v.y, v.x);
        }

        public static Vector2 Swizzle(this Vector2 v, int x, int y)
        {
            return new Vector2(v[x], v[y]);
        }

        public static Vector2 ScaleInPlace(this Vector2 v, Vector2 scale)
        {
            return new Vector2(v.x * scale.x, v.y * scale.y);
        }

        public static Vector2Int ToInt(this Vector2 v, bool round = false)
        {
            return new Vector2Int(
                round ? Mathf.RoundToInt(v.x) : (int)v.x, 
                round ? Mathf.RoundToInt(v.y) : (int)v.y
            );
        }

        public static Vector2 ToFloat(this Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }
        
        /// Ensures the euler angles are in the range -180 - 180 degrees
        public static Vector2 NormalizeEulers(this Vector2 v)
        {
            while (v.x < -180f) v.x += 360f;
            while (v.x > 180f) v.x -= 360f;
            while (v.y < -180f) v.y += 360f;
            while (v.y > 180f) v.y -= 360f;
            return v;
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

        public static Vector3 ApplyScale(this Vector3 v, float x, float y, float z)
        {
            return new Vector3(v.x * x, v.y * y, v.z * z);
        }

        public static Vector3 Flatten(this Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }
        
        public static float[] ToArray(this Vector3 v)
        {
            return new [] {v.x, v.y, v.z};
        }

        public static Vector2 To2DXY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 To2DXZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector3 To2DYZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }
        
        public static Vector2 Swizzle(this Vector3 v, int x, int y)
        {
            return new Vector2(v[x], v[y]);
        }
        
        public static Vector3 Swizzle(this Vector3 v, int x, int y, int z)
        {
            return new Vector3(v[x], v[y], v[z]);
        }

        public static Vector3 ScaleInPlace(this Vector3 v, Vector3 scale)
        {
            return new Vector3(v.x * scale.x, v.y * scale.y, v.z * scale.z);
        }

        public static Vector3Int ToInt(this Vector3 v, bool round = false)
        {
            return new Vector3Int(
                round ? Mathf.RoundToInt(v.x) : (int)v.x, 
                round ? Mathf.RoundToInt(v.y) : (int)v.y,
                round ? Mathf.RoundToInt(v.z) : (int)v.z
            );
        }

        public static Vector3 ToFloat(this Vector3Int v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static bool IsInsideCollider(this Vector3 v, Collider collider)
        {
            if (!collider) return false;
            var closest = collider.ClosestPoint(v);
            return closest == v;
        }

        /// Ensures the euler angles are in the range -180 - 180 degrees
        public static Vector3 NormalizeEulers(this Vector3 v)
        {
            while (v.x < -180f) v.x += 360f;
            while (v.x > 180f) v.x -= 360f;
            while (v.y < -180f) v.y += 360f;
            while (v.y > 180f) v.y -= 360f;
            while (v.z < -180f) v.z += 360f;
            while (v.z > 180f) v.z -= 360f;
            return v;
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

        public static Vector2 Swizzle(this Vector4 v, int x, int y)
        {
            return new Vector2(v[x], v[y]);
        }
        
        public static Vector3 Swizzle(this Vector4 v, int x, int y, int z)
        {
            return new Vector3(v[x], v[y], v[z]);
        }
        
        public static Vector4 Swizzle(this Vector4 v, int x, int y, int z, int w)
        {
            return new Vector4(v[x], v[y], v[z], v[w]);
        }

        public static Vector2 To2DXY(this Vector4 v) => new Vector2(v.x, v.y);
        public static Vector2 To2DXZ(this Vector4 v) => new Vector2(v.x, v.z);
        public static Vector2 To2DXW(this Vector4 v) => new Vector2(v.x, v.w);
        public static Vector2 To2DYZ(this Vector4 v) => new Vector2(v.y, v.z);
        public static Vector2 To2DYW(this Vector4 v) => new Vector2(v.y, v.w);
        public static Vector2 To2DZW(this Vector4 v) => new Vector2(v.z, v.w);
        public static Vector3 To3DXYZ(this Vector4 v) => new Vector3(v.x, v.y, v.z);
        public static Vector3 To3DXYW(this Vector4 v) => new Vector3(v.x, v.y, v.w);
        public static Vector3 To3DXZW(this Vector4 v) => new Vector3(v.x, v.z, v.w);
        public static Vector3 To3DYZW(this Vector4 v) => new Vector3(v.y, v.z, v.w);

        public static Vector4 ApplyScale(this Vector4 v, float x, float y, float z, float w)
        {
            return new Vector4(v.x * x, v.y * y, v.z * z, v.w * w);
        }
        
        public static float[] ToArray(this Vector4 v)
        {
            return new [] {v.x, v.y, v.z, v.w};
        }

        public static Vector4 ScaleInPlace(this Vector4 v, Vector4 scale)
        {
            return new Vector4(v.x * scale.x, v.y * scale.y, v.z * scale.z, v.w * scale.w);
        }
    }
}