using System;
using System.Collections.Generic;
using System.Linq;
using Random = Kits.DevlpKit.Supplements.Structs.Random;

namespace Kits.DevlpKit.Helpers.RandomHelpers
{
	public static class RandomHelper
	{
		private static System.Random m_Random = new System.Random();

		private static byte[] byte8 = new byte[8]; 

		public static UInt64 RandUInt64()
		{
			m_Random.NextBytes(byte8);
			return BitConverter.ToUInt64(byte8, 0);
		}

		public static Int64 RandInt64()
		{
			m_Random.NextBytes(byte8);
			return BitConverter.ToInt64(byte8, 0);
		}

        public static int[] RandomIntArray(int MaxValue, int Number)
        {
            int[] itemIds = new int[Number];

            int[] temp = new int[MaxValue];
            for (int i = 0; i < MaxValue; i++)
                temp[i] = i;

            for (int i = 0; i < Number; i++)
            {
                int r = UnityEngine.Random.Range(0, MaxValue);
                itemIds[i] = temp[r];
                for (int j = r; j < MaxValue - 1; j++)
                {
                    temp[j] = temp[j + 1];
                }
                MaxValue--;
            }

            return itemIds;
        }

        public static bool NextBool()
        {
            return m_Random.NextDouble() > 0.5;
        }
        
        public static byte[] NextBytes( int length)
        {
            byte[] data = new byte[length];
            m_Random.NextBytes(data);
            return data;
        }

        public static UInt16 NextUInt16()
        {
            return BitConverter.ToUInt16(NextBytes(2), 0);
        }
        public static Int16 NextInt16()
        {
            return BitConverter.ToInt16(NextBytes(2), 0);
        }
        public static float NextFloat()
        {
            return BitConverter.ToSingle(NextBytes(4), 0);
        }
        
        /// <summary>
        /// 设置随机数种子。
        /// </summary>
        /// <param name="seed">随机数种子。</param>
        public static void SetSeed(int seed)
        {
            m_Random = new System.Random(seed);
        }

        /// <summary>
        /// 返回非负随机数。
        /// </summary>
        /// <returns>大于等于零且小于 System.Int32.MaxValue 的 32 位带符号整数。</returns>
        public static int GetRandom()
        {
            return m_Random.Next();
        }

        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数。
        /// </summary>
        /// <param name="maxValue">要生成的随机数的上界（随机数不能取该上界值）。maxValue 必须大于等于零。</param>
        /// <returns>大于等于零且小于 maxValue 的 32 位带符号整数，即：返回值的范围通常包括零但不包括 maxValue。不过，如果 maxValue 等于零，则返回 maxValue。</returns>
        public static int GetRandom(int maxValue)
        {
            return m_Random.Next(maxValue);
        }

        /// <summary>
        /// 返回一个指定范围内的随机数。
        /// </summary>
        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）。</param>
        /// <param name="maxValue">返回的随机数的上界（随机数不能取该上界值）。maxValue 必须大于等于 minValue。</param>
        /// <returns>一个大于等于 minValue 且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括 minValue 但不包括 maxValue。如果 minValue 等于 maxValue，则返回 minValue。</returns>
        public static int GetRandom(int minValue, int maxValue)
        {
            return m_Random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数。
        /// </summary>
        /// <returns>大于等于 0.0 并且小于 1.0 的双精度浮点数。</returns>
        public static double GetRandomDouble()
        {
            return m_Random.NextDouble();
        }

        /// <summary>
        /// 用随机数填充指定字节数组的元素。
        /// </summary>
        /// <param name="buffer">包含随机数的字节数组。</param>
        public static void GetRandomBytes(byte[] buffer)
        {
            m_Random.NextBytes(buffer);
        }
        
                /// <summary>
        /// 从一组元素中选择一个. 如果所有元素被选中概率之和小于 1, 那么最后一个元素被选中概率相应增加
        /// </summary>
        /// <param name="getProbability"> 每个元素被选中的概率 </param>
        /// <param name="startIndex"> 开始遍历的索引 </param>
        /// <param name="count"> 遍历元素的数量 </param>
        /// <returns> 被选中的元素索引 </returns>
        public static int Choose(this ref Random random, Func<int, float> getProbability, int startIndex, int count)
        {
            int lastIndex = startIndex + count - 1;
            float rest = (float)random.Next01();
            float current;

            for (; startIndex < lastIndex; startIndex++)
            {
                current = getProbability(startIndex);
                if (rest < current) return startIndex;
                else rest -= current;
            }

            return lastIndex;
        }


        /// <summary>
        /// 从一组元素中选择一个. 如果所有元素被选中概率之和小于 1, 那么最后一个元素被选中概率相应增加
        /// </summary>
        /// <param name="probabilities"> 每个元素被选中的概率 </param>
        /// <param name="startIndex"> 开始遍历的索引 </param>
        /// <param name="count"> 遍历元素的数量. 如果这个值无效, 自动遍历到列表尾部 </param>
        /// <returns> 被选中的元素索引 </returns>
        public static int Choose(this ref Random random, IList<float> probabilities, int startIndex = 0, int count = 0)
        {
            if (count < 1 || count > probabilities.Count - startIndex)
            {
                count = probabilities.Count - startIndex;
            }

            int lastIndex = startIndex + count - 1;
            float rest = (float)random.Next01();
            float current;

            for (; startIndex < lastIndex; startIndex++)
            {
                current = probabilities[startIndex];
                if (rest < current) return startIndex;
                else rest -= current;
            }

            return lastIndex;
        }


        /// <summary>
        /// 将一组元素随机排序
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="list"> 元素列表 </param>
        /// <param name="startIndex"> 开始排序的索引 </param>
        /// <param name="count"> 执行排序的元素总数. 如果这个值无效, 自动遍历到列表尾部 </param>
        public static void Sort<T>(this ref Random random, IList<T> list, int startIndex = 0, int count = 0)
        {
            int lastIndex = startIndex + count;
            if (lastIndex <= startIndex || lastIndex > list.Count)
            {
                lastIndex = list.Count;
            }

            lastIndex -= 1;

            T temp;
            int swapIndex;

            for (int i = startIndex; i < lastIndex; i++)
            {
                swapIndex = random.Range(i, lastIndex + 1);
                temp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = temp;
            }
        }
        
                /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the collection.</typeparam>
        /// <param name="array">The colletion to pick the value from.</param>
        /// <returns>A random value from the collection.</returns>
        public static T PickRandom<T>(this T[] array)
        {
            return array[GetRandom(0, array.Length)];
        }

        /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the collection.</typeparam>
        /// <param name="list">The colletion to pick the value from.</param>
        /// <returns>A random value from the collection.</returns>
        public static T PickRandom<T>(this IList<T> list)
        {
            return list[GetRandom(0, list.Count)];
        }

        /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the collection.</typeparam>
        /// <param name="collection">The colletion to pick the value from.</param>
        /// <returns>A random value from the collection.</returns>
        public static T PickRandom<T>(this ICollection<T> collection)
        {
            return collection.ElementAt(GetRandom(0, collection.Count));
        }

        /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the collection.</typeparam>
        /// <param name="enumerable">The colletion to pick the value from.</param>
        /// <returns>A random value from the collection.</returns>
        public static T PickRandom<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(GetRandom(0, enumerable.Count()));
        }

        /// <summary>
        /// Removes a random value from the collection.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the collection.</typeparam>
        /// <param name="list">The collection to remove the value from.</param>
        /// <returns>The removed element.</returns>
        public static T RemoveRandom<T>(this IList<T> list)
        {
            int index = GetRandom(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }
}