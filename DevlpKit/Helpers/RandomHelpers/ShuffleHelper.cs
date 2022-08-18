using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.RandomHelpers
{
    public static class ShuffleUtility
    {
        /// <summary>
        /// 长度未知可用 n^2 复杂度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void FisherYatesShuffle<T>(IList<T> list)
        {
            List<T> cache = new List<T>();
            int currentIndex;
            while (list.Count > 0)
            {
                currentIndex = UnityEngine.Random.Range(0, list.Count);
                cache.Add(list[currentIndex]);
                list.RemoveAt(currentIndex);
            }
            for (int i = 0; i < cache.Count; i++)
            {
                list.Add(cache[i]);
            }
        }

        /// <summary>
        /// 需要长度已知  复杂度 n
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void KnuthDurstenfeldShuffle<T>(List<T> list)
        {
            //随机交换
            int currentIndex;
            T tempValue;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                currentIndex = UnityEngine.Random.Range(0, i + 1);
                tempValue = list[currentIndex];
                list[currentIndex] = list[i];
                list[i] = tempValue;
            }
        }

        public static void ExchangeShuffle(int[] arr)
        {
            System.Random random = new System.Random();
            for (int i = 0, len = arr.Length; i < arr.Length; i++)
            {
                int randomIndex = random.Next(0, i + 1);
                int value = arr[i];
                arr[i] = arr[randomIndex];
                arr[randomIndex] = value;
            }
        }

        public static T[] InsideOutAlgorithm<T>(T[] array)
        {
            T[] cache = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                cache[i] = array[i];
            }
            //随机交换
            T tempValue;
            int curtIdx;
            for (int i = cache.Length - 1; i >= 0; i--)
            {
                curtIdx = UnityEngine.Random.Range(0, i + 1);
                tempValue = cache[curtIdx];
                cache[curtIdx] = cache[i];
                cache[i] = tempValue;
            }

            return cache;
        }

        public static List<T> ReservoirSampling<T>(List<T> list, int m)
        {
            List<T> cache = new List<T>(m);
            for (int i = 0; i < m; i++)
            {
                cache.Add(list[i]);
            }

            for (int i = m; i < list.Count; i++)
            {
                int curtIdx = UnityEngine.Random.Range(0, i + 1);
                if (curtIdx < m)
                {
                    cache[curtIdx] = list[i];
                }
            }
            return cache;
        }
    }
}
