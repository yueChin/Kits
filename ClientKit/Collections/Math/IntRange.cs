using UnityEngine;

namespace Kits.ClientKit.Collections.Math
{
	[System.Serializable]
	public struct IntRange
	{
		public int Min;
		public int Max;

		public IntRange(int min, int max)
		{
			Min = min;
			Max = max;
		}

		public static bool operator ==(IntRange a, IntRange b)
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

		public static bool operator !=(IntRange a, IntRange b)
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

		public static implicit operator string(IntRange obj)
		{
			return "(" + obj.Min + ", " + obj.Max + ")";
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

			return obj.GetType() == GetType() && this == ((IntRange)obj);
		}

		public override int GetHashCode()
		{
			return Min.GetHashCode() ^ Max.GetHashCode();
		}

		public int Get(float t)
		{
			return Mathf.RoundToInt(Mathf.Lerp(Min, Max, t));
		}

		public float GetT(float value)
		{
			return (value - Min) / (Max - Min);
		}

		public float GetClampedT(float value)
		{
			return Mathf.Clamp01(GetT(value));
		}

		public bool Contains(int value)
		{
			return Min <= value && value <= Max;
		}

		public bool Overlaps(IntRange other)
		{
			return (Min <= other.Min && other.Min <= Max) || (Min <= other.Max && other.Max <= Max);
		}

		public int GetRandomValue()
		{
			float t = Random.value;
			return Get(t);
		}

		public int GetRandomSignedValue()
		{
			int value = GetRandomValue();
			return Random.value > 0.5f ? value : -value;
		}
	}
}