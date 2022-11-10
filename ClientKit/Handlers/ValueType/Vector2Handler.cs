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
            return new Vector2(v2.x, 0);
        }

        public static Vector2 HoldY(this Vector2 v2)
        {
            return new Vector2(0, v2.y);
        }

        public static Vector2 MergeX(this Vector2 v2,Vector2 v2T)
        {
            return new Vector2(v2T.x, v2.y);
        }
        
        public static Vector2 MergeX(this Vector2 v2,Vector3 v3T)
        {
            return new Vector2(v3T.x, v2.y);
        }
        
        public static Vector2 MergeX(this Vector2 v2,Vector4 v4T)
        {
            return new Vector2(v4T.x, v2.y);
        }
        
        public static Vector2 MergeY(this Vector2 v2,Vector2 v2T)
        {
            return new Vector2(v2.x, v2T.y);
        }
        
        public static Vector2 MergeY(this Vector2 v2,Vector3 v3T)
        {
            return new Vector2(v2.x, v3T.y);
        }
        
        public static Vector2 MergeY(this Vector2 v2,Vector4 v4T)
        {
            return new Vector2(v2.x, v4T.y);
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
        
        public static Vector3 ChangeX(this Vector2 v3, float x,VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector3(x, v3.y);
                case VectorOperator.Add:
                    return new Vector3(v3.x + x, v3.y);
                case VectorOperator.Sub:
                    return new Vector3(v3.x - x, v3.y);
                case VectorOperator.Multi:
                    return new Vector3(v3.x * x, v3.y);
                case VectorOperator.Divide:
                    return new Vector3(v3.x / x, v3.y);
                case VectorOperator.Mud:
                    return new Vector3(v3.x % x, v3.y);
                default:
                    return new Vector3(x, v3.y);
            }
        }

        public static Vector3 ChangeY(this Vector2 v3, float y,VectorOperator vOp = VectorOperator.Equal)
        {
            switch (vOp)
            {
                case VectorOperator.Equal:
                    return new Vector3(v3.x, y);
                case VectorOperator.Add:
                    return new Vector3(v3.x, v3.y + y);
                case VectorOperator.Sub:
                    return new Vector3(v3.x, v3.y - y);
                case VectorOperator.Multi:
                    return new Vector3(v3.x, v3.y * y);
                case VectorOperator.Divide:
                    return new Vector3(v3.x, v3.y / y);
                case VectorOperator.Mud:
                    return new Vector3(v3.x, v3.y % y);
                default:
                    return new Vector3(v3.x, y);
            }
        }

        public static Vector2 Yx(this Vector2 v)
        {
            return new Vector2(v.y, v.x);
        }
        
        public static Vector3 V2ToV3EmptyX(Vector2 v)
        {
            return new Vector3(0.0f,v.x, v.y);
        }
        
        public static Vector3 V2ToV3EmptyY(Vector2 v)
        {
            return new Vector3(v.x, 0.0f, v.y);
        }
        
        public static Vector3 V2ToV3EmptyZ(Vector2 v)
        {
            return new Vector3(v.x, v.y, 0.0f);
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