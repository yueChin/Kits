using System;
using UnityEngine;

namespace Kits.ClientKit.Handlers.ValueType
{
    public static class Vector4Handler
    {
        public static readonly Vector4 Double = new Vector4(2, 2, 2, 2);

        public static readonly Vector4 OneHalf = new Vector4(1.5f, 1.5f, 1.5f, 1.5f);

        public static readonly Vector4 Half = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);

        public static readonly Vector4 Quarter = new Vector4(0.25f, 0.25f, 0.25f, 0.25f);

        public static Vector4 V4(this float f)
        {
            return new Vector4(f, f, f, f);
        }

        public static Vector4 X2V4(this float f, float defaultValue = 0)
        {
            return new Vector4(f, defaultValue, defaultValue, defaultValue);
        }

        public static Vector4 Y2V4(this float f, float defaultValue = 0)
        {
            return new Vector4(defaultValue, f, defaultValue, defaultValue);
        }

        public static Vector4 Z2V4(this float f, float defaultValue = 0)
        {
            return new Vector4(defaultValue, defaultValue, f, defaultValue);
        }

        public static Vector4 W2V4(this float f, float defaultValue = 0)
        {
            return new Vector4(defaultValue, defaultValue, defaultValue, f);
        }

        public static Vector4 XYToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(f, f, defaultValue, defaultValue);
        }

        public static Vector4 YZToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(defaultValue, f, f, defaultValue);
        }

        public static Vector4 XZToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(f, defaultValue, f, defaultValue);
        }

        public static Vector4 XYZToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(f, f, f, defaultValue);
        }

        public static Vector4 XYWToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(f, f, defaultValue, f);
        }

        public static Vector4 XZWToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(f, defaultValue, f, f);
        }

        public static Vector4 YZWToV4(this float f, float defaultValue = 0)
        {
            return new Vector4(defaultValue, f, f, f);
        }

        public static Vector4 HoldX(this Vector4 v4, float defaultValue = 0)
        {
            v4.y = defaultValue;
            v4.z = defaultValue;
            v4.w = defaultValue;
            return v4;
        }

        public static Vector4 HoldY(this Vector4 v4, float defaultValue = 0)
        {
            v4.x = defaultValue;
            v4.z = defaultValue;
            v4.w = defaultValue;
            return v4;
        }

        public static Vector4 HoldZ(this Vector4 v4, float defaultValue = 0)
        {
            v4.x = defaultValue;
            v4.y = defaultValue;
            v4.w = defaultValue;
            return v4;
        }

        public static Vector4 HoldW(this Vector4 v4, float defaultValue = 0)
        {
            v4.x = defaultValue;
            v4.y = defaultValue;
            v4.z = defaultValue;
            return v4;
        }

        public static Vector4 X2Zero(this Vector4 v4)
        {
            return v4.ChangeX(0);
        }

        public static Vector4 Y2Zero(this Vector4 v4)
        {
            return v4.ChangeY(0);
        }

        public static Vector4 Z2Zero(this Vector4 v4)
        {
            return v4.ChangeZ(0);
        }

        public static Vector4 W2Zero(this Vector4 v4)
        {
            return v4.ChangeW(0);
        }

        public static Vector4 MergeV2(this Vector4 v4, Vector2 v2T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v4.x = v2T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v4.y = v2T.x;
                    break;
                case VectorMergeOperator.XToZ:
                    v4.z = v2T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v4.x = v2T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v4.y = v2T.y;
                    break;
                case VectorMergeOperator.YToZ:
                    v4.z = v2T.y;
                    break;
                case VectorMergeOperator.XYToXY:
                    v4.x = v2T.x;
                    v4.y = v2T.y;
                    break;
                case VectorMergeOperator.XYToXZ:
                    v4.x = v2T.x;
                    v4.z = v2T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v4.y = v2T.x;
                    v4.x = v2T.y;
                    break;
                case VectorMergeOperator.XYToYZ:
                    v4.y = v2T.x;
                    v4.z = v2T.y;
                    break;
                case VectorMergeOperator.XYToZX:
                    v4.z = v2T.x;
                    v4.x = v2T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v4.x = v2T.x;
                    v4.y = v2T.y;
                    break;
                case VectorMergeOperator.YXToXY:
                    v4.x = v2T.y;
                    v4.y = v2T.x;
                    break;
                case VectorMergeOperator.YXToXZ:
                    v4.x = v2T.y;
                    v4.z = v2T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v4.y = v2T.y;
                    v4.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v4.y = v2T.y;
                    v4.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToZX:
                    v4.z = v2T.y;
                    v4.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToZY:
                    v4.z = v2T.y;
                    v4.y = v2T.x;
                    break;
            }

            return v4;
        }

        public static Vector4 MergeV3(this Vector4 v4, Vector3 v3T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v4.x = v3T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v4.y = v3T.x;
                    break;
                case VectorMergeOperator.XToZ:
                    v4.z = v3T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v4.x = v3T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v4.y = v3T.y;
                    break;
                case VectorMergeOperator.YToZ:
                    v4.z = v3T.y;
                    break;
                case VectorMergeOperator.ZToX:
                    v4.x = v3T.z;
                    break;
                case VectorMergeOperator.ZToY:
                    v4.y = v3T.z;
                    break;
                case VectorMergeOperator.ZToZ:
                    v4.z = v3T.z;
                    break;
                case VectorMergeOperator.XYToXY:
                    v4.x = v3T.x;
                    v4.y = v3T.y;
                    break;
                case VectorMergeOperator.XYToXZ:
                    v4.x = v3T.x;
                    v4.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v4.y = v3T.x;
                    v4.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToYZ:
                    v4.y = v3T.x;
                    v4.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToZX:
                    v4.x = v3T.x;
                    v4.z = v3T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v4.z = v3T.x;
                    v4.y = v3T.y;
                    break;
                case VectorMergeOperator.XZToXY:
                    v4.x = v3T.x;
                    v4.y = v3T.z;
                    break;
                case VectorMergeOperator.XZToXZ:
                    v4.x = v3T.x;
                    v4.z = v3T.z;
                    break;
                case VectorMergeOperator.YXToXY:
                    v4.x = v3T.y;
                    v4.y = v3T.x;
                    break;
                case VectorMergeOperator.YXToXZ:
                    v4.x = v3T.y;
                    v4.z = v3T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v4.y = v3T.y;
                    v4.x = v3T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v4.y = v3T.y;
                    v4.z = v3T.x;
                    break;
                case VectorMergeOperator.YXToZX:
                    v4.z = v3T.y;
                    v4.x = v3T.x;
                    break;
                case VectorMergeOperator.YXToZY:
                    v4.z = v3T.y;
                    v4.y = v3T.x;
                    break;
                case VectorMergeOperator.ZYXToXYZ:
                    v4.x = v3T.z;
                    v4.y = v3T.y;
                    v4.z = v3T.x;
                    break;
            }

            return v4;
        }

        public static Vector4 MergeV4(this Vector4 v4, Vector4 v4T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v4.x = v4T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v4.y = v4T.x;
                    break;
                case VectorMergeOperator.XToZ:
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v4.x = v4T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v4.y = v4T.y;
                    break;
                case VectorMergeOperator.YToZ:
                    v4.z = v4T.y;
                    break;
                case VectorMergeOperator.ZToX:
                    v4.x = v4T.z;
                    break;
                case VectorMergeOperator.ZToY:
                    v4.y = v4T.z;
                    break;
                case VectorMergeOperator.ZToZ:
                    v4.z = v4T.z;
                    break;
                case VectorMergeOperator.XYToXY:
                    v4.x = v4T.x;
                    v4.y = v4T.y;
                    break;
                case VectorMergeOperator.XYToXZ:
                    v4.x = v4T.x;
                    v4.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v4.y = v4T.x;
                    v4.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToYZ:
                    v4.y = v4T.x;
                    v4.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToZX:
                    v4.x = v4T.x;
                    v4.z = v4T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v4.z = v4T.x;
                    v4.y = v4T.y;
                    break;
                case VectorMergeOperator.XZToXY:
                    v4.x = v4T.x;
                    v4.y = v4T.z;
                    break;
                case VectorMergeOperator.XZToXZ:
                    v4.x = v4T.x;
                    v4.z = v4T.z;
                    break;
                case VectorMergeOperator.YXToXY:
                    v4.x = v4T.y;
                    v4.y = v4T.x;
                    break;
                case VectorMergeOperator.YXToXZ:
                    v4.x = v4T.y;
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v4.y = v4T.y;
                    v4.x = v4T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v4.y = v4T.y;
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.YXToZX:
                    v4.z = v4T.y;
                    v4.x = v4T.x;
                    break;
                case VectorMergeOperator.YXToZY:
                    v4.z = v4T.y;
                    v4.y = v4T.x;
                    break;
                case VectorMergeOperator.ZYXToXYZ:
                    v4.x = v4T.z;
                    v4.y = v4T.y;
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.XToW:
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.YToW:
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.ZToW:
                    v4.w = v4T.z;
                    break;
                case VectorMergeOperator.WToX:
                    v4.x = v4T.w;
                    break;
                case VectorMergeOperator.WToY:
                    v4.y = v4T.w;
                    break;
                case VectorMergeOperator.WToZ:
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.WToW:
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.XYToXW:
                    v4.x = v4T.x;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.XYToYW:
                    v4.y = v4T.x;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.XYToZW:
                    v4.z = v4T.x;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.XZToXW:
                    v4.x = v4T.x;
                    v4.w = v4T.z;
                    break;
                case VectorMergeOperator.XWToXY:
                    v4.x = v4T.x;
                    v4.y = v4T.w;
                    break;
                case VectorMergeOperator.XWToXZ:
                    v4.x = v4T.x;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.XWToXW:
                    v4.x = v4T.x;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.YXToZW:
                    v4.z = v4T.y;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.YXToWX:
                    v4.w = v4T.y;
                    v4.x = v4T.x;
                    break;
                case VectorMergeOperator.YXToWY:
                    v4.w = v4T.y;
                    v4.y = v4T.x;
                    break;
                case VectorMergeOperator.YXToWZ:
                    v4.w = v4T.y;
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.XYZToXYZ:
                    v4.x = v4T.x;
                    v4.y = v4T.y;
                    v4.z = v4T.z;
                    break;
                case VectorMergeOperator.XYZToXYW:
                    v4.x = v4T.x;
                    v4.y = v4T.y;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.XYZToXZW:
                    v4.x = v4T.x;
                    v4.y = v4T.z;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.XYZToYZW:
                    v4.x = v4T.y;
                    v4.y = v4T.z;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.XYWToXYZ:
                    v4.x = v4T.x;
                    v4.y = v4T.y;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.XYWToXYW:
                    v4.x = v4T.x;
                    v4.y = v4T.y;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.XYWToXZW:
                    v4.x = v4T.x;
                    v4.z = v4T.y;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.XYWToYZW:
                    v4.y = v4T.x;
                    v4.z = v4T.y;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.XZWToXYZ:
                    v4.x = v4T.x;
                    v4.y = v4T.z;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.XZWToXYW:
                    v4.x = v4T.x;
                    v4.y = v4T.z;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.XZWToXZW:
                    v4.x = v4T.x;
                    v4.z = v4T.z;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.XZWToYZW:
                    v4.y = v4T.x;
                    v4.z = v4T.z;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.YZWToXYZ:
                    v4.x = v4T.y;
                    v4.y = v4T.z;
                    v4.z = v4T.w;
                    break;
                case VectorMergeOperator.YZWToXYW:
                    v4.x = v4T.y;
                    v4.y = v4T.z;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.YZWToXZW:
                    v4.x = v4T.y;
                    v4.z = v4T.z;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.YZWToYZW:
                    v4.y = v4T.y;
                    v4.z = v4T.z;
                    v4.w = v4T.w;
                    break;
                case VectorMergeOperator.WZYToXYZ:
                    v4.x = v4T.w;
                    v4.y = v4T.z;
                    v4.z = v4T.y;
                    break;
                case VectorMergeOperator.WZYToXYW:
                    v4.x = v4T.w;
                    v4.y = v4T.z;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.WZYToXZW:
                    v4.x = v4T.w;
                    v4.z = v4T.z;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.WZYToYZW:
                    v4.y = v4T.w;
                    v4.z = v4T.z;
                    v4.w = v4T.y;
                    break;
                case VectorMergeOperator.WZXToXYZ:
                    v4.x = v4T.w;
                    v4.y = v4T.z;
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.WZXToXYW:
                    v4.x = v4T.w;
                    v4.y = v4T.z;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.WZXToXZW:
                    v4.x = v4T.w;
                    v4.z = v4T.z;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.WZXToYZW:
                    v4.y = v4T.w;
                    v4.z = v4T.z;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.WYXToXYZ:
                    v4.x = v4T.w;
                    v4.y = v4T.y;
                    v4.z = v4T.x;
                    break;
                case VectorMergeOperator.WYXToXYW:
                    v4.x = v4T.w;
                    v4.y = v4T.y;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.WYXToXZW:
                    v4.x = v4T.w;
                    v4.z = v4T.y;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.WYXToYZW:
                    v4.y = v4T.w;
                    v4.z = v4T.y;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.ZYXToXYW:
                    v4.x = v4T.z;
                    v4.y = v4T.y;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.ZYXToXZW:
                    v4.x = v4T.z;
                    v4.z = v4T.y;
                    v4.w = v4T.x;
                    break;
                case VectorMergeOperator.ZYXToYZW:
                    v4.y = v4T.z;
                    v4.z = v4T.y;
                    v4.w = v4T.x;
                    break;
            }

            return v4;
        }

        public static Vector4 ChangeX(this Vector4 v4, float x, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector4(x, v4.y, v4.z, v4.w);
                case VectorOperator.Add:
                    return new Vector4(v4.x + x, v4.y, v4.z, v4.w);
                case VectorOperator.Sub:
                    return new Vector4(v4.x - x, v4.y, v4.z, v4.w);
                case VectorOperator.Multi:
                    return new Vector4(v4.x * x, v4.y, v4.z, v4.w);
                case VectorOperator.Divide:
                    return new Vector4(v4.x / x, v4.y, v4.z, v4.w);
                case VectorOperator.Mud:
                    return new Vector4(v4.x % x, v4.y, v4.z, v4.w);
                default:
                    return new Vector4(x, v4.y, v4.z, v4.w);
            }
        }

        public static Vector4 ChangeY(this Vector4 v4, float y, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector4(v4.x, y, v4.z, v4.w);
                case VectorOperator.Add:
                    return new Vector4(v4.x, v4.y + y, v4.z, v4.w);
                case VectorOperator.Sub:
                    return new Vector4(v4.x, v4.y - y, v4.z, v4.w);
                case VectorOperator.Multi:
                    return new Vector4(v4.x, v4.y * y, v4.z, v4.w);
                case VectorOperator.Divide:
                    return new Vector4(v4.x, v4.y / y, v4.z, v4.w);
                case VectorOperator.Mud:
                    return new Vector4(v4.x, v4.y % y, v4.z, v4.w);
                default:
                    return new Vector4(v4.x, y, v4.z, v4.w);
            }
        }

        public static Vector4 ChangeZ(this Vector4 v4, float z, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector4(v4.x, v4.y, z, v4.w);
                case VectorOperator.Add:
                    return new Vector4(v4.x, v4.y, v4.z + z, v4.w);
                case VectorOperator.Sub:
                    return new Vector4(v4.x, v4.y, v4.z - z, v4.w);
                case VectorOperator.Multi:
                    return new Vector4(v4.x, v4.y, v4.z * z, v4.w);
                case VectorOperator.Divide:
                    return new Vector4(v4.x, v4.y, v4.z / z, v4.w);
                case VectorOperator.Mud:
                    return new Vector4(v4.x, v4.y, v4.z % z, v4.w);
                default:
                    return new Vector4(v4.x, v4.y, z, v4.w);
            }
        }

        public static Vector4 ChangeW(this Vector4 v4, float w, VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector4(v4.x, v4.y, v4.z, w);
                case VectorOperator.Add:
                    return new Vector4(v4.x, v4.y, v4.z, v4.z + w);
                case VectorOperator.Sub:
                    return new Vector4(v4.x, v4.y, v4.z, v4.z - w);
                case VectorOperator.Multi:
                    return new Vector4(v4.x, v4.y, v4.z, v4.z * w);
                case VectorOperator.Divide:
                    return new Vector4(v4.x, v4.y, v4.z, v4.z / w);
                case VectorOperator.Mud:
                    return new Vector4(v4.x, v4.y, v4.z, v4.z % w);
                default:
                    return new Vector4(v4.x, v4.y, v4.z, w);
            }
        }

        public static Vector4 Divide(this Vector4 v4, Vector4 v4d)
        {
            return new Vector4(v4.x / v4d.x, v4.y / v4d.y, v4.z / v4d.z, v4.w / v4d.w);
        }

        public static Vector4 Divide(this Vector4 v4, Vector3 v3d)
        {
            return new Vector4(v4.x / v3d.x, v4.y / v3d.y, v4.z / v3d.z, 0);
        }

        public static Vector4 Divide(this Vector4 v4, Vector2 v2d)
        {
            return new Vector4(v4.x / v2d.x, v4.y / v2d.y, 0, 0);
        }

        public static Vector4 Multi(this Vector4 v4, Vector4 v4M)
        {
            return new Vector4(v4.x * v4M.x, v4.y * v4M.y, v4.z * v4M.z);
        }

        public static Vector4 Multi(this Vector4 v4, Vector3 v3M)
        {
            return new Vector4(v4.x * v3M.x, v4.y * v3M.y, v4.z / v3M.z, 0);
        }

        public static Vector4 Multi(this Vector4 v4, Vector2 v2M)
        {
            return new Vector4(v4.x * v2M.x, v4.y * v2M.y, 0, 0);
        }

        public static Vector2 XY(this Vector4 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 XZ(this Vector4 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector2 Xw(this Vector4 v)
        {
            return new Vector2(v.x, v.w);
        }

        public static Vector2 Yx(this Vector4 v)
        {
            return new Vector2(v.y, v.w);
        }

        public static Vector2 YZ(this Vector4 v)
        {
            return new Vector2(v.y, v.z);
        }

        public static Vector2 Yw(this Vector4 v)
        {
            return new Vector2(v.y, v.w);
        }

        public static Vector2 Zx(this Vector4 v)
        {
            return new Vector2(v.z, v.x);
        }

        public static Vector2 Zy(this Vector4 v)
        {
            return new Vector2(v.z, v.y);
        }

        public static Vector2 Zw(this Vector4 v)
        {
            return new Vector2(v.z, v.w);
        }

        public static bool Approximately(this Vector4 v4, Vector4 v4A, float delta = 0)
        {
            if (delta == 0)
            {
                return Mathf.Approximately(v4.x, v4A.x) && Mathf.Approximately(v4.y, v4A.y) && Mathf.Approximately(v4.z, v4A.z) && Mathf.Approximately(v4.w, v4A.w);
            }
            else
            {
                float x = Mathf.Abs(v4.x - v4A.x);
                float y = Mathf.Abs(v4.y - v4A.y);
                float z = Mathf.Abs(v4.z - v4A.z);
                float w = Mathf.Abs(v4.w - v4A.w);
                return x < delta && y < delta && z < delta && w < delta;
            }
        }
    }
}