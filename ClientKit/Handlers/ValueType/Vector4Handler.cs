using UnityEngine;

namespace Kits.ClientKit.Handlers.ValueType
{
    public static class Vector4Handler
    {
        public static readonly Vector4 Double = new Vector4(2, 2, 2,2);

        public static readonly Vector4 OneHalf = new Vector4(1.5f, 1.5f, 1.5f,1.5f);

        public static readonly Vector4 Half = new Vector4(0.5f, 0.5f, 0.5f,0.5f);

        public static readonly Vector4 Quarter = new Vector4(0.25f, 0.25f, 0.25f,0.25f);

        public static Vector4 V4(this float f)
        {
            return new Vector4(f, f, f,f);
        }

        public static Vector4 X2Zero(this Vector4 v4)
        {
            return new Vector4(0, v4.y, v4.y, v4.w);
        }

        public static Vector4 Y2Zero(this Vector4 v4)
        {
            return new Vector4(v4.x, 0, v4.y, v4.w);
        }

        public static Vector4 Z2Zero(this Vector4 v4)
        {
            return new Vector4(v4.x, v4.y, 0, v4.w);
        }

        public static Vector4 W2Zero(this Vector4 v4)
        {
            return new Vector4(v4.x, v4.y, v4.z, 0);
        }
        
        public static Vector4 ChangeX(this Vector4 v4, float x)
        {
            return new Vector4(x, v4.y, v4.z, v4.w);
        }

        public static Vector4 ChangeY(this Vector4 v4, float y)
        {
            return new Vector4(v4.x, y, v4.z, v4.w);
        }

        public static Vector4 ChangeZ(this Vector4 v4, float z)
        {
            return new Vector4(v4.x, v4.y, z, v4.w);
        }

        public static Vector4 ChangeW(this Vector4 v4, float w)
        {
            return new Vector4(v4.x, v4.y, v4.z, w);
        }
        
        public static Vector4 Divide(this Vector4 v4, Vector4 v4d)
        {
            return new Vector4(v4.x / v4d.x, v4.y / v4d.y, v4.z / v4d.z, v4.w / v4d.w);
        }

        public static Vector4 Divide(this Vector4 v4, Vector3 v3d)
        {
            return new Vector4(v4.x / v3d.x, v4.y / v3d.y, v4.z / v3d.z,0);
        }
        
        public static Vector4 Divide(this Vector4 v4, Vector2 v2d)
        {
            return new Vector4(v4.x / v2d.x, v4.y / v2d.y, 0, 0);
        }

        public static Vector4 Multi(this Vector4 v4, Vector4 v4m)
        {
            return new Vector4(v4.x * v4m.x, v4.y * v4m.y, v4.z * v4m.z);
        }
        
        public static Vector4 Multi(this Vector4 v4, Vector3 v3m)
        {
            return new Vector4(v4.x * v3m.x, v4.y * v3m.y, v4.z / v3m.z, 0);
        }
        
        public static Vector4 Multi(this Vector4 v4, Vector2 v2m)
        {
            return new Vector4(v4.x * v2m.x, v4.y * v2m.y, 0, 0);
        }

        public static Vector2 XY(this Vector4 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 XZ(this Vector4 v)
        {
            return new Vector2(v.x, v.z);
        }
        
        public static Vector2 XW(this Vector4 v)
        {
            return new Vector2(v.x, v.w);
        }
        
        public static Vector2 YX(this Vector4 v)
        {
            return new Vector2(v.y, v.w);
        }
        
        public static Vector2 YZ(this Vector4 v)
        {
            return new Vector2(v.y, v.z);
        }
        
        public static Vector2 YW(this Vector4 v)
        {
            return new Vector2(v.y, v.w);
        }
        
        public static Vector2 ZX(this Vector4 v)
        {
            return new Vector2(v.z, v.x);
        }
        
        public static Vector2 ZY(this Vector4 v)
        {
            return new Vector2(v.z, v.y);
        }
        
        public static Vector2 ZW(this Vector4 v)
        {
            return new Vector2(v.z, v.w);
        }
        
        public static bool Approximately(this Vector4 v4,Vector4 v4a,float delta = 0)
        {
            if (delta == 0)
            {
                return Mathf.Approximately(v4.x, v4a.x) && Mathf.Approximately(v4.y, v4a.y) && Mathf.Approximately(v4.z, v4a.z) && Mathf.Approximately(v4.w, v4a.w);
            }
            else
            {
                float x = Mathf.Abs(v4.x - v4a.x);
                float y = Mathf.Abs(v4.y - v4a.y);
                float z = Mathf.Abs(v4.z - v4a.z);
                float w = Mathf.Abs(v4.w - v4a.w);
                return x < delta && y < delta && z < delta && w < delta;
            }
        }
    }
}