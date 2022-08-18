
namespace Kits.DevlpKit.Helpers.ValueTypeHelpers
{
    public static class IntHelper
    {
        public static int TryParse(this string str, int defaultValue = 0)
        {
            int.TryParse(str, out int value);
            if (value == 0)
            {
                value = defaultValue;
            }

            return value;
        }

        public static string GetD2(this int value)
        {
            if (value >= 10 || value <= -10)
            {
                return value.ToString();
            }
            else
            {
                if (value >= 0)
                {
                    return $"{value:D2}";
                }
                else
                {
                    return $"-{value:D2}";
                }
            }
        }
    }

}

