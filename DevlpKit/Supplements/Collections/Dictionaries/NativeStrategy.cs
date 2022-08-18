using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Kits.DevlpKit.Supplements.Structs;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Collections.Dictionaries
{
    public struct NativeStrategy<T> : IBufferStrategy<T> where T : struct
    {
#if DEBUG && !PROFILE_SVELTO            
        static NativeStrategy()
        {
            if (TypeCache<T>.IsUnmanaged == false)
                throw new PreconditionException("Only unmanaged data can be stored natively");
        }
#endif        

        public void Alloc(uint newCapacity, Allocator nativeAllocator)
        {
#if DEBUG && !PROFILE_SVELTO            
            if (!(this.m_RealBuffer.ToNativeArray(out _) == IntPtr.Zero))
                throw new PreconditionException("can't alloc an already allocated buffer");
#endif            
            m_NativeAllocator = nativeAllocator;

            IntPtr realBuffer = MemoryTool.Alloc((uint) (newCapacity * MemoryTool.SizeOf<T>()), m_NativeAllocator);
            NativeBuffers<T> b = new NativeBuffers<T>(realBuffer, newCapacity);
            m_Buffer          = default;
            m_RealBuffer = b;
        }

        public bool isValid => m_RealBuffer.isValid;

        public NativeStrategy(uint size, Allocator nativeAllocator) : this()
        {
            Alloc(size, nativeAllocator);
        }

        public void Resize(uint newCapacity, bool copyContent = true)
        {
#if DEBUG && !PROFILE_SVELTO            
            if (!(newCapacity > 0))
                throw new PreconditionException("Resize requires a size greater than 0");
            if (!(newCapacity > capacity))
                throw new PreconditionException("can't resize to a smaller size");
#endif            
            IntPtr pointer = m_RealBuffer.ToNativeArray(out _);
            int sizeOf  = MemoryTool.SizeOf<T>();
            pointer = MemoryTool.Realloc(pointer, (uint) (capacity * sizeOf), (uint) (newCapacity * sizeOf)
                                            , m_NativeAllocator, copyContent);
            NativeBuffers<T> b = new NativeBuffers<T>(pointer, newCapacity);
            m_RealBuffer    = b;
            m_InvalidHandle = true;
        }

        public void Clear()     => m_RealBuffer.Clear();
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
            //To use this struct in Burst it cannot hold interfaces. This weird looking code is to
            //be able to store _realBuffer as a c# reference.
            if (m_InvalidHandle == true && ((IntPtr)m_Buffer != IntPtr.Zero))
            {
                m_Buffer.Free();
                m_Buffer = default;
            }
            m_InvalidHandle = false;
            if (((IntPtr)m_Buffer == IntPtr.Zero))
            {
                m_Buffer = GCHandle.Alloc(m_RealBuffer, GCHandleType.Normal);
            }

            return (IBuffer<T>) m_Buffer.Target;
        }

        public NativeBuffers<T> ToRealBuffer() { return m_RealBuffer; }

        public int       capacity           => m_RealBuffer.capacity;
        public Allocator allocationStrategy => m_NativeAllocator;

        public void Dispose()
        {
            if ((IntPtr)m_Buffer != IntPtr.Zero)
                m_Buffer.Free();
            
            if (m_RealBuffer.ToNativeArray(out _) != IntPtr.Zero)
            {
                MemoryTool.Free(m_RealBuffer.ToNativeArray(out _), Allocator.Persistent);
            }
            else
                throw new Exception("trying to dispose disposed buffer");

            m_Buffer     = default;
            m_RealBuffer = default;
        }
        
        Allocator m_NativeAllocator;
        NativeBuffers<T>     m_RealBuffer;
#if UNITY_COLLECTIONS
        [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction]
#endif
        GCHandle m_Buffer;
        bool m_InvalidHandle;
    }
}