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
	public struct ObfuscateFloat : IFormattable, IEquatable<ObfuscateFloat>, IComparable<ObfuscateFloat>, IComparable<float>, IComparable
	{
		private static int s_GlobalSeed = (int)DateTime.Now.Ticks;

		[SerializeField]
		private int m_Seed;
		[SerializeField]
		private int m_Data;

		public ObfuscateFloat(float value)
		{
			m_Seed = s_GlobalSeed++;
			m_Data = 0;
			Value = value;
		}
		internal float Value
		{
			get
			{
				int v = m_Data ^ m_Seed;
				return ConvertValue(v);
			}
			set
			{
				int v = ConvertValue(value);
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
			return obj is ObfuscateFloat && Equals((ObfuscateFloat)obj);
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

		public bool Equals(ObfuscateFloat obj)
		{
			return obj.Value.Equals(Value);
		}
		public int CompareTo(ObfuscateFloat other)
		{
			return Value.CompareTo(other.Value);
		}
		public int CompareTo(float other)
		{
			return Value.CompareTo(other);
		}
		public int CompareTo(object obj)
		{
			return Value.CompareTo(obj);
		}

		#region 运算符重载
		public static implicit operator float(ObfuscateFloat value)
		{
			return value.Value;
		}
		public static implicit operator ObfuscateFloat(float value)
		{
			return new ObfuscateFloat(value);
		}
		#endregion

		unsafe static int ConvertValue(float value)
		{
			float* ptr = &value;
			return *((int*)ptr);
		}
		unsafe static float ConvertValue(int value)
		{
			int* ptr = &value;
			return *((float*)ptr);
		}
	}
}