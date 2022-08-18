using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static class HashSetHelper
    {
        public static void TryAddSet<T>(this ISet<T> set, ICollection<T> list)
        {
            foreach (T t in list)
            {
                set.TryAddSet(t);
            }
        }

        public static void TryAddSet<T>(this ISet<T> set, T value)
        {
            if (!set.Contains(value))
                set.Add(value);
        }

        public static void TryRemoveSet<T>(this ISet<T> set, ICollection<T> setIn)
        {
            foreach (T t in setIn)
            {
                set.TryRemoveSet(t);
            }
        }

        public static void TryRemoveSet<T>(this ISet<T> set, T value)
        {
            if (!set.Contains(value))
                set.Remove(value);
        }

        public static T TryGetValueByIndex<T>(this ISet<T> set, int index)
        {
            if (index < 0 || index > set.Count)
            {
                return default(T);
            }
            else
            {
                int i = 0;
                T outT = default(T);
                foreach (T t in set)
                {
                    if (i == index)
                    {
                        outT = t;
                        break;
                    }

                    i++;
                }

                return outT;
            }
        }
    }
}