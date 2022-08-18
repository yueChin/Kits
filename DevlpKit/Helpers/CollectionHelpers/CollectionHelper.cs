using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static class CollectionHelper
    {
        public static void StrListAddIntDict(this IList<string> strList, IDictionary<int, int> dict)
        {
            if (dict == null || strList == null)
                return;
            foreach (string s in strList)
            {
                s.StrAddIntDict(dict);
            }
        }

        public static T TryGetValueByIndex<T>(this ICollection<T> collection, int idx)
        {
            T[] array = new T[collection.Count];
            collection.CopyTo(array, 0);
            if (collection.Count > idx)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (i == idx)
                    {
                        return array[i];
                    }
                }
            }

            return default(T);
        }
    }
}