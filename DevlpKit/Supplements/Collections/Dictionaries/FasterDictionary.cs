using System;
using System.Runtime.CompilerServices;
using Kits.DevlpKit.Helpers;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Collections.Dictionaries
{
    /// <summary>
    /// This dictionary has been created for just one reason: I needed a dictionary that would have let me iterate
    /// over the values as an array, directly, without generating one or using an iterator.
    /// For this goal is N times faster than the standard dictionary. Faster dictionary is also faster than
    /// the standard dictionary for most of the operations, but the difference is negligible. The only slower operation
    /// is resizing the memory on add, as this implementation needs to use two separate arrays compared to the standard
    /// one
    /// note: use native memory? Use _valuesInfo only when there are collisions?
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class FasterDictionary<TKey, TValue> where TKey : IEquatable<TKey>
    {
        public FasterDictionary(uint size)
        {
            m_ValuesInfo = new FasterDictionaryNode<TKey>[size];
            m_Values = new TValue[size];
            m_Buckets = new int[HashTableHelper.GetPrime((int)size)];
        }

        public FasterDictionary()
        {
            m_ValuesInfo = new FasterDictionaryNode<TKey>[1];
            m_Values = new TValue[1];
            m_Buckets = new int[3];
        }

        public void CopyValuesTo(TValue[] tasks, uint index)
        {
            Array.Copy(m_Values, 0, tasks, index, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue[] GetValuesArray(out uint count)
        {
            count = m_FreeValueCellIndex;

            return m_Values;
        }

        public TValue[] unsafeValues
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_Values;
        }

        public FasterDictionaryNode<TKey>[] unsafeKeys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_ValuesInfo;
        }

        public uint count => m_FreeValueCellIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, in TValue value)
        {
            if (AddValue(key, in value, out _) == false)
                throw new FasterDictionaryException("Key already present");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(TKey key, in TValue value)
        {
            AddValue(key, in value, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (m_FreeValueCellIndex == 0) return;

            m_FreeValueCellIndex = 0;

            Array.Clear(m_Buckets, 0, m_Buckets.Length);
            Array.Clear(m_Values, 0, m_Values.Length);
            Array.Clear(m_ValuesInfo, 0, m_ValuesInfo.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FastClear()
        {
            if (m_FreeValueCellIndex == 0) return;

            m_FreeValueCellIndex = 0;

            Array.Clear(m_Buckets, 0, m_Buckets.Length);
            Array.Clear(m_ValuesInfo, 0, m_ValuesInfo.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key)
        {
            return TryFindIndex(key, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FasterDictionaryKeyValueEnumerator GetEnumerator()
        {
            return new FasterDictionaryKeyValueEnumerator(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TValue result)
        {
            if (TryFindIndex(key, out uint findIndex) == true)
            {
                result = m_Values[(int)findIndex];
                return true;
            }

            result = default;
            return false;
        }

        //todo: can be optimized
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TValue GetOrCreate(TKey key)
        {
            if (TryFindIndex(key, out uint findIndex) == true)
            {
                return ref m_Values[(int)findIndex];
            }

            AddValue(key, default, out findIndex);

            return ref m_Values[(int)findIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TValue GetOrCreate(TKey key, Func<TValue> builder)
        {
            if (TryFindIndex(key, out uint findIndex) == true)
            {
                return ref m_Values[(int)findIndex];
            }

            AddValue(key, builder(), out findIndex);

            return ref m_Values[(int)findIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TValue GetValueByRef(TKey key)
        {
            if (TryFindIndex(key, out uint findIndex) == true)
            {
                return ref m_Values[(int)findIndex];
            }

            throw new FasterDictionaryException("Key not found");
        }

        public void SetCapacity(uint size)
        {
            if (m_Values.Length < size)
            {
                Array.Resize(ref m_Values, (int)size);
                Array.Resize(ref m_ValuesInfo, (int)size);
            }
        }

        public TValue this[TKey key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_Values[(int)GetIndex(key)];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AddValue(key, in value, out _);
        }

        bool AddValue(TKey key, in TValue value, out uint indexSet)
        {
            int hash = key.GetHashCode();
            uint bucketIndex = Reduce((uint)hash, (uint)m_Buckets.Length);

            if (m_FreeValueCellIndex == m_Values.Length)
            {
                int expandPrime = HashTableHelper.ExpandPrime((int)m_FreeValueCellIndex);

                Array.Resize(ref m_Values, expandPrime);
                Array.Resize(ref m_ValuesInfo, expandPrime);
            }

            //buckets value -1 means it's empty
            int valueIndex = m_Buckets[bucketIndex] - 1;

            if (valueIndex == -1)
                //create the info node at the last position and fill it with the relevant information
                m_ValuesInfo[m_FreeValueCellIndex] = new FasterDictionaryNode<TKey>(ref key, hash);
            else //collision or already exists
            {
                int currentValueIndex = valueIndex;
                do
                {
                    //must check if the key already exists in the dictionary
                    //for some reason this is faster than using Comparer<TKey>.default, should investigate
                    ref FasterDictionaryNode<TKey> fasterDictionaryNode = ref m_ValuesInfo[currentValueIndex];
                    if (fasterDictionaryNode.Hashcode == hash &&
                        fasterDictionaryNode.Key.Equals(key) == true)
                    {
                        //the key already exists, simply replace the value!
                        m_Values[currentValueIndex] = value;
                        indexSet = (uint)currentValueIndex;
                        return false;
                    }

                    currentValueIndex = fasterDictionaryNode.Previous;
                } while (currentValueIndex != -1); //-1 means no more values with key with the same hash

                //oops collision!
                m_Collisions++;
                //create a new node which previous index points to node currently pointed in the bucket
                m_ValuesInfo[m_FreeValueCellIndex] = new FasterDictionaryNode<TKey>(ref key, hash, valueIndex);
                //update the next of the existing cell to point to the new one
                //old one -> new one | old one <- next one
                m_ValuesInfo[valueIndex].Next = (int)m_FreeValueCellIndex;
                //Important: the new node is always the one that will be pointed by the bucket cell
                //so I can assume that the one pointed by the bucket is always the last value added
                //(next = -1)
            }

            //item with this bucketIndex will point to the last value created
            //ToDo: if instead I assume that the original one is the one in the bucket
            //I wouldn't need to update the bucket here. Small optimization but important
            m_Buckets[bucketIndex] = (int)(m_FreeValueCellIndex + 1);

            m_Values[(int)m_FreeValueCellIndex] = value;
            indexSet = m_FreeValueCellIndex;

            m_FreeValueCellIndex++;

            //too many collisions?
            if (m_Collisions > m_Buckets.Length)
            {
                //we need more space and less collisions
                m_Buckets = new int[HashTableHelper.ExpandPrime((int)m_Collisions)];

                m_Collisions = 0;

                //we need to get all the hash code of all the values stored so far and spread them over the new bucket
                //length
                for (int newValueIndex = 0; newValueIndex < m_FreeValueCellIndex; newValueIndex++)
                {
                    //get the original hash code and find the new bucketIndex due to the new length
                    ref FasterDictionaryNode<TKey> fasterDictionaryNode = ref m_ValuesInfo[newValueIndex];
                    bucketIndex = Reduce((uint)fasterDictionaryNode.Hashcode, (uint)m_Buckets.Length);
                    //bucketsIndex can be -1 or a next value. If it's -1 means no collisions. If there is collision,
                    //we create a new node which prev points to the old one. Old one next points to the new one.
                    //the bucket will now points to the new one
                    //In this way we can rebuild the linkedlist.
                    //get the current valueIndex, it's -1 if no collision happens
                    int existingValueIndex = m_Buckets[bucketIndex] - 1;
                    //update the bucket index to the index of the current item that share the bucketIndex
                    //(last found is always the one in the bucket)
                    m_Buckets[bucketIndex] = newValueIndex + 1;
                    if (existingValueIndex != -1)
                    {
                        //oops a value was already being pointed by this cell in the new bucket list,
                        //it means there is a collision, problem
                        m_Collisions++;
                        //the bucket will point to this value, so 
                        //the previous index will be used as previous for the new value.
                        fasterDictionaryNode.Previous = existingValueIndex;
                        fasterDictionaryNode.Next = -1;
                        //and update the previous next index to the new one
                        m_ValuesInfo[existingValueIndex].Next = newValueIndex;
                    }
                    else
                    {
                        //ok nothing was indexed, the bucket was empty. We need to update the previous
                        //values of next and previous
                        fasterDictionaryNode.Next = -1;
                        fasterDictionaryNode.Previous = -1;
                    }
                }
            }

            return true;
        }

        public bool Remove(TKey key)
        {
            int hash = Hash(key);
            uint bucketIndex = Reduce((uint)hash, (uint)m_Buckets.Length);

            //find the bucket
            int indexToValueToRemove = m_Buckets[bucketIndex] - 1;

            //Part one: look for the actual key in the bucket list if found I update the bucket list so that it doesn't
            //point anymore to the cell to remove
            while (indexToValueToRemove != -1)
            {
                ref FasterDictionaryNode<TKey> fasterDictionaryNode = ref m_ValuesInfo[indexToValueToRemove];
                if (fasterDictionaryNode.Hashcode == hash &&
                    fasterDictionaryNode.Key.Equals(key) == true)
                {
                    //if the key is found and the bucket points directly to the node to remove
                    if (m_Buckets[bucketIndex] - 1 == indexToValueToRemove)
                    {
                        ContractChecks.Require(fasterDictionaryNode.Next == -1,
                            "if the bucket points to the cell, next MUST NOT exists");
                        //the bucket will point to the previous cell. if a previous cell exists
                        //its next pointer must be updated!
                        //<--- iteration order  
                        //                      B(ucket points always to the last one)
                        //   ------- ------- -------
                        //   |  1  | |  2  | |  3  | //bucket cannot have next, only previous
                        //   ------- ------- -------
                        //--> insert order
                        int value = fasterDictionaryNode.Previous;
                        m_Buckets[bucketIndex] = value + 1;
                    }
                    else
                        ContractChecks.Require(fasterDictionaryNode.Next != -1,
                            "if the bucket points to another cell, next MUST exists");

                    UpdateLinkedList(indexToValueToRemove, m_ValuesInfo);

                    break;
                }

                indexToValueToRemove = fasterDictionaryNode.Previous;
            }

            if (indexToValueToRemove == -1)
                return false; //not found!

            m_FreeValueCellIndex--; //one less value to iterate

            //Part two:
            //At this point nodes pointers and buckets are updated, but the _values array
            //still has got the value to delete. Remember the goal of this dictionary is to be able
            //to iterate over the values like an array, so the values array must always be up to date

            //if the cell to remove is the last one in the list, we can perform less operations (no swapping needed)
            //otherwise we want to move the last value cell over the value to remove
            if (indexToValueToRemove != m_FreeValueCellIndex)
            {
                //we can move the last value of both arrays in place of the one to delete.
                //in order to do so, we need to be sure that the bucket pointer is updated.
                //first we find the index in the bucket list of the pointer that points to the cell
                //to move
                uint movingBucketIndex =
                    Reduce((uint)m_ValuesInfo[m_FreeValueCellIndex].Hashcode, (uint)m_Buckets.Length);

                //if the key is found and the bucket points directly to the node to remove
                //it must now point to the cell where it's going to be moved
                if (m_Buckets[movingBucketIndex] - 1 == m_FreeValueCellIndex)
                    m_Buckets[movingBucketIndex] = (int)(indexToValueToRemove + 1);

                //otherwise it means that there was more than one key with the same hash (collision), so 
                //we need to update the linked list and its pointers
                int next = m_ValuesInfo[m_FreeValueCellIndex].Next;
                int previous = m_ValuesInfo[m_FreeValueCellIndex].Previous;

                //they now point to the cell where the last value is moved into
                if (next != -1)
                    m_ValuesInfo[next].Previous = (int)indexToValueToRemove;
                if (previous != -1)
                    m_ValuesInfo[previous].Next = (int)indexToValueToRemove;

                //finally, actually move the values
                m_ValuesInfo[indexToValueToRemove] = m_ValuesInfo[m_FreeValueCellIndex];
                m_Values[indexToValueToRemove] = m_Values[(int)m_FreeValueCellIndex];
            }

            return true;
        }

        public void Trim()
        {
            Array.Resize(ref m_Values, (int)Math.Max(m_FreeValueCellIndex, 1));
            Array.Resize(ref m_ValuesInfo, (int)Math.Max(m_FreeValueCellIndex, 1));
        }

        //I store all the index with an offset + 1, so that in the bucket list 0 means actually not existing.
        //When read the offset must be offset by -1 again to be the real one. In this way
        //I avoid to initialize the array to -1
        public bool TryFindIndex(TKey key, out uint findIndex)
        {
            int hash = Hash(key);
            uint bucketIndex = Reduce((uint)hash, (uint)m_Buckets.Length);

            int valueIndex = m_Buckets[bucketIndex] - 1;

            //even if we found an existing value we need to be sure it's the one we requested
            while (valueIndex != -1)
            {
                //for some reason this is way faster than using Comparer<TKey>.default, should investigate
                ref FasterDictionaryNode<TKey> fasterDictionaryNode = ref m_ValuesInfo[valueIndex];

                if (fasterDictionaryNode.Hashcode == hash && fasterDictionaryNode.Key.Equals(key) == true)
                {
                    //this is the one
                    findIndex = (uint)valueIndex;
                    return true;
                }

                valueIndex = fasterDictionaryNode.Previous;
            }

            findIndex = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetIndex(TKey key)
        {
            if (TryFindIndex(key, out uint findIndex)) return findIndex;

            throw new FasterDictionaryException("Key not found");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int Hash(TKey key)
        {
            return key.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint Reduce(uint x, uint n)
        {
            if (x >= n)
                return x % n;

            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void UpdateLinkedList(int index, FasterDictionaryNode<TKey>[] valuesInfo)
        {
            int next = valuesInfo[index].Next;
            int previous = valuesInfo[index].Previous;

            if (next != -1)
                valuesInfo[next].Previous = previous;
            if (previous != -1)
                valuesInfo[previous].Next = next;
        }

        public struct FasterDictionaryKeyValueEnumerator
        {
            public FasterDictionaryKeyValueEnumerator(FasterDictionary<TKey, TValue> dic) : this()
            {
                m_Dic = dic;
                m_Index = -1;
                m_Count = (int)dic.count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
#if DEBUG && !PROFILE_SVELTO
                if (m_Count != m_Dic.count)
                    throw new FasterDictionaryException("can't modify a dictionary during its iteration");
#endif
                if (m_Index < m_Count - 1)
                {
                    ++m_Index;
                    return true;
                }

                return false;
            }

            public KeyValuePairFast Current => new KeyValuePairFast(m_Dic.m_ValuesInfo[m_Index].Key, m_Dic.unsafeValues, m_Index);

            readonly FasterDictionary<TKey, TValue> m_Dic;
            readonly int m_Count;

            int m_Index;
        }

        /// <summary>
        ///the mechanism to use arrays is fundamental to work 
        /// </summary>
        public readonly ref struct KeyValuePairFast
        {
            readonly TValue[] m_DicValues;
            readonly TKey m_Key;
            readonly int m_Index;

            public KeyValuePairFast(TKey keys, TValue[] dicValues, int index)
            {
                m_DicValues = dicValues;
                m_Index = index;
                m_Key = keys;
            }

            public TKey Key => m_Key;
            public ref TValue Value => ref m_DicValues[m_Index];
        }

        TValue[] m_Values;
        FasterDictionaryNode<TKey>[] m_ValuesInfo;
        int[] m_Buckets;
        uint m_FreeValueCellIndex;
        uint m_Collisions;
    }

    public class FasterDictionaryException : Exception
    {
        public FasterDictionaryException(string keyAlreadyExisting) : base(keyAlreadyExisting)
        {
        }
    }
}