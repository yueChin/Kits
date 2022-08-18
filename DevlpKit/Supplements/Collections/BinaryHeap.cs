using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections
{
    public enum HeapType
    {
        MinHeap,
        MaxHeap
    }

    public class BinaryHeap<T> where T : IComparable<T>
    {
        private readonly List<T> m_ItemsList;

        public HeapType HType { get; private set; }

        public T Root
        {
            get { return m_ItemsList[0]; }
        }

        public BinaryHeap(HeapType type)
        {
            m_ItemsList = new List<T>();
            this.HType = type;
        }

        public void Push(T item)
        {
            m_ItemsList.Add(item);

            int i = m_ItemsList.Count - 1;

            bool flag = HType == HeapType.MinHeap;

            while (i > 0)
            {
                if ((m_ItemsList[i].CompareTo(m_ItemsList[(i - 1) / 2]) > 0) ^ flag)
                {
                    (m_ItemsList[i], m_ItemsList[(i - 1) / 2]) = (m_ItemsList[(i - 1) / 2], m_ItemsList[i]);
                    i = (i - 1) / 2;
                }
                else
                    break;
            }
        }

        private void DeleteRoot()
        {
            int i = m_ItemsList.Count - 1;

            m_ItemsList[0] = m_ItemsList[i];
            m_ItemsList.RemoveAt(i);

            i = 0;

            bool flag = HType == HeapType.MinHeap;

            while (true)
            {
                int leftInd = 2 * i + 1;
                int rightInd = 2 * i + 2;
                int largest = i;

                if (leftInd < m_ItemsList.Count)
                {
                    if ((m_ItemsList[leftInd].CompareTo(m_ItemsList[largest]) > 0) ^ flag)
                        largest = leftInd;
                }

                if (rightInd < m_ItemsList.Count)
                {
                    if ((m_ItemsList[rightInd].CompareTo(m_ItemsList[largest]) > 0) ^ flag)
                        largest = rightInd;
                }

                if (largest != i)
                {
                    (m_ItemsList[largest], m_ItemsList[i]) = (m_ItemsList[i], m_ItemsList[largest]);
                    i = largest;
                }
                else
                    break;
            }
        }

        public T PopRoot()
        {
            T result = m_ItemsList[0];

            DeleteRoot();

            return result;
        }
    }

}

