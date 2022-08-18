using System;
using System.Runtime.CompilerServices;
using Kits.DevlpKit.Supplements.Structs;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Collections.Dictionaries
{
    public struct ManagedStrategy<T> : IBufferStrategy<T>
    {
        IBuffer<T> m_Buffer;
        ManagedBuffer<T> m_RealBuffer;

        public ManagedStrategy(uint size):this()
        {
            Alloc(size, Allocator.None);
        }

        public bool isValid => m_Buffer != null;

        public void Alloc(uint size, Allocator nativeAllocator)
        {
            ManagedBuffer<T> b = new ManagedBuffer<T>();
            b.Set(new T[size]);
            this.m_RealBuffer = b;
            m_Buffer = null;
        }

        public void Resize(uint newCapacity, bool copyContent = true)
        {
            ContractChecks.Require(newCapacity > 0, "Resize requires a size greater than 0");
            
            T[] realBuffer = m_RealBuffer.ToManagedArray();
            if (copyContent == true)
                Array.Resize(ref realBuffer, (int) newCapacity);
            else
            {
                realBuffer = new T[newCapacity];
            }

            ManagedBuffer<T> b = new ManagedBuffer<T>();
            b.Set(realBuffer);
            this.m_RealBuffer = b;
            m_Buffer = null;
        }

        public int capacity => m_RealBuffer.capacity;

        public void Clear() => m_RealBuffer.Clear();
        public void FastClear() => m_RealBuffer.FastClear();

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref m_RealBuffer[index];
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref m_RealBuffer[index];
        }

        public IBuffer<T> ToBuffer()
        {
            if (m_Buffer == null)
                m_Buffer = m_RealBuffer;
            
            return m_Buffer;
        }

        public Allocator allocationStrategy => Allocator.Managed;

        public void       Dispose() {  }

        public ManagedBuffer<T> ToRealBuffer()
        {
            return m_RealBuffer;
        }
    }
}