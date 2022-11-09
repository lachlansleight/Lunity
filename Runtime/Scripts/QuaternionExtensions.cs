using UnityEngine;

namespace Lunity
{
    public static class QuaternionExtensions
    {
        public static Quaternion GetFlattened(this Quaternion q)
        {
            var fwd = q * Vector3.forward;
            fwd.y = 0f;
            return Quaternion.LookRotation(fwd, Vector3.up);
        }
        
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