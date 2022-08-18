using System;
using System.Runtime.CompilerServices;
using Kits.DevlpKit.Supplements.Collections;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Structs
{
    /// <summary>
    /// NB stands for NB
    /// NativeBuffers are current designed to be used inside Jobs. They wrap an EntityDB array of components
    /// but do not track it. Hence it's meant to be used temporary and locally as the array can become invalid
    /// after a submission of entities.
    ///
    /// NB are wrappers of native arrays. Are not meant to resize or free
    ///
    /// NBs cannot have a count, because a count of the meaningful number of items is not tracked.
    /// Example: an MB could be initialized with a size 10 and count 0. Then the buffer is used to fill entities
    /// but the count will stay zero. It's not the MB responsibility to track the count
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct NativeBuffers<T>:IBuffer<T> where T:struct
    {
        static NativeBuffers()
        {
            if (TypeCache<T>.IsUnmanaged == false)
                throw new Exception("NativeBuffer (NB) supports only unmanaged types");
        }
        
        public NativeBuffers(IntPtr array, uint capacity) : this()
        {
            m_Ptr = array;
            m_Capacity = capacity;
        }

        public void CopyTo(uint sourceStartIndex, T[] destination, uint destinationStartIndex, uint size) { throw new NotImplementedException(); }
        public void Clear()
        {
            MemoryTool.MemClear(m_Ptr, (uint) (m_Capacity * MemoryTool.SizeOf<T>()));
        }

        public void FastClear()
        { }

        public T[] ToManagedArray()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr ToNativeArray(out int capacity)
        {
            capacity = (int) m_Capacity; return m_Ptr; 
        }

        public int capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int) m_Capacity;
        }

        public bool isValid => m_Ptr != IntPtr.Zero;

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
#if DEBUG && !PROFILE_SVELTO
                    if (index >= m_Capacity)
                        throw new Exception("NativeBuffer - out of bound access");
#endif
                    int size = MemoryTool.SizeOf<T>();
                    ref T asRef = ref Unsafe.AsRef<T>((void*) (m_Ptr + (int) (index * size)));
                    return ref asRef;
                }
            }
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
#if DEBUG && !PROFILE_SVELTO
                    if (index < 0 || index >= m_Capacity)
                        throw new Exception($"NativeBuffer - out of bound access: index {index} - capacity {capacity}");
#endif
                    int size = MemoryTool.SizeOf<T>();
                    ref T asRef = ref Unsafe.AsRef<T>((void*) (m_Ptr + (int) (index * size)));
                    return ref asRef;
                }
            }
        }

        readonly uint m_Capacity;
#if UNITY_COLLECTIONS
        //todo can I remove this from here? it should be used outside
        [Unity.Burst.NoAlias]
        [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction]
#endif
        readonly IntPtr m_Ptr; 

        public NativeBuffers<T> AsReader() { return this; }
        public NativeBuffers<T> AsWriter() { return this; }
    }
}
