
namespace Kits.DevlpKit.Helpers.CurveHelpers
{
    public static class EasingHelper
    {
        public static double DampSmooth(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed)
        {
            double deltaTime = (double)UnityEngine.Time.deltaTime;
            return DampSmooth(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static double DampSmooth(double current, double target, ref double currentVelocity, double smoothTime)
        {
            double deltaTime = UnityEngine.Time.deltaTime;
            double maxSpeed = double.PositiveInfinity;
            return DampSmooth(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static double DampSmooth(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed, double deltaTime)
        {
            smoothTime = MathHelper.Max(0.0001d, smoothTime);
            double num1 = 2d / smoothTime;
            double num2 = num1 * deltaTime;
            double num3 = (1.0d / (1.0d + num2 + 0.479999989271164d * num2 * num2 + 0.234999999403954d * num2 * num2 * num2));
            double num4 = current - target;
            double num5 = target;
            double max = maxSpeed * smoothTime;
            double num6 = MathHelper.Clamp(num4, -max, max);
            target = current - num6;
            double num7 = (currentVelocity + num1 * num6) * deltaTime;
            currentVelocity = (currentVelocity - num1 * num7) * num3;
            double num8 = target + (num6 + num7) * num3;
            if (num5 - current > 0.0 == num8 > num5)
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }

        public static double DampSmoothAngle(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed, double deltaTime)
        {
            target = current + MathHelper.DeltaAngle(current, target);
            return DampSmooth(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }
    }
}

