using System;
using Kits.DevlpKit.Helpers;
using Kits.DevlpKit.Supplements.Structs;
using UnityEngine;
using Random = Kits.DevlpKit.Supplements.Structs.Random;
using Range = Kits.DevlpKit.Supplements.Structs.Range;

namespace Kits.ClientKit.Handlers
{
    /// <summary>
    /// Random 扩展
    /// </summary>
    public static partial class RandomHandler
    {
        public static int RandomSign()
        {
            return UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1;
        }

        
        public static Vector2 OnUnitCircle(this ref Random random)
        {
            double a = random.Next01() * MathHelper.TwoPi;
            return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
        }


        public static Vector3 OnUnitSphere(this ref Random random)
        {
            // http://mathworld.wolfram.com/SpherePointPicking.html

            double a = random.Next01() * MathHelper.TwoPi;
            double cosB = random.Next01() * 2 - 1;
            double sinB = Math.Sqrt(1 - cosB * cosB);

            return new Vector3(
                (float)(Math.Cos(a) * sinB),
                (float)cosB,
                (float)(Math.Sin(a) * sinB));
        }


        public static Vector2 InsideUnitCircle(this ref Random random)
        {
            return random.OnUnitCircle() * (float)Math.Sqrt(random.Next01());
        }


        public static Vector3 InsideUnitSphere(this ref Random random)
        {
            return random.OnUnitSphere() * (float)Math.Pow(random.Next01(), 1.0 / 3.0);
        }


        public static Vector2 InsideEllipse(this ref Random random, Vector2 radius)
        {
            return Vector2.Scale(random.InsideUnitCircle(), radius);
        }


        public static Vector3 InsideEllipsoid(this ref Random random, Vector3 radius)
        {
            return Vector3.Scale(random.InsideUnitSphere(), radius);
        }


        public static float InsideRange(this ref Random random, Range range)
        {
            return random.Range(range.min, range.max);
        }


        public static Vector2 InsideRange2(this ref Random random, Range2 range2)
        {
            return new Vector2(
                random.Range(range2.x.min, range2.x.max),
                random.Range(range2.y.min, range2.y.max));
        }


        public static Vector3 InsideRange3(this ref Random random, Range3 range3)
        {
            return new Vector3(
                random.Range(range3.x.min, range3.x.max),
                random.Range(range3.y.min, range3.y.max),
                random.Range(range3.z.min, range3.z.max));
        }

    } // class Extension

} // namespace UnityExtensions