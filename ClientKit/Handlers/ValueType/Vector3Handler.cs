using UnityEngine;

namespace Kits.ClientKit.Handlers.ValueType
{
    public enum VectorOperator
    {
        Equal,
        Add,
        Sub,
        Multi,
        Divide,
        Mud,
    }

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

        public static Vector3 XToV3(this float f)
        {
            return new Vector3(f, 0, 0);
        }
        
        public static Vector3 YToV3(this float f)
        {
            return new Vector3(0, f, 0);
        }
        
        public static Vector3 ZToV3(this float f)
        {
            return new Vector3(0, 0, f);
        }
        
        public static Vector3 HoldX(this Vector3 v3)
        {
            return new Vector3(v3.x, 0, 0);
        }

        public static Vector3 HoldY(this Vector3 v3)
        {
            return new Vector3(0, v3.y, 0);
        }
        
        public static Vector3 HoldZ(this Vector3 v3)
        {
            return new Vector3(0, 0,v3.z);
        }
        
        public static Vector3 MergeX(this Vector3 v3,Vector2 v2T)
        {
            return new Vector3(v2T.x, v3.y, v3.z);
        }

        public static Vector3 MergeX(this Vector3 v3,Vector3 v3T)
        {
            return new Vector3(v3T.x, v3.y, v3.z);
        }
        
        public static Vector3 MergeX(this Vector3 v3,Vector4 v4T)
        {
            return new Vector3(v4T.x, v3.y, v3.z);
        }
        
        public static Vector3 MergeY(this Vector3 v3,Vector2 v2T)
        {
            return new Vector3(v3.x, v2T.y, v3.z);
        }
        
        public static Vector3 MergeY(this Vector3 v3,Vector3 v3T)
        {
            return new Vector3(v3.x, v3T.y, v3.z);
        }
        
        public static Vector3 MergeY(this Vector3 v3,Vector4 v4T)
        {
            return new Vector3(v3.x, v4T.y, v3.z);
        }
        
        public static Vector3 MergeZ(this Vector3 v3,Vector3 v3T)
        {
            return new Vector3(v3.x, v3.y, v3T.z);
        }
        
        public static Vector3 MergeZ(this Vector3 v3,Vector4 v4T)
        {
            return new Vector3(v3.x, v3.y, v4T.z);
        }
        
        public static Vector3 MergeXY(this Vector3 v3,Vector2 v2T)
        {
            return new Vector3(v2T.x, v2T.y, v3.z);
        }
        
        public static Vector3 MergeXY(this Vector3 v3,Vector3 v3T)
        {
            return new Vector3(v3T.x, v3T.y, v3.z);
        }
        
        public static Vector3 MergeXY(this Vector3 v3,Vector4 v4T)
        {
            return new Vector3(v4T.x, v4T.y, v3.z);
        }
        
        public static Vector3 MergeXZ(this Vector3 v3,Vector3 v3T)
        {
            return new Vector3(v3T.x, v3.y, v3T.z);
        }
        
        public static Vector3 MergeXZ(this Vector3 v3,Vector4 v4T)
        {
            return new Vector3(v4T.x, v3.y, v4T.z);
        }
        
        public static Vector3 MergeYZ(this Vector3 v3,Vector4 v4T)
        {
            return new Vector3(v3.x, v4T.y, v4T.z);
        }
        
        public static Vector3 X2Zero(this Vector3 v3)
        {
            return v3.ChangeX(0);
        }

        public static Vector3 Y2Zero(this Vector3 v3)
        {
            return v3.ChangeY(0);
        }

        public static Vector3 Z2Zero(this Vector3 v3)
        {
            return v3.ChangeZ(0);
        }

        public static Vector3 ChangeX(this Vector3 v3, float x, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector3(x, v3.y, v3.z);
                case VectorOperator.Add:
                    return new Vector3(v3.x + x, v3.y, v3.z);
                case VectorOperator.Sub:
                    return new Vector3(v3.x - x, v3.y, v3.z);
                case VectorOperator.Multi:
                    return new Vector3(v3.x * x, v3.y, v3.z);
                case VectorOperator.Divide:
                    return new Vector3(v3.x / x, v3.y, v3.z);
                case VectorOperator.Mud:
                    return new Vector3(v3.x % x, v3.y, v3.z);
                default:
                    return new Vector3(x, v3.y, v3.z);
            }
        }

        public static Vector3 ChangeY(this Vector3 v3, float y, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector3(v3.x, y, v3.z);
                case VectorOperator.Add:
                    return new Vector3(v3.x, v3.y + y, v3.z);
                case VectorOperator.Sub:
                    return new Vector3(v3.x, v3.y - y, v3.z);
                case VectorOperator.Multi:
                    return new Vector3(v3.x, v3.y * y, v3.z);
                case VectorOperator.Divide:
                    return new Vector3(v3.x, v3.y / y, v3.z);
                case VectorOperator.Mud:
                    return new Vector3(v3.x, v3.y % y, v3.z);
                default:
                    return new Vector3(v3.x, y, v3.z);
            }
        }

        public static Vector3 ChangeZ(this Vector3 v3, float z, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector3(v3.x, v3.y, z);
                case VectorOperator.Add:
                    return new Vector3(v3.x, v3.y, v3.z + z);
                case VectorOperator.Sub:
                    return new Vector3(v3.x, v3.y, v3.z - z);
                case VectorOperator.Multi:
                    return new Vector3(v3.x, v3.y, v3.z * z);
                case VectorOperator.Divide:
                    return new Vector3(v3.x, v3.y, v3.z / z);
                case VectorOperator.Mud:
                    return new Vector3(v3.x, v3.y, v3.z % z);
                default:
                    return new Vector3(v3.x, v3.y, z);
            }
        }


        public static Vector3 Divide(this Vector3 v3, Vector3 v3d)
        {
            return new Vector3(v3.x / v3d.x, v3.y / v3d.y, v3.z / v3d.z);
        }

        public static Vector3 Divide(this Vector3 v3, Vector2 v2d)
        {
            return new Vector3(v3.x / v2d.x, v3.y / v2d.y, 0);
        }

        public static Vector3 Multi(this Vector3 v3, Vector3 v3M)
        {
            return new Vector3(v3.x * v3M.x, v3.y * v3M.y, v3.z * v3M.z);
        }

        public static Vector3 Multi(this Vector3 v3, Vector2 v2M)
        {
            return new Vector3(v3.x * v2M.x, v3.y * v2M.y, 0);
        }

        public static Vector2 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector2 Yx(this Vector3 v)
        {
            return new Vector2(v.y, v.x);
        }

        public static Vector2 YZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }

        public static Vector2 Zx(this Vector3 v)
        {
            return new Vector2(v.z, v.x);
        }

        public static Vector2 Zy(this Vector3 v)
        {
            return new Vector2(v.z, v.y);
        }

        public static Vector4 V3ToV4EmptyX(Vector3 v)
        {
            return new Vector4(0.0f, v.x, v.y, v.z);
        }

        public static Vector3 V3ToV4EmptyY(Vector3 v)
        {
            return new Vector4(v.x, 0.0f, v.y, v.z);
        }

        public static Vector4 V3ToV4EmptyZ(Vector3 v)
        {
            return new Vector4(v.x, v.y, 0.0f, v.z);
        }

        public static Vector4 V3ToV4EmptyW(Vector3 v)
        {
            return new Vector4(v.x, v.y, v.z, 0.0f);
        }

        public static bool Approximately(this Vector3 v3, Vector3 v3A, float delta = 0)
        {
            if (delta == 0)
            {
                return Mathf.Approximately(v3.x, v3A.x) && Mathf.Approximately(v3.y, v3A.y) && Mathf.Approximately(v3.z, v3A.z);
            }
            else
            {
                float x = Mathf.Abs(v3.x - v3A.x);
                float y = Mathf.Abs(v3.y - v3A.y);
                float z = Mathf.Abs(v3.x - v3A.z);
                return x < delta && y < delta && z < delta;
            }
        }
    }
}