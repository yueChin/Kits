
using System;

namespace Kits.DevlpKit.Helpers.SecurityHelpers.BitMask32
{
    public struct BitMask64
    {
        private long m_Mask;

        public static implicit operator long(BitMask64 mask) { return mask.m_Mask; }
        public static implicit operator BitMask64(long mask) { return new BitMask64(mask); }
        
        public BitMask64(long mask)
        {
            m_Mask = mask;
        }

        /// <summary>
        /// 打开位
        /// </summary>
        public void Open(int bit)
        {
			if (bit < 0 || bit > 63)
				throw new ArgumentOutOfRangeException();
            else
                m_Mask |= 1L << bit;
        }

        /// <summary>
        /// 关闭位
        /// </summary>
        public void Close(int bit)
        {
            if (bit < 0 || bit > 63)
				throw new ArgumentOutOfRangeException();
			else
                m_Mask &= ~(1L << bit);
        }

        /// <summary>
        /// 位取反
        /// </summary>
        public void Reverse(int bit)
        {
            if (bit < 0 || bit > 63)
				throw new ArgumentOutOfRangeException();
			else
                m_Mask ^= 1L << bit;
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
            if (bit < 0 || bit > 63)
				throw new ArgumentOutOfRangeException();
			else
				return (m_Mask & (1L << bit)) != 0;
        }
    }
}