using System;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Collections.Dictionaries
{
    /// <summary>
    ///   original code: http://devplanet.com/blogs/brianr/archive/2008/09/29/thread-safe-dictionary-update.aspx
    ///   simplified (not an IDictionary) and apdated (uses NewFasterList)
    /// </summary>
    /// <typeparam name = "TKey"></typeparam>
    /// <typeparam name = "TValue"></typeparam>

    public sealed class ThreadSafeDictionary<TKey, TValue> where TKey : IEquatable<TKey>
    {
        public ThreadSafeDictionary(int size)
        {
            m_Dict = new FasterDictionary<TKey, TValue>((uint) size);
        }

        public ThreadSafeDictionary()
        {
            m_Dict = new FasterDictionary<TKey, TValue>();
        }

        // setup the lock;
        public uint Count
        {
            get
            {
                m_LockQ.EnterReadLock();
                try
                {
                    return (uint)m_Dict.count;
                }
                finally
                {
                    m_LockQ.ExitReadLock();
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                m_LockQ.EnterReadLock();
                try
                {
                    return m_Dict[key];
                }
                finally
                {
                    m_LockQ.ExitReadLock();
                }
            }

            set
            {
                m_LockQ.EnterWriteLock();
                try
                {
                    m_Dict[key] = value;
                }
                finally
                {
                    m_LockQ.ExitWriteLock();
                }
            }
        }

        public void Clear()
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_Dict.Clear();
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        public void Add(TKey key, TValue value)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_Dict.Add(key, value);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }
        
        public void Add(TKey key, ref TValue value)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_Dict.Add(key, value);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        public bool ContainsKey(TKey key)
        {
            m_LockQ.EnterReadLock();
            try
            {
                return m_Dict.ContainsKey(key);
            }
            finally
            {
                m_LockQ.ExitReadLock();
            }
        }

        public bool Remove(TKey key)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                return m_Dict.Remove(key);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            m_LockQ.EnterReadLock();
            try
            {
                return m_Dict.TryGetValue(key, out value);
            }
            finally
            {
                m_LockQ.ExitReadLock();
            }
        }

        /// <summary>
        ///   Merge does a blind remove, and then add.  Basically a blind Upsert.
        /// </summary>
        /// <param name = "key">Key to lookup</param>
        /// <param name = "newValue">New Value</param>
        public void MergeSafe(TKey key, TValue newValue)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                // take a writelock immediately since we will always be writing
                if (m_Dict.ContainsKey(key))
                    m_Dict.Remove(key);

                m_Dict.Add(key, newValue);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        /// <summary>
        ///   This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name = "key">Key to remove</param>
        public void RemoveSafe(TKey key)
        {
            m_LockQ.EnterReadLock();
            try
            {
                if (m_Dict.ContainsKey(key))
                    m_LockQ.EnterWriteLock();
                try
                {
                    m_Dict.Remove(key);
                }
                finally
                {
                    m_LockQ.ExitWriteLock();
                }
            }
            finally
            {
                m_LockQ.ExitReadLock();
            }
        }

        public void Update(TKey key, ref TValue value)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_Dict[key] = value;
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        public void Set(TKey key, TValue value)
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_Dict.Set(key, value);
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }
        
        public void CopyValuesTo(TValue[] tasks, uint index)
        {
            m_LockQ.EnterReadLock();
            try
            {
                m_Dict.CopyValuesTo(tasks, index);
            }
            finally
            {
                m_LockQ.ExitReadLock();
            }
        }

        public void CopyValuesTo(FasterList<TValue> values)
        {
            values.ExpandTo(m_Dict.count);
            CopyValuesTo(values.ToArrayFast(out _), 0);
        }

        public void FastClear()
        {
            m_LockQ.EnterWriteLock();
            try
            {
                m_Dict.FastClear();
            }
            finally
            {
                m_LockQ.ExitWriteLock();
            }
        }

        readonly FasterDictionary<TKey, TValue> m_Dict;
        readonly ReaderWriterLockSlimEx m_LockQ = ReaderWriterLockSlimEx.Create();
    }
}
