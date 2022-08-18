using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kits.DevlpKit.Supplements.Structs;
using Kits.DevlpKit.Tools;
using Console = Kits.DevlpKit.Tools.ConsoleLog.Console;

namespace Kits.DevlpKit.Supplements.Collections
{
    public class FasterList<T>
    {
        internal static readonly FasterList<T> DefaultEmptyList = new FasterList<T>();
        
        public int count => (int) m_Count;
        public int Count => (int) m_Count;
        public uint capacity => (uint) m_Buffer.Length;
        
        public static explicit operator FasterList<T>(T[] array)
        {
            return new FasterList<T>(array);
        }

        public FasterList()
        {
            m_Count = 0;

            m_Buffer = new T[0];
        }

        public FasterList(uint initialSize)
        {
            m_Count = 0;

            m_Buffer = new T[initialSize];
        }

        public FasterList(int initialSize):this((uint)initialSize)
        { }

        public FasterList(T[] collection)
        {
            m_Buffer = new T[collection.Length];

            Array.Copy(collection, m_Buffer, collection.Length);

            m_Count = (uint) collection.Length;
        }

        public FasterList(T[] collection, uint actualSize)
        {
            m_Buffer = new T[actualSize];
            Array.Copy(collection, m_Buffer, actualSize);

            m_Count = actualSize;
        }

        public FasterList(ICollection<T> collection)
        {
            m_Buffer = new T[collection.Count];

            collection.CopyTo(m_Buffer, 0);

            m_Count = (uint) collection.Count;
        }

        public FasterList(ICollection<T> collection, int extraSize)
        {
            m_Buffer = new T[(uint) collection.Count + (uint)extraSize];

            collection.CopyTo(m_Buffer, 0);
            
            m_Count = (uint) collection.Count;
        }
        
        public FasterList(in FasterList<T> source)
        {
            m_Buffer = new T[ source.count];

            source.CopyTo(m_Buffer, 0);

            m_Count = (uint) source.count;
        }
        
        public FasterList(in FasterReadOnlyList<T> source)
        {
            m_Buffer = new T[ source.count];

            source.CopyTo(m_Buffer, 0);

            m_Count = (uint) source.count;
        }
        
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ContractChecks.Require(index < m_Count && m_Count > 0, "out of bound index");
                return ref m_Buffer[(uint) index];
            }
        }

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ContractChecks.Require(index < m_Count, "out of bound index");
                return ref m_Buffer[index];
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterList<T> Add(in T item)
        {
            if (m_Count == m_Buffer.Length)
                AllocateMore();

            m_Buffer[m_Count++] = item;

            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddAt(uint location, in T item)
        {
            ExpandTo(location + 1);

            m_Buffer[location] = item;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterList<T> AddRange(in FasterList<T> items)
        {
            AddRange(items.m_Buffer, (uint) items.count);

            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterList<T> AddRange(in FasterReadOnlyList<T> items)
        {
            AddRange(items.List.m_Buffer, (uint) items.count);

            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(T[] items, uint count)
        {
            if (count == 0) return;

            if (m_Count + count > m_Buffer.Length)
                AllocateMore(m_Count + count);

            Array.Copy(items, 0, m_Buffer, m_Count, count);
            m_Count += count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(T[] items)
        {
            AddRange(items, (uint) items.Length);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            EqualityComparer<T> comp = EqualityComparer<T>.Default;

            for (uint index = 0; index < m_Count; index++)
                if (comp.Equals(m_Buffer[index], item))
                    return true;

            return false;
        }

        /// <summary>
        /// Careful, you could keep on holding references you don't want to hold to anymore
        /// Use Clear in case.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FastClear()
        {
#if DEBUG && !PROFILE_SVELTO
            if (TypeCache<T>.Type.IsClass)
                Console.LogWarning(
                    "Warning: objects held by this list won't be garbage collected. Use ResetToReuse or Clear " +
                    "to avoid this warning");
#endif
            m_Count = 0;
        }
        
        /// <summary>
        /// this is a dirtish trick to be able to use the index operator
        /// before adding the elements through the Add functions
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <param name="initialSize"></param>
        /// <returns></returns>
        public static FasterList<T> PreFill<TU>(uint initialSize) where TU: T, new()
        {
            FasterList<T> list = new FasterList<T>(initialSize);

             if (default(TU) == null)
            {
                for (int i = 0; i < initialSize; i++)
                    list.m_Buffer[(uint) (i)] = new TU();
            }

            return list;
        }

        public static FasterList<T> Fill<TU>(uint initialSize) where TU: T, new()
        {
            FasterList<T> list = PreFill<TU>(initialSize);

            list.m_Count = initialSize;

            return list;
        }
        
        public static FasterList<T> PreInit(uint initialSize)
        {
            FasterList<T> list = new FasterList<T>(initialSize);

            list.m_Count = initialSize;

            return list;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetToReuse()
        {
            m_Count = 0;
        }

        public bool ReuseOneSlot<TU>(out TU result) where TU:T 
        {
            if (m_Count >= m_Buffer.Length)
            {
                result = default(TU);
                
                return false;
            }

            if (default(TU) == null)
            {
                result = (TU) m_Buffer[m_Count];

                if (result != null)
                {
                    m_Count++;
                    return true;
                }
                return false;
            }

            m_Count++;
            result = default(TU);
            return true;
        }
        
        public bool ReuseOneSlot<TU>() where TU: T
        {
            if (m_Count >= m_Buffer.Length)
                return false;

            m_Count++;

            return true;
        }
        
        public bool ReuseOneSlot()
        {
            if (m_Count >= m_Buffer.Length)
                return false;

            m_Count++;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(m_Buffer, 0, m_Buffer.Length);

            m_Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterListEnumerator<T> GetEnumerator()
        {
            return new FasterListEnumerator<T>(m_Buffer, (uint) count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, in T item)
        {
            ContractChecks.Require(index <= m_Count, "out of bound index");

            if (m_Count == m_Buffer.Length) AllocateMore();

            Array.Copy(m_Buffer, index, m_Buffer, index + 1, m_Count - index);
            ++m_Count;
            
            m_Buffer[index] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            ContractChecks.Require(index < m_Count, "out of bound index");

            if (index == --m_Count)
                return;

            Array.Copy(m_Buffer, index + 1, m_Buffer, index, m_Count - index);

            m_Buffer[m_Count] = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(uint newSize)
        {
            if (newSize == m_Buffer.Length) return;
            
            Array.Resize(ref m_Buffer, (int) newSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
        {
            T[] destinationArray = new T[m_Count];

            Array.Copy(m_Buffer, 0, destinationArray, 0, m_Count);

            return destinationArray;
        }

        /// <summary>
        /// This function exists to allow fast iterations. The size of the array returned cannot be
        /// used. The list count must be used instead.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArrayFast(out uint count)
        {
            count = m_Count;

            return m_Buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool UnorderedRemoveAt(int index)
        {
            ContractChecks.Require(index < m_Count && m_Count > 0, "out of bound index");

            if (index == --m_Count)
            {
                m_Buffer[m_Count] = default;
                return false;
            }

            m_Buffer[(uint) index] = m_Buffer[m_Count];
            m_Buffer[m_Count] = default;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Trim()
        {
            if (m_Count < m_Buffer.Length)
                Resize(m_Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimCount(uint newCount)
        {
            ContractChecks.Require(m_Count >= newCount, "the new length must be less than the current one");

            m_Count = newCount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExpandBy(uint increment)
        {
            uint count = m_Count + increment;

            if (m_Buffer.Length < count)
                AllocateMore(count);

            m_Count = count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExpandTo(uint newSize)
        {
            if (m_Buffer.Length < newSize)
                AllocateMore(newSize);

            if (m_Count < newSize)
                m_Count = newSize;
        }

        public void EnsureCapacity(uint newSize)
        {
            if (m_Buffer.Length < newSize)
                AllocateMore(newSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Push(in T item)
        {
            AddAt(m_Count, item);

            return m_Count - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T Pop() 
        { 
            --m_Count;
            return ref m_Buffer[m_Count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T Peek()
        {
            return ref m_Buffer[m_Count - 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(m_Buffer, 0, array, arrayIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AllocateMore()
        {
            int newLength = (int) ((m_Buffer.Length + 1) * 1.5f);
            T[] newList = new T[newLength];
            if (m_Count > 0) Array.Copy(m_Buffer, newList, m_Count);
            m_Buffer = newList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AllocateMore(uint newSize)
        {
            ContractChecks.Require(newSize > m_Buffer.Length);
            int newLength = (int) (newSize * 1.5f);

            T[] newList = new T[newLength];
            if (m_Count > 0) Array.Copy(m_Buffer, newList, m_Count);
            m_Buffer = newList;
        }

        T[]         m_Buffer;
        uint        m_Count;

        public static class NoVirt
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Count(FasterList<T> fasterList)
            {
                return fasterList.m_Count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T[] ToArrayFast(FasterList<T> fasterList, out uint count)
            {
                count = fasterList.m_Count;

                return fasterList.m_Buffer;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T[] ToArrayFast(FasterList<T> fasterList)
            {
                return fasterList.m_Buffer;
            }
        }
    }
}