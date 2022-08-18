
namespace Kits.DevlpKit.Helpers.CurveHelpers
{
    public static class InterpolationHelper
    {
        public static double InverseLerp(double from, double to, double value)
        {
            if (from < to)
            {
                if (value < from)
                    return 0d;
                if (value > to)
                    return 1d;
                value -= from;
                value /= to - from;
                return value;
            }
            else
            {
                if (from <= to)
                    return 0d;
                if (value < to)
                    return 1d;
                if (value > from)
                    return 0d;
                else
                    return (1.0d - (value - to) / (from - to));
            }
        }
        
        public static double Lerp(double from, double to, double t)
        {
            return from + (to - from) * MathHelper.Clamp01(t);
        }

        public static double LerpAngle(double a, double b, double t)
        {
            double num = MathHelper.Repeat(b - a, 360d);
            if (num > 180.0d)
                num -= 360d;
            return a + num * MathHelper.Clamp01(t);
        }
        
        public static double LerpSmoothStep(double from, double to, double t)
        {
            t = MathHelper.Clamp01(t);
            t = (-2.0 * t * t * t + 3.0 * t * t);
            return to * t + from * (1.0 - t);
        }
        
        /// <summary>
        /// Linear interpolation between two values
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <param name="fraction">Fraction</param>
        /// <returns></returns>
        public static float LerpLinear(float value1, float value2, float fraction)
        {
            return value1 * (1f - fraction) + value2 * fraction;
        }

        /// <summary>
        /// Smooth interpolation between two values
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <param name="fraction">Fraction</param>
        /// <returns></returns>
        public static float LerpSmooth(float value1, float value2, float fraction)
        {
            if (fraction < 0.5f)
            {
                fraction = 2f * fraction * fraction;
            }
            else
            {
                fraction = 1f - 2f * (fraction - 1f) * (fraction - 1f);
            }
            return value1 * (1f - fraction) + value2 * fraction;
        }
        
        public static float LerpSmooth2(float v1, float v2, float fraction)
        {
            float fraction2 = fraction * fraction;
            fraction = 3 * fraction2 - 2f * fraction * fraction2;
            return v1 * (1f - fraction) + v2 * fraction;
        }

        public static float LerpCubic(float v0, float v1, float v2, float v3, float fraction)
        {
            float p = (v3 - v2) - (v0 - v1);
            float q = (v0 - v1) - p;
            float r = v2 - v0;
            float fraction2 = fraction * fraction;
            return p * fraction * fraction2 + q * fraction2 + r * fraction + v1;
        }
    }
}

