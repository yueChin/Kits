using System.Runtime.CompilerServices;
using Kits.DevlpKit.Supplements.Collections;

namespace Kits.DevlpKit.Supplements.Structs
{
    public readonly struct FasterReadOnlyList<T> 
    {
        public static FasterReadOnlyList<T> DefaultEmptyList = new FasterReadOnlyList<T>(FasterList<T>.DefaultEmptyList);

        public int count      => List.count;
        public uint capacity => List.capacity;

        public FasterReadOnlyList(FasterList<T> list)
        {
            List = list;
        }
        
        public static implicit operator FasterReadOnlyList<T>(FasterList<T> list)
        {
            return new FasterReadOnlyList<T>(list);
        }
        
        public static implicit operator LocalFasterReadOnlyList<T>(FasterReadOnlyList<T> list)
        {
            return new LocalFasterReadOnlyList<T>(list.List);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterListEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }
        
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref List[index];
        }

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref List[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        internal readonly FasterList<T> List;
    }
    
    public readonly ref struct LocalFasterReadOnlyList<T> 
    {
        public int count      => m_List.count;
        public uint capacity   => m_List.capacity;

        public LocalFasterReadOnlyList(FasterList<T> list)
        {
            m_List = list;
        }
        
        public static implicit operator LocalFasterReadOnlyList<T>(FasterList<T> list)
        {
            return new LocalFasterReadOnlyList<T>(list);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterListEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }
        
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref m_List[index];
        }

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref m_List[index];
        }

        readonly FasterList<T> m_List;
    }
}