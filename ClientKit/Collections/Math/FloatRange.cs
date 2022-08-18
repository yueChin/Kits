using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kits.ClientKit.Collections.Math
{
	[Serializable]
	public struct FloatRange
	{
		public float Min;
		public float Max;

		public FloatRange(float min, float max)
		{
			Min = min;
			Max = max;
		}

		public static bool operator ==(FloatRange a, FloatRange b)
		{
			// If both are null, or both are same instance, return true.
			if (ReferenceEquals(a, b))
			{
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}

			return a.Min == b.Min && a.Max == b.Max;
		}

		public static bool operator !=(FloatRange a, FloatRange b)
		{
			// If both are null, or both are same instance, return false.
			if (ReferenceEquals(a, b))
			{
				return false;
			}

			// If one is null, but not both, return true.
			if (((object)a == null) || ((object)b == null))
			{
				return true;
			}

			return a.Min != b.Min || a.Max != b.Max;
		}
		
		public override string ToString()
		{
			return "(" + Min + ", " + Max + ")";
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj.GetType() == GetType() && this == ((FloatRange)obj);
		}

		public override int GetHashCode()
		{
			return Min.GetHashCode() ^ Max.GetHashCode();
		}

		public float Get(float t)
		{
			return Mathf.Lerp(Min, Max, t);
		}

		public float GetT(float value)
		{
			return (value - Min) / (Max - Min);
		}

		public float GetClampedT(float value)
		{
			return Mathf.Clamp01(GetT(value));
		}

		public bool Contains(float value)
		{
			return Min <= value && value <= Max;
		}

		public bool Overlaps(FloatRange other)
		{
			return (Min <= other.Min && other.Min <= Max) || (Min <= other.Max && other.Max <= Max);
		}

		public float GetRandomValue()
		{
			float t = Random.value;
			return Get(t);
		}

		public float GetRandomSignedValue()
		{
			float value = GetRandomValue();
			return Random.value > 0.5f ? value : -value;
		}
	}
}