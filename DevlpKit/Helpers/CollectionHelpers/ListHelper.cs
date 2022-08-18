using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static partial class ListHelper
    {
        public static T TryGetListValue<T>(this IList<T> list, int idx, T defaultT = default(T))
        {
            T value = defaultT;
            if (list != null && list.Count > idx)
            {
                value = list[idx];
            }

            return value;
        }

        public static IList<T> TryAddList<T>(this IList<T> list, T t)
        {
            if (list == null)
                list = new List<T>();
            if (t == null)
                return list;
            list.Add(t);
            return list;
        }

        public static IList<T> TryAddList<T>(this IList<T> list, IList<T> tList)
        {
            if (list == null)
                list = new List<T>();
            if (tList == null)
                return list;
            foreach (T t in tList)
            {
                list.Add(t);
            }

            return list;
        }

        public static IList<T> TryAddClearList<T>(this IList<T> list, IList<T> tList)
        {
            if (list == null)
                list = new List<T>();
            if (tList == null)
                return list;
            list.Clear();
            foreach (T t in tList)
            {
                list.Add(t);
            }

            return list;
        }

        public static IList<T> TryAddClearList<T, TV>(this IList<T> list, IList<TV> tList, Func<TV, T> addAction)
        {
            if (list == null)
                list = new List<T>();
            if (tList == null)
                return list;
            list.Clear();
            foreach (TV t in tList)
            {
                list.Add(addAction(t));
            }

            return list;
        }

        public static bool IsListEqual<T>(this IList<T> list1, IList<T> list2, Func<T, T, bool> boolFunc)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    bool isEqual = boolFunc.Invoke(list1[i], list2[i]);
                    if (!isEqual)
                    {
                        return false;
                    }
                }

                return true;
            }
        }


        public static bool IsListEqualCompare<T, TV>(this IList<T> list1, IList<TV> list2, Func<T, TV, bool> boolFunc)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    bool isEqual = boolFunc.Invoke(list1[i], list2[i]);
                    if (!isEqual)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static bool IsListAllNull<T>(this IList<T> list) where T : class
        {
            if (list == null)
                return false;
            foreach (T t in list)
            {
                if (t != null)
                    return false;
            }

            return true;
        }

        public static T Pop<T>(this List<T> list)
        {

            int index = list.Count - 1;

            T r = list[index];
            list.RemoveAt(index);
            return r;
        }

        public static bool SequenceEqualFast(this List<bool> self, List<bool> value)
        {
            if (self.Count != value.Count) return false;
            for (int i = 0; i < self.Count; ++i)
            {
                if (self[i] != value[i]) return false;
            }

            return true;
        }

        public static int CountFast(this List<bool> self)
        {
            int count = 0;
            for (int i = 0; i < self.Count; ++i)
            {
                if (self[i]) count++;
            }

            return count;
        }
    }
}