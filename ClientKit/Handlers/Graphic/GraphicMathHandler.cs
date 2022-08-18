using Kits.DevlpKit.Helpers;

namespace Kits.ClientKit.Handlers.Graphic
{

    public static class GraphicMathHandler
    {
        public static double Gamma(double value, double absmax, double gamma)
        {
            bool flag = value < 0.0;
            double num1 = MathHelper.Abs(value);
            if (num1 > absmax)
            {
                if (flag)
                    return -num1;
                else
                    return num1;
            }
            else
            {
                double num2 = MathHelper.Pow(num1 / absmax, gamma) * absmax;
                if (flag)
                    return -num2;
                else
                    return num2;
            }
        }
    }

}