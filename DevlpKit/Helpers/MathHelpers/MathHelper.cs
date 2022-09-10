using System;

namespace Kits.DevlpKit.Helpers
{
    /// <summary>
    /// 数学工具箱
    /// </summary>
    public static partial class MathHelper
    {
        public const double PI = 3.141593d;
        
        public const double Infinity = double.PositiveInfinity;
        
        public const double NegativeInfinity = double.NegativeInfinity;
        
        public const double Deg2Rad = 0.01745329d;
        
        public const double Rad2Deg = 57.29578d;
        
        public const double Epsilon = 1.401298E-45d;
        
        /// <summary>
        /// 2 的平方根
        /// </summary>
        public const float Sqrt2 = 1.4142136f;

        /// <summary>
        /// 3 的平方根
        /// </summary>
        public const float Sqrt3 = 1.7320508f;

        /// <summary>
        /// Pi x 2
        /// </summary>
        public const float TwoPi = 6.2831853f;

        /// <summary>
        /// Pi / 2
        /// </summary>
        public const float HalfPi = 1.5707963f;

        /// <summary>
        /// 百万分之一
        /// </summary>
        public const float OneMillionth = 1e-6f;

        /// <summary>
        /// 一百万
        /// </summary>
        public const float Million = 1e6f;

        /// <summary>
        /// 计算 2 的指数
        /// </summary>
        public static double Exp2(double x)
        {
            return Math.Exp(x * 0.69314718055994530941723212145818);
        }


        /// <summary>
        /// 保留指定的有效位数, 对剩余部分四舍五入
        /// </summary>
        public static double RoundToSignificantDigits(double value, int digits = 15)
        {
            if (value == 0.0) return 0.0;

            double scale = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            return scale * Math.Round(value / scale, digits);
        }


        /// <summary>
        /// 保留指定的有效位数, 对剩余部分四舍五入
        /// </summary>
        public static float RoundToSignificantDigitsFloat(float value, int digits = 6)
        {
            return (float)RoundToSignificantDigits(value, digits);
        }


        /// <summary>
        /// 将参数单位化
        /// </summary>
        /// <returns> 正值返回 1, 负值返回 -1, 0 返回 0 </returns>
        public static float Normalize(float value)
        {
            if (value > 0f) return 1f;
            if (value < 0f) return -1f;
            return 0f;
        }


        /// <summary>
        /// 线性映射到 01
        /// </summary>
        public static float Map01(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// 线性映射
        /// </summary>
        public static float Map(float value, float min, float max, float outputMin, float outputMax)
        {
            return (value - min) / (max - min) * (outputMax - outputMin) + outputMin;
        }

        /// <summary>
        /// 反转插值
        /// </summary>
        /// <param name="t"> 单位化的时间, 即取值范围为 [0, 1] </param>
        /// <param name="interpolate"> 一个在 [0, 1] 上的插值方法 </param>
        /// <returns> 与给定插值方法关于 (0.5, 0.5) 中心对称的插值方法的插值结果 </returns>
        public static float InverseInterpolate(float t, Func<float, float> interpolate)
        {
            return 1f - interpolate(1f - t);
        }

        /// <summary>
        /// 一维 Cardinal Spline
        /// </summary>
        public static float CardinalSpline(
            float t,
            float p0, float p1, float p2, float p3,
            float tension = 0.5f)
        {
            float a = tension * (p2 - p0);
            float b = p2 - p1;
            float c = tension * (p3 - p1) + a - b - b;

            return p1 + t * a - t * t * (a + c - b) + t * t * t * c;
        }

        /// <summary>
        /// 将 [0, 1] 范围的指数映射到 [OneMillionth, Million], 0.5 对应 1
        /// 设计用于传递给 shader 中的 pow
        /// </summary>
        public static float BalancePowExponent(float exp01)
        {
            if (exp01 > 1f - OneMillionth * 0.5f) return Million;
            if (exp01 < OneMillionth * 0.5f) return OneMillionth;
            return (exp01 > 0.5f) ? (1.0f / (2.0f - exp01 - exp01)) : (exp01 + exp01);
        }

        public static float DegreesTo180Range(float angle)
        {
            angle %= 360.0f;

            if (angle > 180.0f)
                angle -= 360.0f;
            else if (angle < -180.0f)
                angle += 360.0f;

            return angle;
        }

        public static float AngleDiff(float a, float b)
        {
            float angleDiff = a - b;
            float invAngle = 360.0f - angleDiff;

            if (invAngle < angleDiff)
            {
                angleDiff = invAngle;
            }

            return angleDiff;
        }
        
        public static bool FloatRangeIntersects(float range1start, float range1end, float range2start, float range2end)
        {
            if (range1start <= range2end)
            {
                return (range1end >= range2start);
            }
            else if (range1end >= range2start)
            {
                return (range1start <= range2end);
            }

            return false;
        }

        public static double Sin(double d)
        {
            return Math.Sin(d);
        }

        public static double Cos(double d)
        {
            return Math.Cos(d);
        }

        public static double Tan(double d)
        {
            return Math.Tan(d);
        }

        public static double Asin(double d)
        {
            return Math.Asin(d);
        }

        public static double Acos(double d)
        {
            return Math.Acos(d);
        }

        public static double Atan(double d)
        {
            return Math.Atan(d);
        }

        public static double Atan2(double y, double x)
        {
            return Math.Atan2(y, x);
        }

        public static double Sqrt(double d)
        {
            return Math.Sqrt(d);
        }

        public static double Abs(double d)
        {
            return Math.Abs(d);
        }

        public static int Abs(int value)
        {
            return Math.Abs(value);
        }

        public static double Min(double a, double b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        public static double Min(params double[] values)
        {
            int length = values.Length;
            if (length == 0)
                return 0.0d;
            double num = values[0];
            for (int index = 1; index < length; ++index)
            {
                if (values[index] < num)
                    num = values[index];
            }
            return num;
        }

        public static int Min(int a, int b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        public static int Min(params int[] values)
        {
            int length = values.Length;
            if (length == 0)
                return 0;
            int num = values[0];
            for (int index = 1; index < length; ++index)
            {
                if (values[index] < num)
                    num = values[index];
            }
            return num;
        }

        public static double Max(double a, double b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        public static double Max(params double[] values)
        {
            int length = values.Length;
            if (length == 0)
                return 0d;
            double num = values[0];
            for (int index = 1; index < length; ++index)
            {
                if ((double)values[index] > (double)num)
                    num = values[index];
            }
            return num;
        }

        public static int Max(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        public static int Max(params int[] values)
        {
            int length = values.Length;
            if (length == 0)
                return 0;
            int num = values[0];
            for (int index = 1; index < length; ++index)
            {
                if (values[index] > num)
                    num = values[index];
            }
            return num;
        }

        public static double Pow(double d, double p)
        {
            return Math.Pow(d, p);
        }

        /// <summary>
        /// Return true if the value is a power of 2
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>True if a power of 2</returns>
        public static bool IsPowerOf2(int value)
        {
            return (value & (value - 1)) == 0;
        }
        
        public static double Exp(double power)
        {
            return Math.Exp(power);
        }

        public static double Log(double d, double p)
        {
            return Math.Log(d, p);
        }

        public static double Log(double d)
        {
            return Math.Log(d);
        }

        public static double Log10(double d)
        {
            return Math.Log10(d);
        }

        public static double Ceil(double d)
        {
            return Math.Ceiling(d);
        }

        public static double Floor(double d)
        {
            return Math.Floor(d);
        }

        public static double Round(double d)
        {
            return Math.Round(d);
        }

        public static int CeilToInt(double d)
        {
            return (int)Math.Ceiling(d);
        }

        public static int FloorToInt(double d)
        {
            return (int)Math.Floor(d);
        }

        public static int RoundToInt(double d)
        {
            return (int)Math.Round(d);
        }

        public static double Sign(double d)
        {
            return d >= 0.0 ? 1d : -1d;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        public static double Clamp01(double value)
        {
            if (value < 0.0)
                return 0.0d;
            if (value > 1.0)
                return 1d;
            else
                return value;
        }

        /// <summary>
        /// Return mod of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="mod">Mod value</param>
        /// <returns>Mode of value</returns>
        public static float Modulo(float value, float mod)
        {
            return value - mod * (float)Math.Floor(value / mod);
        }

        /// <summary>
        /// Return mod of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="mod">Mod value</param>
        /// <returns>Mode of value</returns>
        public static int Modulo(int value, int mod)
        {
            return (int)(value - mod * (float)Math.Floor((float)value / mod));
        }

        public static double MoveTowards(double current, double target, double maxDelta)
        {
            if (Abs(target - current) <= maxDelta)
                return target;
            else
                return current + Sign(target - current) * maxDelta;
        }

        public static double MoveTowardsAngle(double current, double target, double maxDelta)
        {
            target = current + DeltaAngle(current, target);
            return MoveTowards(current, target, maxDelta);
        }

        public static bool Approximately(double a, double b)
        {
            return Abs(b - a) < Max(1E-06d * Max(Abs(a), Abs(b)), 1.121039E-44d);
        }

        public static double Repeat(double t, double length)
        {
            return t - Floor(t / length) * length;
        }

        public static double PingPong(double t, double length)
        {
            t = Repeat(t, length * 2d);
            return length - Abs(t - length);
        }

        public static double DeltaAngle(double current, double target)
        {
            double num = Repeat(target - current, 360d);
            if (num > 180.0d)
                num -= 360d;
            return num;
        }
    }

}