
using System;

namespace Kits.DevlpKit.Helpers.SecurityHelpers.BitMask32
{
    public struct BitMask32
    {
        private int m_Mask;

        public static implicit operator int(BitMask32 mask) { return mask.m_Mask; }
        public static implicit operator BitMask32(int mask) { return new BitMask32(mask); }

        public BitMask32(int mask)
        {
            m_Mask = mask;
        }

        /// <summary>
        /// 打开位
        /// </summary>
        public void Open(int bit)
        {
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException();
            else
                m_Mask |= 1 << bit;
        }

        /// <summary>
        /// 关闭位
        /// </summary>
        public void Close(int bit)
        {
            if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException();
			else
                m_Mask &= ~(1 << bit);
        }

        /// <summary>
        /// 位取反
        /// </summary>
        public void Reverse(int bit)
        {
            if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException();
			else
                m_Mask ^= 1 << bit;
        }

		/// <summary>
		/// 所有位取反
		/// </summary>
		public void Inverse()
		{
			m_Mask = ~m_Mask;
		}

		/// <summary>
		/// 比对位值
		/// </summary>
		public bool Test(int bit)
        {
            if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException();
			else
				return (m_Mask & (1 << bit)) != 0;
        }
    }
}