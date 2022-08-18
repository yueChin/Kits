using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Kits.ClientKit.Handlers
{
    public static class ColorHandler
    {
        public static readonly Regex RichColorReg = new Regex("<color=#([a-f0-9]{8})>", RegexOptions.IgnoreCase);

        public const int ColorMax = 255;


        //Color color = Color.white;
        //ColorUtility.TryParseHtmlString(hex, out color); 推荐
        public static Color32 HexToColor(string hex)
        {
            hex = hex.Replace("0x", string.Empty);
            hex = hex.Replace("#", string.Empty);
            byte a = byte.MaxValue;
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            

            return new Color32(r, g, b, a);
        }

        private static string ColorToHex(Color32 color, bool isAlpha = false)
        {
            return isAlpha
                ? $"#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}"
                : $"#{color.r:X2}{color.g:X2}{color.b:X2}";
        }
        
        public static float GetPerceivedBrightness(Color color)
        {
            return Mathf.Sqrt(
                0.241f * color.r * color.r +
                0.691f * color.g * color.g +
                0.068f * color.b * color.b);
        }
        
        public static uint ToARGB32(Color c)
        {
            return ((uint)(c.a * 255) << 24)
                   | ((uint)(c.r * 255) << 16)
                   | ((uint)(c.g * 255) << 8)
                   | ((uint)(c.b * 255));
        }
        
        public static Color FromARGB32(uint argb)
        {
            return new Color(
                ((argb >> 16) & 0xFF) / 255f,
                ((argb >> 8) & 0xFF) / 255f,
                ((argb) & 0xFF) / 255f,
                ((argb >> 24) & 0xFF) / 255f);

        }
        
        public static Color HueToColor(float hue)
        {
            return new Color(
                HueToGreen(hue + 1f / 3f),
                HueToGreen(hue),
                HueToGreen(hue - 1f / 3f));

            float HueToGreen(float h)
            {
                h = ((h % 1f + 1f) % 1f) * 6f;

                if (h < 1f) return h;
                if (h < 3f) return 1f;
                if (h < 4f) return (4f - h);
                return 0f;
            }
        }


        public static bool SetColor(ref Color currentValue, Color newValue)
        {
            if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
                return false;

            currentValue = newValue;
            return true;
        }
        
        public static Color SetAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color LerpRGB(Color from, Color to, float t)
        {
            return new Color(Mathf.Lerp(from.r, to.r, t), Mathf.Lerp(from.g, to.g, t), Mathf.Lerp(from.b, to.b, t), from.a);
        }

        public static Color Invert(Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
                
            float hue = h + 0.5f;
            if (hue > 1f)
                hue -= 1f;

            return Color.HSVToRGB(hue, s, v);
        }

        ///<summary>A greyscale color</summary>
        public static Color Grey(float value) {
            return new Color(value, value, value);
        }

        ///<summary>The color, with alpha</summary>
        public static Color WithAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }
        
        public static bool Approximately(Color a, Color b, float epsilon)
        {
            return MathHandler.Approximately(a.r, b.r, epsilon) && MathHandler.Approximately(a.g, b.g, epsilon) && MathHandler.Approximately(a.b, b.b, epsilon);
        }
        
        /// <summary>
        /// Get a color from a html string
        /// </summary>
        /// <param name="htmlString">Color in RRGGBB or RRGGBBBAA or #RRGGBB or #RRGGBBAA format.</param>
        /// <returns>Color or white if unable to parse it.</returns>
        public static Color GetColorFromHTML(string htmlString)
        {
            Color color = Color.white;
            if (!htmlString.StartsWith("#"))
            {
                htmlString = "#" + htmlString;
            }
            if (!ColorUtility.TryParseHtmlString(htmlString, out color))
            {
                color = Color.white;
            }
            return color;
        }
        /// <summary>
        /// Converts the kelvin to a color
        /// </summary>
        public static Color ExecuteKelvinColor(float value)
        {
            return Mathf.CorrelatedColorTemperatureToRGB(value);
        }
        
        /// <summary>
        /// Checks if all the keys have the same check value
        /// </summary>
        /// <param name="colorKeys"></param>
        /// <param name="checkColor"></param>
        /// <returns></returns>
        public static bool CheckGradientColorKeys(GradientColorKey[] colorKeys, Color checkColor)
        {
            if (checkColor == null || colorKeys.Length < 1)
            {
                return true;
            }

            int checkCount = 0;
            for (int i = 0; i < colorKeys.Length; i++)
            {
                if (colorKeys[i].color == checkColor)
                {
                    checkCount++;
                }
            }

            if (checkCount == colorKeys.Length)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Checks if 2 gradient color and time values are not the same
        /// </summary>
        /// <param name="colorKeys"></param>
        /// <param name="checkKeys"></param>
        /// <returns></returns>
        public static bool CheckGradientColorKeys(GradientColorKey[] colorKeys, GradientColorKey[] checkKeys)
        {
            if (checkKeys.Length < 1 || colorKeys.Length < 1)
            {
                return true;
            }

            if (colorKeys.Length != checkKeys.Length)
            {
                return true;
            }

            for (int i = 0; i < colorKeys.Length; i++)
            {
                if (colorKeys[i].color.r != checkKeys[i].color.r)
                {
                    return true;
                }
                if (colorKeys[i].color.g != checkKeys[i].color.g)
                {
                    return true;
                }
                if (colorKeys[i].color.b != checkKeys[i].color.b)
                {
                    return true;
                }
                if (colorKeys[i].time != checkKeys[i].time)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Checks if the color is not null or does not equal the color check value
        /// </summary>
        /// <param name="checkColor"></param>
        /// <param name="colorValue"></param>
        /// <returns></returns>
        public static bool CheckColorKey(Color checkColor, Color colorValue)
        {
            if (checkColor == null || checkColor.r == colorValue.r && checkColor.g == colorValue.g && checkColor.b == colorValue.b)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Checks if two colors are equal or "look the same" according to their argb color values. 
        /// A regular comparison with "==" or .Equals does not work to compare if two colors are "the same" since the objects can still have different fields beyond the argb values.
        /// </summary>
        /// <param name="firstColor">first color to compare</param>
        /// <param name="secondColor">second color to compare</param>
        /// <returns></returns>
        public static bool ColorsEqual(Color firstColor, Color secondColor)
        {
            return (firstColor.r == secondColor.r) &&
                   (firstColor.g == secondColor.g) &&
                   (firstColor.b == secondColor.b) &&
                   (firstColor.a == secondColor.a);
        }
        
        /// <summary>
        /// Color inverter for seasonal color setup
        /// </summary>
        /// <param name="i_color"></param>
        /// <returns></returns>
        public static Color ColorInvert(Color color)
        {
            Color result;

            result.r = 1.0f - color.r;
            result.g = 1.0f - color.g;
            result.b = 1.0f - color.b;
            result.a = 1.0f - color.a;

            return (result);
        }
    }

}

