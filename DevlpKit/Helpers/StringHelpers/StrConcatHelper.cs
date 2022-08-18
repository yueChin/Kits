using System.Text;
using System.Threading;

namespace Kits.DevlpKit.Helpers.StringHelpers
{
    public static class StrConcatHelper
    {
        static readonly ThreadLocal<StringBuilder> s_StringBuilder = new ThreadLocal<StringBuilder>(() => new StringBuilder(256));

        public static string FastConcat(this string str1, string value)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1).Append(value);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, int value)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1).Append(value);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, uint value)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1).Append(value);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, long value)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1).Append(value);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, float value)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1).Append(value);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, double value)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1).Append(value);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, string str2, string str3)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1);
                s_StringBuilder.Value.Append(str2);
                s_StringBuilder.Value.Append(str3);

                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, string str2, string str3, string str4)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1);
                s_StringBuilder.Value.Append(str2);
                s_StringBuilder.Value.Append(str3);
                s_StringBuilder.Value.Append(str4);


                return s_StringBuilder.Value.ToString();
            }
        }

        public static string FastConcat(this string str1, string str2, string str3, string str4, string str5)
        {
            {
                s_StringBuilder.Value.Clear();

                s_StringBuilder.Value.Append(str1);
                s_StringBuilder.Value.Append(str2);
                s_StringBuilder.Value.Append(str3);
                s_StringBuilder.Value.Append(str4);
                s_StringBuilder.Value.Append(str5);

                return s_StringBuilder.Value.ToString();
            }
        }
    }
}