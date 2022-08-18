using UnityEngine;

namespace Kits.ClientKit.Handlers.ValueType
{
    public static class Vector3Handler
    {

        public static readonly Vector3 Double = new Vector3(2, 2, 2);

        public static readonly Vector3 OneHalf = new Vector3(1.5f, 1.5f, 1.5f);

        public static readonly Vector3 Half = new Vector3(0.5f, 0.5f, 0.5f);

        public static readonly Vector3 Quarter = new Vector3(0.25f, 0.25f, 0.25f);

        public static Vector3 V3(this float f)
        {
            return new Vector3(f, f, f);
        }

        public static Vector3 X2Zero(this Vector3 v3)
        {
            return new Vector3(0, v3.y, v3.y);
        }

        public static Vector3 Y2Zero(this Vector3 v3)
        {
            return new Vector3(v3.x, 0, v3.y);
        }

        public static Vector3 Z2Zero(this Vector3 v3)
        {
            return new Vector3(v3.x, v3.y, 0);
        }

        public static Vector3 ChangeX(this Vector3 v3, float x)
        {
            return new Vector3(x, v3.y, v3.z);
        }

        public static Vector3 ChangeY(this Vector3 v3, float y)
        {
            return new Vector3(v3.x, y, v3.z);
        }

        public static Vector3 ChangeZ(this Vector3 v3, float z)
        {
            return new Vector3(v3.x, v3.y, z);
        }

        public static Vector3 Divide(this Vector3 v3, Vector3 v3d)
        {
            return new Vector3(v3.x / v3d.x, v3.y / v3d.y, v3.z / v3d.z);
        }

        public static Vector3 Divide(this Vector3 v3, Vector2 v2d)
        {
            return new Vector3(v3.x / v2d.x, v3.y / v2d.y, 0);
        }

        public static Vector3 Multi(this Vector3 v3, Vector3 v3m)
        {
            return new Vector3(v3.x * v3m.x, v3.y * v3m.y, v3.z * v3m.z);
        }

        public static Vector3 Multi(this Vector3 v3, Vector2 v2m)
        {
            return new Vector3(v3.x * v2m.x, v3.y * v2m.y, 0);
        }

        public static Vector2 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }


        public static Vector2 YZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }


        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
    }
}