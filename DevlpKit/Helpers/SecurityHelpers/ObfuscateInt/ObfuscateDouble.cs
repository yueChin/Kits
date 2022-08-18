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
	public struct ObfuscateDouble : IFormattable, IEquatable<ObfuscateDouble>, IComparable<ObfuscateDouble>, IComparable<double>, IComparable
	{
		private static long s_GlobalSeed = DateTime.Now.Ticks;

		[SerializeField]
		private long m_Seed;
		[SerializeField]
		private long m_Data;

		public ObfuscateDouble(double value)
		{
			m_Seed = s_GlobalSeed++;
			m_Data = 0;
			Value = value;
		}
		internal double Value
		{
			get
			{
				long v = m_Data ^ m_Seed;
				return ConvertValue(v);
			}
			set
			{
				long v = ConvertValue(value);
				m_Data = v ^ m_Seed;
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
			return obj is ObfuscateDouble && Equals((ObfuscateDouble)obj);
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

		public bool Equals(ObfuscateDouble obj)
		{
			return obj.Value.Equals(Value);
		}
		public int CompareTo(ObfuscateDouble other)
		{
			return Value.CompareTo(other.Value);
		}
		public int CompareTo(double other)
		{
			return Value.CompareTo(other);
		}
		public int CompareTo(object obj)
		{
			return Value.CompareTo(obj);
		}

		#region 运算符重载
		public static implicit operator double(ObfuscateDouble value)
		{
			return value.Value;
		}
		public static implicit operator ObfuscateDouble(double value)
		{
			return new ObfuscateDouble(value);
		}
		public static explicit operator ObfuscateDouble(ObfuscateFloat value)
		{
			return (float)value;
		}
		#endregion

		unsafe static long ConvertValue(double value)
		{
			double* ptr = &value;
			return *((long*)ptr);
		}
		unsafe static double ConvertValue(long value)
		{
			long* ptr = &value;
			return *((double*)ptr);
		}
	}
}