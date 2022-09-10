using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Kits.DevlpKit.Helpers.StringHelpers
{
	public static partial class StringHelper
	{
		public static IEnumerable<byte> ToBytes(this string str)
		{
			byte[] byteArray = Encoding.Default.GetBytes(str);
			return byteArray;
		}

		public static byte[] ToByteArray(this string str)
		{
			byte[] byteArray = Encoding.Default.GetBytes(str);
			return byteArray;
		}

	    public static byte[] ToUTF8(this string str)
	    {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return byteArray;
        }

		public static byte[] HexToBytes(this string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
			}

			byte[] hexAsBytes = new byte[hexString.Length / 2];
			for (int index = 0; index < hexAsBytes.Length; index++)
			{
				string byteValue = "";
				byteValue += hexString[index * 2];
				byteValue += hexString[index * 2 + 1];
				hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return hexAsBytes;
		}

		public static string ListToString<T>(this List<T> list)
		{
			StringBuilder sb = new StringBuilder();
			foreach (T t in list)
			{
				sb.Append(t);
				sb.Append(",");
			}
			return sb.ToString();
		}

        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, "^[/a-zA-z]+[/0-9]+$");
        }
        public static bool IsValidEmail(string mail)
        {
            return Regex.IsMatch(mail, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }
        
        /// <summary>
		/// 正则表达式
		/// </summary>
		private static readonly Regex s_Regex = new Regex(@"\{[-+]?[0-9]+\.?[0-9]*\}", RegexOptions.IgnoreCase);

		/// <summary>
		/// 替换掉字符串里的特殊字符
		/// </summary>
		public static string ReplaceSpecialChar(string str)
		{
			return str.Replace("\\n", "\n").Replace("\\r", "'\r").Replace("\\t", "\t");
		}

		/// <summary>
		/// 字符串转换为字符串
		/// </summary>
		public static string StringToString(string str)
		{
			return str;
		}

		/// <summary>
		/// 字符串转换为BOOL
		/// </summary>
		public static bool StringToBool(string str)
		{
			int value = (int)Convert.ChangeType(str, typeof(int));
			return value > 0;
		}

		/// <summary>
		/// 字符串转换为数值
		/// </summary>
		public static T StringToValue<T>(string str)
		{
			return (T)Convert.ChangeType(str, typeof(T));
		}

		/// <summary>
		/// 字符串转换为数值列表
		/// </summary>
		/// <param name="separator">分隔符</param>
		public static List<T> StringToValueList<T>(string str, char separator)
		{
			List<T> result = new List<T>();
			if (!string.IsNullOrEmpty(str))
			{
				string[] splits = str.Split(separator);
				foreach (string split in splits)
				{
					if (!string.IsNullOrEmpty(split))
					{
						result.Add((T)Convert.ChangeType(split, typeof(T)));
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 字符串转为字符串列表
		/// </summary>
		public static List<string> StringToStringList(string str, char separator)
		{
			List<string> result = new List<string>();
			if (!string.IsNullOrEmpty(str))
			{
				string[] splits = str.Split(separator);
				foreach (string split in splits)
				{
					if (!string.IsNullOrEmpty(split))
					{
						result.Add(split);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 转换为枚举
		/// 枚举索引转换为枚举类型
		/// </summary>
		public static T IndexToEnum<T>(string index) where T : IConvertible
		{
			int enumIndex = (int)Convert.ChangeType(index, typeof(int));
			return IndexToEnum<T>(enumIndex);
		}

		/// <summary>
		/// 转换为枚举
		/// 枚举索引转换为枚举类型
		/// </summary>
		public static T IndexToEnum<T>(int index) where T : IConvertible
		{
			if (Enum.IsDefined(typeof(T), index) == false)
			{
				throw new ArgumentException($"Enum {typeof(T)} is not defined index {index}");
			}
			return (T)Enum.ToObject(typeof(T), index);
		}

		/// <summary>
		/// 转换为枚举
		/// 枚举名称转换为枚举类型
		/// </summary>
		public static T NameToEnum<T>(string name)
		{
			if (Enum.IsDefined(typeof(T), name) == false)
			{
				throw new ArgumentException($"Enum {typeof(T)} is not defined name {name}");
			}
			return (T)Enum.Parse(typeof(T), name);
		}

		/// <summary>
		/// 字符串转换为参数列表
		/// </summary>
		public static List<float> StringToParams(string str)
		{
			List<float> result = new List<float>();
			MatchCollection matches = s_Regex.Matches(str);
			for (int i = 0; i < matches.Count; i++)
			{
				string value = matches[i].Value.Trim('{', '}');
				result.Add(StringToValue<float>(value));
			}
			return result;
		}
	}
}