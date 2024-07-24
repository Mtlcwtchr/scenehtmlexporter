using UnityEngine;

namespace Extensions
{
    public static class SerializationUtils
    {
        public static string ToStr(this Vector3 vector)
        {
            return $"{vector.x}, {vector.y}, {vector.z}";
        }
        
        public static string ToStr(this Quaternion quaternion)
        {
            return $"{quaternion.x}, {quaternion.y}, {quaternion.z}, {quaternion.w}";
        }
    }
}