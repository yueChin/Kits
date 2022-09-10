using System;
using System.Text;

namespace Kits.DevlpKit.Helpers.StringHelpers
{
    public static partial class StringHelper
    {
        /// <summary>
        /// 找到第一个空白字符
        /// </summary>
        public static int IndexOfWhiteSpace(string s, int startIndex = 0)
        {
            while (startIndex < s.Length)
            {
                if (char.IsWhiteSpace(s, startIndex))
                {
                    return startIndex;
                }
                startIndex++;
            }
            return -1;
        }


        /// <summary>
        /// 找到第一个非空白字符
        /// </summary>
        public static int IndexOfNonWhiteSpace(string s, int startIndex = 0)
        {
            while (startIndex < s.Length)
            {
                if (!char.IsWhiteSpace(s, startIndex))
                {
                    return startIndex;
                }
                startIndex++;
            }
            return -1;
        }


        /// <summary>
        /// 找到最后一个空白字符
        /// </summary>
        public static int LastIndexOfWhiteSpace(string s, int startIndex)
        {
            while (startIndex >= 0)
            {
                if (char.IsWhiteSpace(s, startIndex))
                {
                    return startIndex;
                }
                startIndex--;
            }
            return -1;
        }


        /// <summary>
        /// 找到最后一个非空白字符
        /// </summary>
        public static int LastIndexOfNonWhiteSpace(string s, int startIndex)
        {
            while (startIndex >= 0)
            {
                if (!char.IsWhiteSpace(s, startIndex))
                {
                    return startIndex;
                }
                startIndex--;
            }
            return -1;
        }


        /// <summary>
        /// 在 StringBuilder 中查找第一个指定字符
        /// </summary>
        public static int IndexOf(StringBuilder builder, char character, int startIndex)
        {
            while (startIndex < builder.Length)
            {
                if (builder[startIndex] == character)
                {
                    return startIndex;
                }
                startIndex++;
            }
            return -1;
        }

        public static string CheckEmpty(this string str, string defaultStr = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultStr;
            }
            else
            {
                return str;
            }
        }
        
        private static string s_OperateString;
		private static int s_OperateIndex = 0;

		/// <summary>
		/// 设置要处理的字符串
		/// </summary>
		public static void SetOperateString(string content)
		{
			s_OperateString = content;
			s_OperateIndex = 0;
		}

		public static bool NextFloat(char separator, out float value)
		{
			value = 0;

#if MOTION_SERVER
			ReadOnlySpan<char> span = MoveNext(separator);		
#else
			string span = MoveNext(separator);
#endif

			if (span == null)
			{
				return false;
			}
			else
			{
				value = float.Parse(span);
				return true;
			}
		}
		public static bool NextDouble(char separator, out double value)
		{
			value = 0;

#if MOTION_SERVER
			ReadOnlySpan<char> span = MoveNext(separator);		
#else
			string span = MoveNext(separator);
#endif

			if (span == null)
			{
				return false;
			}
			else
			{
				value = double.Parse(span);
				return true;
			}
		}
		public static bool NextInt(char separator, out int value)
		{
			value = 0;

#if MOTION_SERVER
			ReadOnlySpan<char> span = MoveNext(separator);
#else
			string span = MoveNext(separator);
#endif

			if (span == null)
			{
				return false;
			}
			else
			{
				value = int.Parse(span);
				return true;
			}
		}
		public static bool NextLong(char separator, out long value)
		{
			value = 0;

#if MOTION_SERVER
			ReadOnlySpan<char> span = MoveNext(separator);
#else
			string span = MoveNext(separator);		
#endif

			if (span == null)
			{
				return false;
			}
			else
			{
				value = long.Parse(span);
				return true;
			}
		}
		public static bool NextString(char separator, out string value)
		{
			value = null;

#if MOTION_SERVER
			ReadOnlySpan<char> span = MoveNext(separator);
#else
			string span = MoveNext(separator);
#endif

			if (span == null)
			{
				return false;
			}
			else
			{
				value = span.ToString();
				return true;
			}
		}

#if MOTION_SERVER
		private static ReadOnlySpan<char> MoveNext(char separator)
#else
		private static string MoveNext(char separator)
#endif
		{
			int beginIndex = s_OperateIndex;

			for (int i = s_OperateIndex; i < s_OperateString.Length; i++)
			{
				bool isLastChar = s_OperateIndex == s_OperateString.Length - 1;
				bool isSeparatorChar = s_OperateString[i] == separator;

				if (isSeparatorChar || isLastChar)
				{
					if (isLastChar && isSeparatorChar == false)
						s_OperateIndex++;

					int charCount = s_OperateIndex - beginIndex;
					if (charCount == 0)
					{
						throw new InvalidOperationException($"Invalid operate string : {s_OperateString}");
					}

					s_OperateIndex++;

#if MOTION_SERVER
					return _operateString.AsSpan(beginIndex, charCount);
#else
					return s_OperateString.Substring(beginIndex, charCount);				
#endif
				}
				else
				{
					s_OperateIndex++;
				}
			}

			return null; //移动失败返回NULL
		}
		
    }
}
