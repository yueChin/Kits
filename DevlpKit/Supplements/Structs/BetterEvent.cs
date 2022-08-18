using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Structs
{

    // Memory friendly multi-cast-delegate implementation
    // It could eliminate unnecessary GC-allocation for delegate cloning( add, remove )
    // Usage:
    // class Test {
    //    BetterEvent<int> callback;
    //    Test() {
    //        callback += Func;
    //        if ( callback ) {
    //            callback.Invoke( 0 );
    //        }
    //    }
    //    void Func( int a ) {
    //        // no problem at all
    //        callback -= Func;
    //    }
    //}

    public struct BetterEvent
    {

        List<Action> m_CalleeList;
        int m_Depth;
        int m_SparseIndex;

        public static implicit operator bool(BetterEvent exists)
        {
            return exists.m_CalleeList != null && exists.m_CalleeList.Count > 0;
        }

        public static BetterEvent operator +(BetterEvent lhs, Action rhs)
        {
            lhs.Slot += rhs;
            return lhs;
        }

        public static BetterEvent operator -(BetterEvent lhs, Action rhs)
        {
            lhs.Slot -= rhs;
            return lhs;
        }

        public event Action Slot
        {
            add
            {
                if (value != null)
                {
                    if (m_CalleeList == null)
                    {
                        m_CalleeList = new List<Action>(1);
                    }

                    m_CalleeList.Add(value);
                }
            }
            remove
            {
                if (value != null)
                {
                    List<Action> list = m_CalleeList;
                    if (list != null)
                    {
                        if (m_Depth != 0)
                        {
                            for (int i = 0, count = list.Count; i < count; ++i)
                            {
                                if (list[i] != null && list[i].Equals(value))
                                {
                                    list[i] = null;
                                    if (i <= m_SparseIndex)
                                    {
                                        m_SparseIndex = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            list.Remove(value);
                        }
                    }
                }
            }
        }

        public void Invoke()
        {
            List<Action> list = m_CalleeList;
            if (list != null)
            {
                try
                {
                    ++m_Depth;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (list[i] != null)
                        {
                            try
                            {
                                list[i]();
                            }
                            catch (Exception e)
                            {
                                throw new SystemException("", e);
                            }
                        }
                    }
                }
                finally
                {
                    --m_Depth;
                    if (m_SparseIndex > 0 && m_Depth == 0 && list.Count > 0)
                    {
                        int count = list.Count;
                        int removeCount = 0;
                        for (int i = m_SparseIndex - 1; i < count; ++i)
                        {
                            if (list[i] == null)
                            {
                                int newCount = i++;
                                for (; i < count; ++i)
                                {
                                    if (list[i] != null)
                                    {
                                        list[newCount++] = list[i];
                                    }
                                }

                                removeCount = count - newCount;
                                list.RemoveRange(newCount, removeCount);
                                break;
                            }
                        }

                        m_SparseIndex = 0;
                    }
                }
            }
        }
    }

    public struct BetterEvent<T>
    {

        List<Action<T>> m_CalleeList;
        int m_Depth;
        int m_SparseIndex;

        public static BetterEvent<T> operator +(BetterEvent<T> lhs, Action<T> rhs)
        {
            lhs.Slot += rhs;
            return lhs;
        }

        public static BetterEvent<T> operator -(BetterEvent<T> lhs, Action<T> rhs)
        {
            lhs.Slot -= rhs;
            return lhs;
        }

        public static implicit operator bool(BetterEvent<T> exists)
        {
            return exists.m_CalleeList != null && exists.m_CalleeList.Count > 0;
        }

        public event Action<T> Slot
        {
            add
            {
                if (value != null)
                {
                    if (m_CalleeList == null)
                    {
                        m_CalleeList = new List<Action<T>>(1);
                    }

                    m_CalleeList.Add(value);
                }
            }
            remove
            {
                if (value != null)
                {
                    List<Action<T>> list = m_CalleeList;
                    if (list != null)
                    {
                        if (m_Depth != 0)
                        {
                            for (int i = 0, count = list.Count; i < count; ++i)
                            {
                                if (list[i] != null && list[i].Equals(value))
                                {
                                    list[i] = null;
                                    if (i <= m_SparseIndex)
                                    {
                                        m_SparseIndex = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            list.Remove(value);
                        }
                    }
                }
            }
        }

        public void Invoke(T arg)
        {
            List<Action<T>> list = m_CalleeList;
            if (list != null)
            {
                try
                {
                    ++m_Depth;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (list[i] != null)
                        {
                            try
                            {
                                list[i](arg);
                            }
                            catch (Exception e)
                            {
                                throw new SystemException("", e);
                            }

                        }
                    }
                }
                finally
                {
                    --m_Depth;
                    if (m_SparseIndex > 0 && m_Depth == 0 && list.Count > 0)
                    {
                        int count = list.Count;
                        int removeCount = 0;
                        for (int i = m_SparseIndex - 1; i < count; ++i)
                        {
                            if (list[i] == null)
                            {
                                int newCount = i++;
                                for (; i < count; ++i)
                                {
                                    if (list[i] != null)
                                    {
                                        list[newCount++] = list[i];
                                    }
                                }

                                removeCount = count - newCount;
                                list.RemoveRange(newCount, removeCount);
                                break;
                            }
                        }

                        m_SparseIndex = 0;
                    }
                }
            }
        }
    }

    public struct BetterEvent<T1, T2>
    {

        List<Action<T1, T2>> m_CalleeList;
        int m_Depth;
        int m_SparseIndex;

        public static BetterEvent<T1, T2> operator +(BetterEvent<T1, T2> lhs, Action<T1, T2> rhs)
        {
            lhs.Slot += rhs;
            return lhs;
        }

        public static BetterEvent<T1, T2> operator -(BetterEvent<T1, T2> lhs, Action<T1, T2> rhs)
        {
            lhs.Slot -= rhs;
            return lhs;
        }

        public static implicit operator bool(BetterEvent<T1, T2> exists)
        {
            return exists.m_CalleeList != null && exists.m_CalleeList.Count > 0;
        }

        public event Action<T1, T2> Slot
        {
            add
            {
                if (value != null)
                {
                    if (m_CalleeList == null)
                    {
                        m_CalleeList = new List<Action<T1, T2>>(1);
                    }

                    m_CalleeList.Add(value);
                }
            }
            remove
            {
                if (value != null)
                {
                    List<Action<T1, T2>> list = m_CalleeList;
                    if (list != null)
                    {
                        if (m_Depth != 0)
                        {
                            for (int i = 0, count = list.Count; i < count; ++i)
                            {
                                if (list[i] != null && list[i].Equals(value))
                                {
                                    list[i] = null;
                                    if (i <= m_SparseIndex)
                                    {
                                        m_SparseIndex = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            list.Remove(value);
                        }
                    }
                }
            }
        }

        public void Invoke(T1 arg1, T2 arg2)
        {
            List<Action<T1, T2>> list = m_CalleeList;
            if (list != null)
            {
                try
                {
                    ++m_Depth;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (list[i] != null)
                        {
                            try
                            {
                                list[i](arg1, arg2);
                            }
                            catch (Exception e)
                            {
                                throw new SystemException("", e);
                            }
                        }
                    }
                }
                finally
                {
                    --m_Depth;
                    if (m_SparseIndex > 0 && m_Depth == 0 && list.Count > 0)
                    {
                        int count = list.Count;
                        int removeCount = 0;
                        for (int i = m_SparseIndex - 1; i < count; ++i)
                        {
                            if (list[i] == null)
                            {
                                int newCount = i++;
                                for (; i < count; ++i)
                                {
                                    if (list[i] != null)
                                    {
                                        list[newCount++] = list[i];
                                    }
                                }

                                removeCount = count - newCount;
                                list.RemoveRange(newCount, removeCount);
                                break;
                            }
                        }

                        m_SparseIndex = 0;
                    }
                }
            }
        }
    }

    public struct BetterEvent<T1, T2, T3>
    {

        List<Action<T1, T2, T3>> m_CalleeList;
        int m_Depth;
        int m_SparseIndex;

        public static BetterEvent<T1, T2, T3> operator +(BetterEvent<T1, T2, T3> lhs, Action<T1, T2, T3> rhs)
        {
            lhs.Slot += rhs;
            return lhs;
        }

        public static BetterEvent<T1, T2, T3> operator -(BetterEvent<T1, T2, T3> lhs, Action<T1, T2, T3> rhs)
        {
            lhs.Slot -= rhs;
            return lhs;
        }

        public static implicit operator bool(BetterEvent<T1, T2, T3> exists)
        {
            return exists.m_CalleeList != null && exists.m_CalleeList.Count > 0;
        }

        public event Action<T1, T2, T3> Slot
        {
            add
            {
                if (value != null)
                {
                    if (m_CalleeList == null)
                    {
                        m_CalleeList = new List<Action<T1, T2, T3>>(1);
                    }

                    m_CalleeList.Add(value);
                }
            }
            remove
            {
                if (value != null)
                {
                    List<Action<T1, T2, T3>> list = m_CalleeList;
                    if (list != null)
                    {
                        if (m_Depth != 0)
                        {
                            for (int i = 0, count = list.Count; i < count; ++i)
                            {
                                if (list[i] != null && list[i].Equals(value))
                                {
                                    list[i] = null;
                                    if (i <= m_SparseIndex)
                                    {
                                        m_SparseIndex = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            list.Remove(value);
                        }
                    }
                }
            }
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            List<Action<T1, T2, T3>> list = m_CalleeList;
            if (list != null)
            {
                try
                {
                    ++m_Depth;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (list[i] != null)
                        {
                            try
                            {
                                list[i](arg1, arg2, arg3);
                            }
                            catch (Exception e)
                            {
                                throw new SystemException("", e);
                            }
                        }
                    }
                }
                finally
                {
                    --m_Depth;
                    if (m_SparseIndex > 0 && m_Depth == 0 && list.Count > 0)
                    {
                        int count = list.Count;
                        int removeCount = 0;
                        for (int i = m_SparseIndex - 1; i < count; ++i)
                        {
                            if (list[i] == null)
                            {
                                int newCount = i++;
                                for (; i < count; ++i)
                                {
                                    if (list[i] != null)
                                    {
                                        list[newCount++] = list[i];
                                    }
                                }

                                removeCount = count - newCount;
                                list.RemoveRange(newCount, removeCount);
                                break;
                            }
                        }

                        m_SparseIndex = 0;
                    }
                }
            }
        }
    }

    public struct BetterEvent<T1, T2, T3, T4>
    {

        List<Action<T1, T2, T3, T4>> m_CalleeList;
        int m_Depth;
        int m_SparseIndex;

        public static BetterEvent<T1, T2, T3, T4> operator +(BetterEvent<T1, T2, T3, T4> lhs, Action<T1, T2, T3, T4> rhs)
        {
            lhs.Slot += rhs;
            return lhs;
        }

        public static BetterEvent<T1, T2, T3, T4> operator -(BetterEvent<T1, T2, T3, T4> lhs, Action<T1, T2, T3, T4> rhs)
        {
            lhs.Slot -= rhs;
            return lhs;
        }

        public static implicit operator bool(BetterEvent<T1, T2, T3, T4> exists)
        {
            return exists.m_CalleeList != null && exists.m_CalleeList.Count > 0;
        }

        public event Action<T1, T2, T3, T4> Slot
        {
            add
            {
                if (value != null)
                {
                    if (m_CalleeList == null)
                    {
                        m_CalleeList = new List<Action<T1, T2, T3, T4>>(1);
                    }

                    m_CalleeList.Add(value);
                }
            }
            remove
            {
                if (value != null)
                {
                    List<Action<T1, T2, T3, T4>> list = m_CalleeList;
                    if (list != null)
                    {
                        if (m_Depth != 0)
                        {
                            for (int i = 0, count = list.Count; i < count; ++i)
                            {
                                if (list[i] != null && list[i].Equals(value))
                                {
                                    list[i] = null;
                                    if (i <= m_SparseIndex)
                                    {
                                        m_SparseIndex = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            list.Remove(value);
                        }
                    }
                }
            }
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            List<Action<T1, T2, T3, T4>> list = m_CalleeList;
            if (list != null)
            {
                try
                {
                    ++m_Depth;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (list[i] != null)
                        {
                            try
                            {
                                list[i](arg1, arg2, arg3, arg4);
                            }
                            catch (Exception e)
                            {
                                throw new SystemException("", e);
                            }
                        }
                    }
                }
                finally
                {
                    --m_Depth;
                    if (m_SparseIndex > 0 && m_Depth == 0 && list.Count > 0)
                    {
                        int count = list.Count;
                        int removeCount = 0;
                        for (int i = m_SparseIndex - 1; i < count; ++i)
                        {
                            if (list[i] == null)
                            {
                                int newCount = i++;
                                for (; i < count; ++i)
                                {
                                    if (list[i] != null)
                                    {
                                        list[newCount++] = list[i];
                                    }
                                }

                                removeCount = count - newCount;
                                list.RemoveRange(newCount, removeCount);
                                break;
                            }
                        }

                        m_SparseIndex = 0;
                    }
                }
            }
        }
    }
}
//EOF
