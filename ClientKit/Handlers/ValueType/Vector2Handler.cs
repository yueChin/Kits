using UnityEngine;

namespace Kits.ClientKit.Handlers.ValueType
{
    public static class Vector2Handler
    {
        public static readonly Vector2 Double = new Vector2(2, 2);

        public static readonly Vector2 Half = new Vector2(0.5f, 0.5f);

        public static Vector2 V2(float f)
        {
            return new Vector2(f, f);
        }

        public static Vector2 XToV2(float f)
        {
            return new Vector2(f, 0);
        }
        
        public static Vector2 YToV2(float f)
        {
            return new Vector2(0, f);
        }
        
        public static Vector2 HoldX(this Vector2 v2)
        {
            v2.y = 0;
            return v2;
        }

        public static Vector2 HoldY(this Vector2 v2)
        {
            v2.x = 0;
            return v2;
        }

        public static Vector2 MergeV2(this Vector2 v2,Vector2 v2T,VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v2.x = v2T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v2.y = v2T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v2.x = v2T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v2.y = v2T.y;
                    break;
                case VectorMergeOperator.XYToXY:
                    v2.x = v2T.x;
                    v2.y = v2T.y;
                    break;
                case VectorMergeOperator.XYToYX:
                    v2.y = v2T.x;
                    v2.x = v2T.y;
                    break;
                case VectorMergeOperator.XYToZY:
                    v2.x = v2T.x;
                    v2.y = v2T.y;
                    break;
                case VectorMergeOperator.YXToXY:
                    v2.x = v2T.y;
                    v2.y = v2T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v2.y = v2T.y;
                    v2.x = v2T.x;
                    break;
                case VectorMergeOperator.YXToYZ:
                    v2.y = v2T.y;
                    v2.x = v2T.x;
                    break;
            }
            return v2;
        }

        public static Vector2 MergeV3(this Vector2 v2, Vector3 v3T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v2.x = v3T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v2.y = v3T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v2.x = v3T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v2.y = v3T.y;
                    break;
                case VectorMergeOperator.ZToX:
                    v2.x = v3T.z;
                    break;
                case VectorMergeOperator.ZToY:
                    v2.y = v3T.z;
                    break;
                case VectorMergeOperator.XYToXY:
                    v2.x = v3T.x;
                    v2.y = v3T.y;
                    break;
                case VectorMergeOperator.XZToXY:
                    v2.x = v3T.x;
                    v2.y = v3T.z;
                    break;
                case VectorMergeOperator.YXToXY:
                    v2.x = v3T.y;
                    v2.y = v3T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v2.y = v3T.y;
                    v2.x = v3T.x;
                    break;
            }
            return v2;
        }
        
        public static Vector2 MergeV4(this Vector2 v2, Vector4 v4T, VectorMergeOperator mergeV2Operator)
        {
            switch (mergeV2Operator)
            {
                case VectorMergeOperator.XToX:
                    v2.x = v4T.x;
                    break;
                case VectorMergeOperator.XToY:
                    v2.y = v4T.x;
                    break;
                case VectorMergeOperator.YToX:
                    v2.x = v4T.y;
                    break;
                case VectorMergeOperator.YToY:
                    v2.y = v4T.y;
                    break;
                case VectorMergeOperator.ZToX:
                    v2.x = v4T.z;
                    break;
                case VectorMergeOperator.ZToY:
                    v2.y = v4T.z;
                    break;
                case VectorMergeOperator.XYToXY:
                    v2.x = v4T.x;
                    v2.y = v4T.y;
                    break;
                case VectorMergeOperator.XZToXY:
                    v2.x = v4T.x;
                    v2.y = v4T.z;
                    break;
                case VectorMergeOperator.YXToXY:
                    v2.x = v4T.y;
                    v2.y = v4T.x;
                    break;
                case VectorMergeOperator.YXToYX:
                    v2.y = v4T.y;
                    v2.x = v4T.x;
                    break;
                case VectorMergeOperator.WToX:
                    v2.x = v4T.w;
                    break;
                case VectorMergeOperator.WToY:
                    v2.y = v4T.w;
                    break;
                case VectorMergeOperator.XWToXY:
                    v2.x = v4T.x;
                    v2.y = v4T.w;
                    break;
            }
            return v2;
        }

        public static Vector2 Y2Zero(this Vector2 v2)
        {
            return v2.ChangeY(0);
        }

        public static Vector2 X2Zero(this Vector2 v2)
        {
            return v2.ChangeX(0);
        }

        public static float Cross(this Vector2 a,Vector2 b)
        {
            return (a.x * b.y - b.x * a.y);
        }
        
        public static Vector2 ChangeX(this Vector2 v2, float x,VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    v2.x = x;
                    return v2;
                case VectorOperator.Add:
                    v2.x += x;
                    return v2;
                case VectorOperator.Sub:
                    v2.x -= x;
                    return v2;
                case VectorOperator.Multi:
                    v2.x *= x;
                    return v2;
                case VectorOperator.Divide:
                    v2.x /= x;
                    return v2;
                case VectorOperator.Mud:
                    v2.x %= x;
                    return v2;
                default:
                    v2.x = x;
                    return v2;
            }
        }

        public static Vector2 ChangeY(this Vector2 v2, float y,VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    v2.x = y;
                    return v2;
                case VectorOperator.Add:
                    v2.x += y;
                    return v2;
                case VectorOperator.Sub:
                    v2.x -= y;
                    return v2;
                case VectorOperator.Multi:
                    v2.x *= y;
                    return v2;
                case VectorOperator.Divide:
                    v2.x /= y;
                    return v2;
                case VectorOperator.Mud:
                    v2.x %= y;
                    return v2;
                default:
                    v2.x = y;
                    return v2;
            }
        }

        public static Vector2 YX(this Vector2 v2)
        {
            float value = v2.y;
            v2.x = v2.y;
            v2.y = value;
            return v2;
        }
        
        public static Vector3 V2ToV3EmptyX(this Vector2 v2)
        {
            return new Vector3(0, v2.x, v2.y);
        }
        
        public static Vector3 V2ToV3EmptyY(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }
        
        public static Vector3 V2ToV3EmptyZ(this Vector2 v2)
        {
            return new Vector3(v2.x, v2.y, 0);
        }
        
        public static Vector4 V2ToV4EmptyXY(this Vector2 v2)
        {
            return new Vector4(0, 0, v2.x, v2.y);
        }
        
        public static Vector4 V2ToV4EmptyXZ(this Vector2 v2)
        {
            return new Vector4(v2.x, 0, 0, v2.y);
        }
        
        public static Vector4 V2ToV4EmptyXW(this Vector2 v2)
        {
            return new Vector4(0, v2.x, v2.y, 0);
        }
        
        public static bool Approximately(this Vector2 v2,Vector2 v2A,float delta = 0)
        {
            if (delta == 0)
            {
                return Mathf.Approximately(v2.x, v2A.x) && Mathf.Approximately(v2.y, v2A.y) ;
            }
            else
            {
                float x = Mathf.Abs(v2.x - v2A.x);
                float y = Mathf.Abs(v2.y - v2A.y);
                return x < delta && y < delta ;
            }
        }
    }
}