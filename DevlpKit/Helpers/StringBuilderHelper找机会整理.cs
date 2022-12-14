using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Kits.DevlpKit.Helpers.StringHelpers;
using UnityEngine;

namespace Kits.DevlpKit.Helpers
{
    public enum StringFormat
    {
        None, // 无
        Lower, // 小写
        Upper, // 大写
        FirstUpper, // 首字母大写
        EachFirstUpper, // 每个单词首字母大写
    }

    public static class StringBuilderHelper
    {
        private static readonly ObjectPool<StringBuilder> s_Builders = new ObjectPool<StringBuilder>(32, () => new StringBuilder(128));
        
        public static StringBuilder GetBuilder()
        {
            StringBuilder builder = s_Builders.Get();
            builder.Remove(0, builder.Length);
            return builder;
        }

        public static StringBuilder GetBuilder(string data)
        {
            StringBuilder builder = s_Builders.Get();
            builder.Remove(0, builder.Length);
            builder.Append(data);
            return builder;
        }

        public static string ReleaseBuilder(StringBuilder builder)
        {
            string ret = builder.ToString();
            s_Builders.Add(builder);
            return ret;
        }

        public static void Release(StringBuilder builder)
        {
            s_Builders.Add(builder);
        }

        public static void Format(StringBuilder buf, StringFormat format)
        {
            switch (format)
            {
                case StringFormat.Lower:
                    for (int i = 0; i < buf.Length; i++)
                    {
                        buf[i] = CharHelper.ToLower(buf[i]);
                    }
                    break;
                case StringFormat.Upper:
                    for (int i = 0; i < buf.Length; i++)
                    {
                        buf[i] = CharHelper.ToUpper(buf[i]);
                    }
                    break;
                case StringFormat.FirstUpper:
                    if (buf.Length > 0)
                        buf[0] = CharHelper.ToUpper(buf[0]);
                    break;
                case StringFormat.EachFirstUpper:
                    bool trans = true;
                    for (int i = 0; i < buf.Length; i++)
                    {
                        char c = buf[i];
                        if (c == ' ' || c == '\t' || c == '\n')
                        {
                            trans = true;
                        }
                        else if (trans)
                        {
                            buf[i] = CharHelper.ToUpper(c);
                            trans = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public static string Format(string text, StringFormat format)
        {
            if (string.IsNullOrEmpty(text) || format == 0)
                return text;
            StringBuilder buf = GetBuilder(text);
            Format(buf, format);
            return ReleaseBuilder(buf);
        }

        // 根据大小写分割单词
        public static void SplitKeyWords(StringBuilder buf)
        {
            for (int i = buf.Length - 1; i > 0; i--)
            {
                if (buf[i] >= 'A' && buf[i] <= 'Z' && buf[i - 1] >= 'a' && buf[i - 1] <= 'z')
                {
                    buf.Insert(i, ' ');
                }
            }
        }

        public static string SplitKeyWords(string word)
        {
            StringBuilder buf = GetBuilder(word);
            SplitKeyWords(buf);
            return ReleaseBuilder(buf);
        }

        //public const string EMPTY = "\"\"";
        //public const string HEX_CHARS = "0123456789ABCDEFabcdef";
        private const string c_InvalidCharSet = ",<.>/?;:'\"[{]}\\|`~!@#$%^&*()-=+ \r\n\t";

        public static void ToSbc(StringBuilder buf)
        {
            for (int i = 0; i < buf.Length; i++)
            {
                if (c_InvalidCharSet.IndexOf(buf[i]) > -1)
                {
                    if (32 == buf[i])
                    {
                        buf[i] = (char)12288;
                    }
                    else if (buf[i] < 127)
                    {
                        buf[i] = (char)(buf[i] + 65248);
                    }
                }
            }
        }

        // to全角
        public static string ToSbc(string text)
        {
            StringBuilder buf = GetBuilder(text);
            ToSbc(buf);
            return ReleaseBuilder(buf);
        }

        public static bool ContainInvalidChar(string text)
        {
            char[] c = text.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c_InvalidCharSet.IndexOf(c[i]) > -1)
                {
                    return true;
                }
            }

            return false;
        }

        public static int LengthOfUTF8(string str)
        {
            int length = 0;
            char[] characters = str.ToCharArray();
            foreach (char c in characters)
            {
                int cInt = (int)c;
                if (cInt < 256)
                {
                    length++;
                }
                else
                {
                    length += 2;
                }
            }
            return length;
        }

        public static int GetCharIndexIgnoreCase(string str, char lowerChar)
        {
            if (str == null)
                return -1;
            int delta = 'a' - 'A';
            for (int i = str.Length - 1; i >= 0; i--)
            {
                char v = str[i];
                if (v >= 'A' && v <= 'Z')
                    v = (char)(v + delta);
                if (v == lowerChar)
                    return i;
            }
            return -1;
        }

        public static int GetCharIndex(string str, char w)
        {
            if (str == null)
                return -1;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                char v = str[i];
                if (v == w)
                    return i;
            }
            return -1;
        }

        public static string GetHex(byte[] bytes)
        {
            StringBuilder builder = GetBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                uint b = bytes[i];
                if (i > 0 && i % 2 == 0)
                    builder.Append('-');
                if (b < 0x10)
                    builder.Append('0');
                builder.Append(b.ToString("x"));
            }
            return ReleaseBuilder(builder);
        }

        public static string ToMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            StringBuilder buffer = GetBuilder(str);
            string ret = null;
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                buffer.Remove(0, buffer.Length);
                buffer.Append(BitConverter.ToString(hashBytes));
                int delta = 'A' - 'a';
                for (int i = buffer.Length - 1; i >= 0; i--)
                {
                    char c = buffer[i];
                    if (c == '-')
                        buffer.Remove(i, 1);
                    else if (c >= 'a' && c <= 'a')
                        buffer[i] = (char)(c + delta);
                }
                ret = buffer.ToString();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            s_Builders.Add(buffer);
            return ret;
        }

        public static string Hash(string str, int minsize = 6)
        {
            string value = ToHash(str).ToString("x");
            int len = value.Length;
            if (len < minsize)
            {
                StringBuilder buf = GetBuilder();
                for (int i = minsize - len; i > 0; i--)
                {
                    buf.Append('0');
                }
                buf.Append(value);
                value = ReleaseBuilder(buf);
            }
            return value;
        }

        public static int ToHash(string str)
        {
            int hash = 0;
            int len = str == null ? 0 : str.Length;
            for (int i = 0; i < len; i++)
            {
                hash = hash * 31 + str[i];
            }
            return hash;
        }

        public static int IgnoreCaseToHash(string str)
        {
            int hash = 0;
            int len = str == null ? 0 : str.Length;
            for (int i = 0; i < len; i++)
            {
                hash = hash * 31 + GetCharIgnoreCase(str, i);
            }
            return hash;
        }

        public static int IgnoreCaseToHash(char[] sequence, int offset, int len)
        {
            int hash = 0;
            int end = offset + len;
            for(int i= offset; i < end; i++)
            {
                hash = hash * 31 + CharHelper.ToLower(sequence[i]);
            }
            return hash;
        }

        public static int ToHash(char[] sequence, int offset, int len)
        {
            int hash = 0;
            int end = offset + len;
            for (int i = offset; i < end; i++)
            {
                hash = hash * 31 + sequence[i];
            }
            return hash;
        }

        public static char GetCharIgnoreCase(string str, int index)
        {
            char c = str[index];
            if (c >= 'A' && c <= 'Z')
                return (char)(c + CharHelper.Upper2Lower);
            else
                return c;
        }

        public static bool StartWithIgnoreCase(string str, string pattern)
        {
            int len = pattern.Length;
            if (str.Length < len)
                return false;
            for (int i = 0; i < len; i++)
            {
                if (GetCharIgnoreCase(str, i) != GetCharIgnoreCase(pattern, i))
                    return false;
            }
            return true;
        }

        public static bool EndWithIgnoreCase(this string str, string pattern)
        {
            int len = pattern.Length;
            int off = str.Length - len;
            if (off < 0)
                return false;
            for (int i = len - 1; i >= 0; i--)
            {
                if (GetCharIgnoreCase(str, off + i) != GetCharIgnoreCase(pattern, i))
                    return false;
            }
            return true;
        }

        public static bool EqualIgnoreCase(string a, string b)
        {
            int la = a == null ? 0 : a.Length;
            int lb = b == null ? 0 : b.Length;
            if (la != lb)
                return false;
            for (int i = 0; i < la; i++)
            {
                if (CharHelper.ToLower(a[i]) != CharHelper.ToLower(b[i]))
                    return false;
            }
            return true;
        }

        public static string Concat(params object[] strs)
        {
            if (strs == null || strs.Length == 0)
                return "";
            StringBuilder builder = GetBuilder();
            builder.Remove(0, builder.Length);
            int len = strs == null ? 0 : strs.Length;
            for (int i = 0; i < len; i++)
            {
                builder.Append(strs[i]);
            }
            return ReleaseBuilder(builder);
        }

        public static string Gather<T>(IList<T> lst, int count = -1, string seperator = ",")
        {
            if (lst == null)
                return "";
            if (count < 0)
                count = lst.Count;
            StringBuilder buf = GetBuilder();
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    buf.Append(seperator);
                buf.Append(lst[i]);
            }
            return ReleaseBuilder(buf);
        }

        public static string Gather(IEnumerator iter, string seperator = ",")
        {
            if (iter == null)
                return "";
            StringBuilder buf = GetBuilder();
            if (string.IsNullOrEmpty(seperator))
            {
                while (iter.MoveNext())
                {
                    buf.Append(iter.Current);
                }
            }
            else
            {
                bool first = true;
                while (iter.MoveNext())
                {
                    if (first)
                        first = false;
                    else
                        buf.Append(seperator);
                    buf.Append(iter.Current);
                }
            }
            return ReleaseBuilder(buf);
        }

        public static string FormatTime(float seconds)
        {
            string s = "{0:00}:{1:00}:{2:00}";
            int sec = Mathf.CeilToInt(seconds);
            int minute = sec / 60;
            sec = sec % 60;
            int hour = minute / 60;
            minute = minute % 60;

            return string.Format(s, hour, minute, sec);
        }

        public static string ReplaceLast(string format, char oldChar, char newChar)
        {
            int index = format.LastIndexOf(oldChar);
            if (index >= 0)
            {
                StringBuilder buf = GetBuilder();
                if (index > 0)
                    buf.Append(format, 0, index);
                buf.Append(newChar);
                if (index < format.Length - 1)
                    buf.Append(format, index + 1, format.Length - index - 1);
                return ReleaseBuilder(buf);
            }
            else
            {
                return format;
            }
        }

        public static string ReplaceFirst(string format, char oldChar, char newChar)
        {
            int index = format.IndexOf(oldChar);
            if (index >= 0)
            {
                StringBuilder buf = GetBuilder();
                if (index > 0)
                    buf.Append(format, 0, index);
                buf.Append(newChar);
                if (index < format.Length - 1)
                    buf.Append(format, index + 1, format.Length - index - 1);
                return ReleaseBuilder(buf);
            }
            else
            {
                return format;
            }
        }

        public static string Replace(string format, params object[] matches)
        {
            StringBuilder buf = GetBuilder(format);
            int len = matches == null ? 0 : matches.Length;
            for (int i = 0; i < len; i += 2)
            {
                string key = matches[i] == null ? null : matches[i].ToString();
                if (string.IsNullOrEmpty(key))
                    continue;
                string v = matches[i + 1] == null ? "" : matches[i + 1].ToString();
                buf.Replace(key, v);
            }
            return ReleaseBuilder(buf);
        }

        public static string WrapString(string text, int length, string replaceStr)
        {
            int len = text.Length;
            if (len <= length)
                return text;
            int n = replaceStr.Length;
            string s = text.Substring(0, Mathf.Min(length, len) - n);
            s += replaceStr;
            return s;
        }

        public static string WrapRichText(string text, int length, string replaceStr)
        {
            MatchCollection mats = Regex.Matches(text, @"<quad=[a-zA-Z0-9_]+>|<size=\d+>|<color=[\w\W]+>|</color>|</size>|</?b>|</?i>");
            StringBuilder buf = GetBuilder();
            int num = 0;
            int p = 0;
            Match m = mats == null && p < mats.Count ? mats[p++] : null;
            int cp = 0;
            while (num < length && cp < text.Length)
            {
                if (m == null || m.Index > cp)
                {
                    buf.Append(text[cp++]);
                    num++;
                }
                else
                {
                    for (int i = m.Index; i < m.Length + m.Index; i++)
                    {
                        buf.Append(text[i]);
                    }
                    cp = m.Length + m.Index;
                    m = p < mats.Count ? mats[p++] : null;
                }
            }
            if (cp < text.Length)
                buf.Append(replaceStr);
            return ReleaseBuilder(buf);
        }

        public static string WrapLinePerChar(string text, int charCount)
        {
            char l = '\n';
            StringBuilder str = GetBuilder(text);
            for (int i = charCount; i < str.Length; i += charCount)
            {
                str.Insert(i++, l);
            }
            return ReleaseBuilder(str);
        }

        public static bool ParseArray(string str, ICollection<string> arr, char split = ',', char bracketl = '\0', char bracketr = '\0')
        {
            int p = 0;
            int offset = 0;
            StringBuilder buf = GetBuilder(str);
            if (bracketl == '\0' && bracketr == '\0')
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    char c = buf[i];
                    if (c == split)
                    {
                        arr.Add(buf.ToString(offset, i - offset));
                        offset = i + 1;
                    }
                }
                if (offset < buf.Length)
                    arr.Add(buf.ToString(offset, buf.Length - offset));
                s_Builders.Add(buf);
                return true;
            }
            for (int i = 0; i < buf.Length; i++)
            {
                char c = buf[i];
                if (c == bracketl)
                {
                    p++;
                    if (p == 1)
                        offset = i + 1;
                }
                else if (c == bracketr)
                {
                    p--;
                    if (p == 0)
                    {
                        if (offset < i)
                            arr.Add(buf.ToString(offset, i - offset));
                        offset = i + 1;
                    }
                }
                else if (c == split && p == 1)
                {
                    arr.Add(buf.ToString(offset, i - offset));
                    offset = i + 1;
                }
                else if (p < 1)
                {
                    return false;
                }
            }
            s_Builders.Add(buf);
            return p == 0;
        }

        public static string[][] ParseMatrix(string s, char split = ',', char bracketl = '[', char bracketr = ']')
        {
            if (string.IsNullOrEmpty(s))
                return new string[0][];
            List<string> tmp = new List<string>();
            string[][] matrix;
            if (!ParseArray(s, tmp, split, bracketl, bracketr))
                return null;
            matrix = new string[tmp.Count][];
            List<string> tmp2 = new List<string>();
            for (int i = 0; i < matrix.Length; i++)
            {
                tmp2.Clear();
                if (!ParseArray(tmp[i], tmp2, split, bracketl, bracketr))
                    return null;
                string[] arr = new string[tmp2.Count];
                for (int j = 0; j < arr.Length; j++)
                {
                    arr[j] = tmp2[j];
                }
                matrix[i] = arr;
            }
            return matrix;
        }

        public static int[] ParseArray(string s, char split = ',', char bracketl = '\0', char bracketr = '\0')
        {
            List<string> arr = new List<string>(5);
            arr.Clear();
            if (!ParseArray(s, arr, split, bracketl, bracketr))
            {
                return null;
            }
            int[] result = new int[arr.Count];
            int num;
            for (int i = 0; i < result.Length; i++)
            {
                if (!int.TryParse(arr[i].Trim(), out num))
                    return null;
                result[i] = num;
            }
            return result;
        }

        public static float[] ParseFloatArray(string s, char split = ',', char bracketl = '\0', char bracketr = '\0')
        {
            List<string> arr = new List<string>(5);
            if (!ParseArray(s, arr, split, bracketl, bracketr))
            {
                return null;
            }
            float[] result = new float[arr.Count];
            float num;
            for (int i = 0; i < result.Length; i++)
            {
                if (!float.TryParse(arr[i].Trim(), out num))
                    return null;
                result[i] = num;
            }
            return result;
        }

        /// <summary>
        /// 将字符串(x,y,z)转换为 Vector3
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3 ParseVector3(string s)
        {
            string t = s.Trim();
            float[] arr = ParseFloatArray(t, ',', '(', ')');
            if (arr == null)
                return default(Vector3);
            Vector3 v = Vector3.zero;
            if (arr.Length > 0)
                v.x = arr[0];
            if (arr.Length > 1)
                v.y = arr[1];
            if (arr.Length > 2)
                v.z = arr[2];
            return v;
        }

        public static Vector2 ParseVector2(string s)
        {
            string t = s.Trim();
            float[] arr = ParseFloatArray(t, ',', '(', ')');
            if (arr == null)
                return default(Vector2);
            Vector2 v = Vector2.zero;
            if (arr.Length > 0)
                v.x = arr[0];
            if (arr.Length > 1)
                v.y = arr[1];
            return v;
        }
        public static Vector4 ParseVector4(string s)
        {
            string t = s.Trim();
            float[] arr = ParseFloatArray(t, ',', '(', ')');
            if (arr == null)
                return default(Vector4);
            Vector4 v = Vector4.zero;
            if (arr.Length > 0)
                v.x = arr[0];
            if (arr.Length > 1)
                v.y = arr[1];
            if (arr.Length > 2)
                v.z = arr[2];
            if (arr.Length > 3)
                v.w = arr[4];
            return v;
        }
        
        public static string ChineseNumber(int num, bool upcase)
        {
            num = Mathf.Clamp(num, 0, 99999);
            string number;
            if (upcase)
                number = "零壹贰叁肆五陆柒捌玖拾佰仟萬";
            else
                number = "〇一二三四五六七八九十百千万";
            if (num == 0)
                return number[0].ToString();
            StringBuilder s = GetBuilder();
            if (num < 20)
            {
                int n = num / 10;
                if (n > 0)
                    s.Append(number[10]);
                n = num % 10;
                if (num < 10 || n > 0)
                    s.Append(number[n]);
            }
            else
            {
                bool zero = true;
                string unit = "";
                int uindex = 9;
                while (num > 0)
                {
                    int n = num % 10;
                    num /= 10;
                    if (n > 0)
                    {
                        s.Insert(0, unit).Insert(0, number[n]);
                    }
                    else if (num > 0 && !zero)
                    {
                        s.Insert(0, number[0]);
                    }
                    if (num > 0)
                        unit = number[++uindex].ToString();
                    zero = n == 0;
                }
            }
            return ReleaseBuilder(s);
        }


        public static string ToMask(int value, string connector, string[] names)
        {
            StringBuilder buf = GetBuilder();
            int n = 1;
            for (int i = 0; i < Mathf.Min(32, names.Length); i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    if (buf.Length > 0)
                        buf.Append(connector);
                    buf.Append(names[i]);
                }
            }
            return ReleaseBuilder(buf);
        }
    }

}