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

        public static Vector2 Y2Zero(Vector2 v2)
        {
            return new Vector2(v2.x, 0);
        }

        public static Vector2 X2Zero(Vector2 v2)
        {
            return new Vector2(0, v2.y);
        }

        public static Vector2 ChangeX(Vector2 v2, float x)
        {
            return new Vector2(x, v2.y);
        }

        public static Vector2 ChangeY(Vector2 v2, float y)
        {
            return new Vector2(v2.x, y);
        }
    }
}