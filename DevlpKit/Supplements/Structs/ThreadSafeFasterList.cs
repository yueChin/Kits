using System;
using System.Runtime.CompilerServices;
using Kits.DevlpKit.Supplements.Collections;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Structs
{
    public class ThreadSafeFasterList<T> 
    {
        public ThreadSafeFasterList(FasterList<T> list)
        {
            m_List = list ?? throw new ArgumentException("invalid list");
            m_LockQ = ReaderWriterLockSlimEx.Create();
        }

        public ThreadSafeFasterList()
        {
            m_List  = new FasterList<T>();
            m_LockQ = ReaderWriterLockSlimEx.Create();
        }

        public int count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                m_LockQ.EnterReadLock();
                try
                {
                    return m_List.count;
                }
                finally
                {
                    m_LockQ.ExitReadLock();
                }
            }
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                m_LockQ.EnterWriteLock();
                try
                {
                    return ref m_List[index];
                }
                finally
                {
                    m_LockQ.ExitWriteLock();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterListEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.Add(item);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(uint location, T item)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.AddAt(location, item);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.Clear();
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FastClear()
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.FastClear();
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.Insert(index, item);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.RemoveAt(index);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnorderedRemoveAt(int index)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_List.UnorderedRemoveAt(index);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArrayFast(out uint count)
        {
            m_LockQ.EnterReadLock();
            try
            {
                return m_List.ToArrayFast(out count);
            }
            finally
            {
                m_LockQ.ExitReadLock();
            }
        }

        readonly FasterList<T> m_List;

        readonly ReaderWriterLockSlimEx m_LockQ;
    }
}