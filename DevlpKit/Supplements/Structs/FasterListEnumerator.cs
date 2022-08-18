namespace Kits.DevlpKit.Supplements.Structs
{
    public struct FasterListEnumerator<T>
    {
        public T Current =>
            m_Buffer[(uint) m_Counter - 1];

        public FasterListEnumerator(in T[] buffer, uint size)
        {
            m_Size = size;
            m_Counter = 0;
            m_Buffer = buffer;
        }

        public bool MoveNext()
        {
            if (m_Counter < m_Size)
            {
                m_Counter++;

                return true;
            }

            return false;
        }

        public void Reset()
        {
            m_Counter = 0;
        }

        readonly T[]  m_Buffer;
        int           m_Counter;
        readonly uint m_Size;
    }
}