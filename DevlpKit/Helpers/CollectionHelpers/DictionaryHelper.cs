using System.Collections.Generic;
using UnityEngine;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{


    public static class DictionaryHelper
    {
        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static IDictionary<T, V> TryChangeDictPair<T, V>(this IDictionary<T, V> dict, T key, V value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static IDictionary<T, V> TryAdd<T, V>(this IDictionary<T, V> dict, T key, V value)
        {
            if (dict.ContainsKey(key) == false) 
                dict.Add(key, value);
            return dict;
        }
        
        public static void TryChangeDictSet<T, V>(this IDictionary<T, HashSet<V>> dict, T key, V value)
        {
            if (dict.ContainsKey(key))
            {
                HashSet<V> set = dict[key];
                set.Add(value);
            }
            else
            {
                HashSet<V> set = new HashSet<V>() {value};
                dict.Add(key, set);
            }
        }

        public static void TryChangeDictList<T, V>(this IDictionary<T, List<V>> dict, T key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                list.Add(value);
            }
            else
            {
                List<V> list = new List<V>() {value};
                dict.Add(key, list);
            }
        }

        public static void TryAddDictValueCnt<T>(this IDictionary<T, int> dict, T key, int cnt = 1)
        {
            if (dict.ContainsKey(key))
                dict[key] += cnt;
            else
                dict.Add(key, cnt);
        }

        public static void TryAddDictValueCnt<T>(this IDictionary<T, float> dict, T key, float cnt = 1)
        {
            if (dict.ContainsKey(key))
                dict[key] += cnt;
            else
                dict.Add(key, cnt);
        }

        public static void TryAddDict<T>(this IDictionary<T, int> dict, IDictionary<T, int> add)
        {
            foreach (KeyValuePair<T, int> pair in add)
            {
                dict.TryAddDictValueCnt(pair.Key, pair.Value);
            }
        }

        public static void TryAddDict<T>(this IDictionary<T, float> dict, IDictionary<T, float> add)
        {
            foreach (KeyValuePair<T, float> pair in add)
            {
                dict.TryAddDictValueCnt(pair.Key, pair.Value);
            }
        }

        public static int TryGetDictIntValue<T>(this IDictionary<T, int> dict, T key, int defaultValue = 0)
        {
            int value = 0;
            if (dict != null && dict.ContainsKey(key))
                value = dict[key];
            return value;
        }

        public static V TryGetDictValue<T, V>(this IDictionary<T, V> dict, T key, V defaultV = default(V))
        {
            V value = defaultV;
            if (dict != null && dict.ContainsKey(key))
                value = dict[key];
            return value;
        }

        public static V TryGetDictValue<T, V>(this SortedDictionary<T, V> dict, T key, V defaultV = default(V))
        {
            V value = defaultV;
            if (dict != null && dict.ContainsKey(key))
                value = dict[key];
            return value;
        }

        public static void TryRemoveDictKey<T, V>(this IDictionary<T, V> dict, T key)
        {
            if (dict.ContainsKey(key))
                dict.Remove(key);
            else
                Debug.LogError("不存在的删除 Key ：" + key);
        }

        public static void StrAddIntDict(this string str, IDictionary<int, int> dict, bool isCheckNull = false)
        {
            if (isCheckNull)
                if (string.IsNullOrEmpty(str) || dict == null)
                    return;
            string[] strs = str.Split('#');
            dict.TryChangeDictPair(int.Parse(strs[0]), int.Parse(strs[1]));
        }
    }
}