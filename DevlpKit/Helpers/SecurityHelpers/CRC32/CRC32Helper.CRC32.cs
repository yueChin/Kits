namespace Kits.DevlpKit.Helpers.SecurityHelpers.CRC32
{
    /// <summary>
    /// 校验相关的实用函数。
    /// </summary>
    public static partial class CRC32Helper
    {
         /// <summary>
        /// CRC32 算法。
        /// </summary>
        private sealed class CRC32
        {
            private const int c_TableLength = 256;
            private const uint c_DefaultPolynomial = 0xedb88320;
            private const uint c_DefaultSeed = 0xffffffff;

            private readonly uint m_Seed;
            private readonly uint[] m_Table;
            private uint m_Hash;

            public CRC32() : this(c_DefaultPolynomial, c_DefaultSeed)
            {
            }

            public CRC32(uint polynomial, uint seed)
            {
                m_Seed = seed;
                m_Table = InitializeTable(polynomial);
                m_Hash = seed;
            }

            public void Initialize()
            {
                m_Hash = m_Seed;
            }

            public void HashCore(byte[] bytes, int offset, int length)
            {
                m_Hash = CalculateHash(m_Table, m_Hash, bytes, offset, length);
            }

            public uint HashFinal()
            {
                return ~m_Hash;
            }

            private static uint CalculateHash(uint[] table, uint value, byte[] bytes, int offset, int length)
            {
                int last = offset + length;
                for (int i = offset; i < last; i++)
                {
                    unchecked
                    {
                        value = (value >> 8) ^ table[bytes[i] ^ value & 0xff];
                    }
                }

                return value;
            }

            private static uint[] InitializeTable(uint polynomial)
            {
                uint[] table = new uint[c_TableLength];
                for (int i = 0; i < c_TableLength; i++)
                {
                    uint entry = (uint)i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                        {
                            entry = (entry >> 1) ^ polynomial;
                        }
                        else
                        {
                            entry >>= 1;
                        }
                    }

                    table[i] = entry;
                }

                return table;
            }
        }

    }
}