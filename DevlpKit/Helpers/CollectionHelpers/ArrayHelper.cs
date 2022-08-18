
using System;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static class ArrayHelper
    {
		public static void Add<T>(ref T[] array, T item)
		{
			if (array == null)
			{
				array = new T[1];
				array[0] = item;
			}
			else
			{
				T[] newArray = new T[array.Length + 1];

				if (array.Length > 0)
					Array.Copy(array, newArray, array.Length);

				newArray[array.Length] = item;
				array = newArray;
			}
		}

		public static void Concat<T>(ref T[] array, T[] items)
		{
			if (array == null)
			{
				array = new T[items.Length];
				Array.Copy(items, array, items.Length);
			}
			else
			{
				T[] newArray = new T[array.Length + items.Length];

				if (array.Length > 0)
					Array.Copy(array, newArray, array.Length);

				if (items.Length > 0)
					Array.Copy(items, 0, newArray, array.Length, items.Length);

				array = newArray;
			}
		}
		
		public static void Insert<T>(ref T[] array, T item, int index)
		{
			T[] newArray = new T[array.Length + 1];

			if (index > 0)
				Array.Copy(array, newArray, index);

			newArray[index] = item;

			if (index != array.Length)
				Array.Copy(array, index, newArray, index + 1, array.Length - index);

			array = newArray;
		}

		public static void Remove<T>(ref T[] array, T item)
		{
			int index = Array.IndexOf<T>(array, item);
			if (index != -1)
				RemoveAt<T>(ref array, index);
		}

		public static void RemoveAt<T>(ref T[] array, int index)
		{
			T[] newArray = new T[array.Length - 1];

			if (index > 0)
			{
				Array.Copy(array, newArray, index);
			}
			if (index + 1 < array.Length)
			{
				Array.Copy(array, index + 1, newArray, index, array.Length - 1 - index);
			}

			array = newArray;
		}

		public static void Randomise<T>(T[] array)
		{
			int n = array.Length;
			while (n > 1)
			{
				n--;
				int k = UnityEngine.Random.Range(0, n + 1);
				(array[k], array[n]) = (array[n], array[k]);
			}
		}
		
		/// <summary>
        /// Generic Function to remove an array element at a certain index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputArray"></param>
        /// <param name="indexToRemove"></param>
        /// <returns></returns>
        public static T[] RemoveArrayIndexAt<T>(T[] inputArray, int indexToRemove)
        {
            T[] newArray = new T[inputArray.Length - 1];
            for (int i = 0; i < newArray.Length; ++i)
            {
                if (i < indexToRemove)
                {
                    newArray[i] = inputArray[i];
                }
                else if (i >= indexToRemove)
                {
                    newArray[i] = inputArray[i + 1];
                }
            }
            return newArray;
        }


        /// <summary>
        /// Generic Function to add a new array element at the end of an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputArray"></param>
        /// <param name="newElement"></param>
        /// <returns></returns>
        public static T[] AddElementToArray<T>(T[] inputArray, T newElement)
        {
            T[] newArray = new T[inputArray.Length + 1];
            for (int i = 0; i < inputArray.Length; ++i)
            {
                newArray[i] = inputArray[i];
            }
            newArray[newArray.Length - 1] = newElement;
            return newArray;
        }

        /// <summary>
        /// Generic Function to insert a new array element at a certain index position in an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputArray"></param>
        /// <param name="newElement"></param>
        /// <param name="indexToInsertAt"></param>
        /// <returns></returns>
        public static T[] InsertElementInArray<T>(T[] inputArray, T newElement, int indexToInsertAt)
        {
            T[] newArray = new T[inputArray.Length + 1];
            for (int i = 0; i < newArray.Length; ++i)
            {
                if (i < indexToInsertAt)
                {
                    newArray[i] = inputArray[i];
                }
                else if (i > indexToInsertAt)
                {
                    newArray[i] = inputArray[i - 1];
                }
                else
                {
                    newArray[i] = newElement;
                }
            }

            return newArray;
        }

        /// <summary>
        /// Generic Function to swap the position of two array elements
        /// </summary>
        /// <param name="m_reorderableRuleMasksLists"></param>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        /// <returns></returns>
        public static T[] SwapElementsInArray<T>(T[] inputArray, int firstIndex, int secondIndex)
        {
            //sanity check
            int maxIndex = inputArray.Length - 1;
            if (firstIndex > maxIndex || secondIndex > maxIndex || firstIndex < 0 || secondIndex < 0)
            {
                return inputArray;
            }

            (inputArray[firstIndex], inputArray[secondIndex]) = (inputArray[secondIndex], inputArray[firstIndex]);

            return inputArray;
        }
    }
}
