using System;
using System.Runtime.CompilerServices;

namespace Kits.DevlpKit.Supplements.Structs
{
    /// <summary>
    /// MB stands for ManagedBuffer
    ///
    /// MB are wrappers of arrays. Are not meant to resize or free
    /// MBs cannot have a count, because a count of the meaningful number of items is not tracked.
    /// Example: an MB could be initialized with a size 10 and count 0. Then the buffer is used to fill entities
    /// but the count will stay zero. It's not the MB responsibility to track the count
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ManagedBuffer<T>:IBuffer<T> 
    {
        public ManagedBuffer(T[]  array) : this()
        {
            m_Buffer = array;
        }
        
        public void Set(T[] array)
        {
            m_Buffer = array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(uint sourceStartIndex, T[] destination, uint destinationStartIndex, uint size)
        {
            Array.Copy(m_Buffer, sourceStartIndex, destination, destinationStartIndex, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(m_Buffer, (int) 0, (int) m_Buffer.Length);
        }
        
        public void FastClear() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToManagedArray()
        {
            return m_Buffer;
        }

        public IntPtr ToNativeArray(out int capacity)
        {
            throw new NotImplementedException();
        }

        public int capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_Buffer.Length;
        }
        
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref m_Buffer[index];
        }
        
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref m_Buffer[index];
        }

        T[] m_Buffer;
    }
}