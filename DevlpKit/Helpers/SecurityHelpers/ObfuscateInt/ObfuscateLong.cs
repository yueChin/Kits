//--------------------------------------------------
// Motion Framework
// Copyright©2020-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

using System;
using UnityEngine;

namespace Kits.DevlpKit.Helpers.SecurityHelpers.ObfuscateInt
{
	[Serializable]
	public struct ObfuscateLong : IFormattable, IEquatable<ObfuscateLong>, IComparable<ObfuscateLong>, IComparable<long>, IComparable
	{
		private static long s_GlobalSeed = DateTime.Now.Ticks;

		[SerializeField]
		private long m_Seed;
		[SerializeField]
		private long m_Data;

		public ObfuscateLong(long value)
		{
			m_Seed = s_GlobalSeed++;
			m_Data = 0;
			Value = value;
		}
		internal long Value
		{
			get
			{
				long v = m_Data ^ m_Seed;
				return v;
			}
			set
			{
				m_Data = value ^ m_Seed;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		public override string ToString()
		{
			return Value.ToString();
		}
		public override bool Equals(object obj)
		{
			return obj is ObfuscateLong && Equals((ObfuscateLong)obj);
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}
		public string ToString(IFormatProvider provider)
		{
			return Value.ToString(provider);
		}
		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public bool Equals(ObfuscateLong obj)
		{
			return Value.Equals(obj.Value);
		}
		public int CompareTo(ObfuscateLong other)
		{
			return Value.CompareTo(other.Value);
		}
		public int CompareTo(long other)
		{
			return Value.CompareTo(other);
		}
		public int CompareTo(object obj)
		{
			return Value.CompareTo(obj);
		}

		#region 运算符重载
		public static implicit operator long(ObfuscateLong value)
		{
			return value.Value;
		}
		public static implicit operator ObfuscateLong(long value)
		{
			return new ObfuscateLong(value);
		}
		public static ObfuscateLong operator ++(ObfuscateLong value)
		{
			return value.Value + 1;
		}
		public static ObfuscateLong operator --(ObfuscateLong value)
		{
			return value.Value - 1;
		}
		#endregion
	}
}