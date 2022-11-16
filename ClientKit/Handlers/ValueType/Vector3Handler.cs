using System;
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

    public enum VectorMergeOperator
    {
        XToX,
        XToY,
        XToZ,
        XToW,
        YToX,
        YToY,
        YToZ,
        YToW,
        ZToX,
        ZToY,
        ZToZ,
        ZToW,
        WToX,
        WToY,
        WToZ,
        WToW,
        XYToXY,
        XYToXZ,
        XYToXW,
        XYToYX,
        XYToYZ,
        XYToYW,
        XYToZX,
        XYToZY,
        XYToZW,
        XZToXY,
        XZToXZ,
        XZToXW,
        XWToXY,
        XWToXZ,
        XWToXW,
        YXToXY,
        YXToXZ,
        YXToYX,
        YXToYZ,
        YXToZX,
        YXToZY,
        YXToZW,
        YXToWX,
        YXToWY,
        YXToWZ,
        XYZToXYZ,
        XYZToXYW,
        XYZToXZW,
        XYZToYZW,
        XYWToXYZ,
        XYWToXYW,
        XYWToXZW,
        XYWToYZW,
        XZWToXYZ,
        XZWToXYW,
        XZWToXZW,
        XZWToYZW,
        YZWToXYZ,
        YZWToXYW,
        YZWToXZW,
        YZWToYZW,
        WZYToXYZ,
        WZYToXYW,
        WZYToXZW,
        WZYToYZW,
        WZXToXYZ,
        WZXToXYW,
        WZXToXZW,
        WZXToYZW,
        WYXToXYZ,
        WYXToXYW,
        WYXToXZW,
        WYXToYZW,
        ZYXToXYZ,
        ZYXToXYW,
        ZYXToXZW,
        ZYXToYZW,
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

        public static Vector3 XToV3(this float f, float defaultValue = 0)
        {
            return new Vector3(f, defaultValue, defaultValue);
        }

        public static Vector3 YToV3(this float f, float defaultValue = 0)
        {
            return new Vector3(defaultValue, f, defaultValue);
        }

        public static Vector3 ZToV3(this float f, float defaultValue = 0)
        {
            return new Vector3(defaultValue, defaultValue, f);
        }

        public static Vector3 XYToV3(this float f, float defaultValue = 0)
        {
            return new Vector3(f, f, defaultValue);
        }

        public static Vector3 YZToV3(this float f, float defaultValue = 0)
        {
            return new Vector3(defaultValue, f, f);
        }

        public static Vector3 XZToV3(this float f, float defaultValue = 0)
        {
            return new Vector3(f, defaultValue, f);
        }

        public static Vector3 HoldX(this Vector3 v3, float defaultValue = 0)
        {
            v3.y = defaultValue;
            v3.z = defaultValue;
            return v3;
        }

        public static Vector3 HoldY(this Vector3 v3, float defaultValue = 0)
        {
            v3.x = defaultValue;
            v3.z = defaultValue;
            return v3;
        }

        public static Vector3 HoldZ(this Vector3 v3, float defaultValue = 0)
        {
            v3.y = defaultValue;
            v3.z = defaultValue;
            return v3;
        }

        public static Vector3 MergeV2(this Vector3 v3, Vector2 v2T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v3.x = v2T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v3.y = v2T.x;
                    break;
                case VectorMergeOperator.XToZ:
                    v3.z = v2T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v3.x = v2T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v3.y = v2T.y;
                    break;
                case VectorMergeOperator.YToZ:
                    v3.z = v2T.y;
                    break;
                case VectorMergeOperator.XYToXY:
                    v3.x = v2T.x;
                    v3.y = v2T.y;
                    break;
                case VectorMergeOperator.XYToXZ:
                    v3.x = v2T.x;
                    v3.z = v2T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v3.y = v2T.x;
                    v3.x = v2T.y;
                    break;
                case VectorMergeOperator.XYToYZ:
                    v3.y = v2T.x;
                    v3.z = v2T.y;
                    break;
                case VectorMergeOperator.XYToZX:
                    v3.z = v2T.x;
                    v3.x = v2T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v3.x = v2T.x;
                    v3.y = v2T.y;
                    break;
                case VectorMergeOperator.YXToXY:
                    v3.x = v2T.y;
                    v3.y = v2T.x;
                    break;
                case VectorMergeOperator.YXToXZ:
                    v3.x = v2T.y;
                    v3.z = v2T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v3.y = v2T.y;
                    v3.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v3.y = v2T.y;
                    v3.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToZX:
                    v3.z = v2T.y;
                    v3.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToZY:
                    v3.z = v2T.y;
                    v3.y = v2T.x;
                    break;
            }

            return v3;
        }

        public static Vector3 MergeV3(this Vector3 v3, Vector3 v3T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v3.x = v3T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v3.y = v3T.x;
                    break;
                case VectorMergeOperator.XToZ:
                    v3.z = v3T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v3.x = v3T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v3.y = v3T.y;
                    break;
                case VectorMergeOperator.YToZ:
                    v3.z = v3T.y;
                    break;
                case VectorMergeOperator.ZToX:
                    v3.x = v3T.z;
                    break;
                case VectorMergeOperator.ZToY:
                    v3.y = v3T.z;
                    break;
                case VectorMergeOperator.ZToZ:
                    v3.z = v3T.z;
                    break;
                case VectorMergeOperator.XYToXY:
                    v3.x = v3T.x;
                    v3.y = v3T.y;
                    break;
                case VectorMergeOperator.XYToXZ:
                    v3.x = v3T.x;
                    v3.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v3.y = v3T.x;
                    v3.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToYZ:
                    v3.y = v3T.x;
                    v3.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToZX:
                    v3.x = v3T.x;
                    v3.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v3.z = v3T.x;
                    v3.y = v3T.y;
                    break;
                case VectorMergeOperator.XZToXY:
                    v3.x = v3T.x;
                    v3.y = v3T.z;
                    break;
                case VectorMergeOperator.XZToXZ:
                    v3.x = v3T.x;
                    v3.z = v3T.z;
                    break;
                case VectorMergeOperator.YXToXY:
                    v3.x = v3T.y;
                    v3.y = v3T.x;
                    break;
                case VectorMergeOperator.YXToXZ:
                    v3.x = v3T.y;
                    v3.z = v3T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v3.y = v3T.y;
                    v3.x = v3T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v3.y = v3T.y;
                    v3.z = v3T.x;
                    break;
                case VectorMergeOperator.YXToZX:
                    v3.z = v3T.y;
                    v3.x = v3T.x;
                    break;
                case VectorMergeOperator.YXToZY:
                    v3.z = v3T.y;
                    v3.y = v3T.x;
                    break;
                case VectorMergeOperator.ZYXToXYZ:
                    v3.x = v3T.z;
                    v3.y = v3T.y;
                    v3.z = v3T.x;
                    break;
            }

            return v3;
        }

        public static Vector3 MergeV4(this Vector3 v3, Vector4 v4T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v3.x = v4T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v3.y = v4T.x;
                    break;
                case VectorMergeOperator.XToZ:
                    v3.z = v4T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v3.x = v4T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v3.y = v4T.y;
                    break;
                case VectorMergeOperator.YToZ:
                    v3.z = v4T.y;
                    break;
                case VectorMergeOperator.ZToX:
                    v3.x = v4T.z;
                    break;
                case VectorMergeOperator.ZToY:
                    v3.y = v4T.z;
                    break;
                case VectorMergeOperator.ZToZ:
                    v3.z = v4T.z;
                    break;
                case VectorMergeOperator.XYToXY:
                    v3.x = v4T.x;
                    v3.y = v4T.y;
                    break;
                case VectorMergeOperator.XYToXZ:
                    v3.x = v4T.x;
                    v3.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v3.y = v4T.x;
                    v3.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToYZ:
                    v3.y = v4T.x;
                    v3.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToZX:
                    v3.x = v4T.x;
                    v3.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v3.z = v4T.x;
                    v3.y = v4T.y;
                    break;
                case VectorMergeOperator.XZToXY:
                    v3.x = v4T.x;
                    v3.y = v4T.z;
                    break;
                case VectorMergeOperator.XZToXZ:
                    v3.x = v4T.x;
                    v3.z = v4T.z;
                    break;
                case VectorMergeOperator.YXToXY:
                    v3.x = v4T.y;
                    v3.y = v4T.x;
                    break;
                case VectorMergeOperator.YXToXZ:
                    v3.x = v4T.y;
                    v3.z = v4T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v3.y = v4T.y;
                    v3.x = v4T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v3.y = v4T.y;
                    v3.z = v4T.x;
                    break;
                case VectorMergeOperator.YXToZX:
                    v3.z = v4T.y;
                    v3.x = v4T.x;
                    break;
                case VectorMergeOperator.YXToZY:
                    v3.z = v4T.y;
                    v3.y = v4T.x;
                    break;
                case VectorMergeOperator.ZYXToXYZ:
                    v3.x = v4T.z;
                    v3.y = v4T.y;
                    v3.z = v4T.x;
                    break;
                case VectorMergeOperator.WToX:
                    v3.x = v4T.w;
                    break;
                case VectorMergeOperator.WToY:
                    v3.y = v4T.w;
                    break;
                case VectorMergeOperator.WToZ:
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.XWToXY:
                    v3.x = v4T.x;
                    v3.y = v4T.w;
                    break;
                case VectorMergeOperator.XWToXZ:
                    v3.x = v4T.x;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.XYZToXYZ:
                    v3.x = v4T.x;
                    v3.y = v4T.y;
                    v3.z = v4T.z;
                    break;
                case VectorMergeOperator.XYZToXYW:
                    v3.x = v4T.x;
                    v3.y = v4T.y;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.XYZToXZW:
                    v3.x = v4T.x;
                    v3.y = v4T.z;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.XYZToYZW:
                    v3.x = v4T.y;
                    v3.y = v4T.z;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.XYWToXYZ:
                    v3.x = v4T.x;
                    v3.y = v4T.y;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.XZWToXYZ:
                    v3.x = v4T.x;
                    v3.y = v4T.z;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.YZWToXYZ:
                    v3.x = v4T.y;
                    v3.y = v4T.z;
                    v3.z = v4T.w;
                    break;
                case VectorMergeOperator.WZYToXYZ:
                    v3.x = v4T.w;
                    v3.y = v4T.z;
                    v3.z = v4T.y;
                    break;
                case VectorMergeOperator.WZXToXYZ:
                    v3.x = v4T.w;
                    v3.y = v4T.z;
                    v3.z = v4T.x;
                    break;
                case VectorMergeOperator.WYXToXYZ:
                    v3.x = v4T.w;
                    v3.y = v4T.y;
                    v3.z = v4T.x;
                    break;
            }

            return v3;
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
                    v3.x = x;
                    return v3;
                case VectorOperator.Add:
                    v3.x += x;
                    return v3;
                case VectorOperator.Sub:
                    v3.x -= x;
                    return v3;
                case VectorOperator.Multi:
                    v3.x *= x;
                    return v3;
                case VectorOperator.Divide:
                    v3.x /= x;
                    return v3;
                case VectorOperator.Mud:
                    v3.x %= x;
                    return v3;
                default:
                    v3.x = x;
                    return v3;
            }
        }

        public static Vector3 ChangeY(this Vector3 v3, float y, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    v3.y = y;
                    return v3;
                case VectorOperator.Add:
                    v3.y += y;
                    return v3;
                case VectorOperator.Sub:
                    v3.y -= y;
                    return v3;
                case VectorOperator.Multi:
                    v3.y *= y;
                    return v3;
                case VectorOperator.Divide:
                    v3.y /= y;
                    return v3;
                case VectorOperator.Mud:
                    v3.y %= y;
                    return v3;
                default:
                    v3.y = y;
                    return v3;
            }
        }

        public static Vector3 ChangeZ(this Vector3 v3, float z, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    v3.z = z;
                    return v3;
                case VectorOperator.Add:
                    v3.z += z;
                    return v3;
                case VectorOperator.Sub:
                    v3.z -= z;
                    return v3;
                case VectorOperator.Multi:
                    v3.z *= z;
                    return v3;
                case VectorOperator.Divide:
                    v3.z /= z;
                    return v3;
                case VectorOperator.Mud:
                    v3.z %= z;
                    return v3;
                default:
                    v3.z = z;
                    return v3;
            }
        }


        public static Vector3 Divide(this Vector3 v3, Vector3 v3d)
        {
            v3.x /= v3d.x;
            v3.y /= v3d.y;
            v3.z /= v3d.z;
            return v3;
        }

        public static Vector3 Divide(this Vector3 v3, Vector2 v2d)
        {
            v3.x /= v2d.x;
            v3.y /= v2d.y;
            v3.z = 0;
            return v3;
        }

        public static Vector3 Multi(this Vector3 v3, Vector3 v3M)
        {
            v3.x *= v3M.x;
            v3.y *= v3M.y;
            v3.z *= v3M.z;
            return v3;
        }

        public static Vector3 Multi(this Vector3 v3, Vector2 v2M)
        {
            v3.x *= v2M.x;
            v3.y *= v2M.y;
            v3.z = 0;
            return v3;
        }

        public static Vector2 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector2 YX(this Vector3 v)
        {
            return new Vector2(v.y, v.x);
        }

        public static Vector2 YZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }

        public static Vector2 ZX(this Vector3 v)
        {
            return new Vector2(v.z, v.x);
        }

        public static Vector2 ZY(this Vector3 v)
        {
            return new Vector2(v.z, v.y);
        }

        public static Vector4 V3ToV4SetX(Vector3 v, float defaultValue = 0)
        {
            return new Vector4(defaultValue, v.x, v.y, v.z);
        }

        public static Vector3 V3ToV4SetY(Vector3 v, float defaultValue = 0)
        {
            return new Vector4(v.x, defaultValue, v.y, v.z);
        }

        public static Vector4 V3ToV4SetZ(Vector3 v, float defaultValue = 0)
        {
            return new Vector4(v.x, v.y, defaultValue, v.z);
        }

        public static Vector4 V3ToV4SetW(Vector3 v, float defaultValue = 0)
        {
            return new Vector4(v.x, v.y, v.z, defaultValue);
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