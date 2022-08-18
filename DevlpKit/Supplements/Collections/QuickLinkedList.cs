using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections
{
    /// <summary>
    /// 快速链表，与 LinkedList 相比，本实现使用连续的 struct node 提高性能
    /// 注意：必须使用带参数的构造函数
    /// </summary>
    public struct QuickLinkedList<T>
    {
        /// <summary>
        /// 链表节点
        /// </summary>
        public struct Node
        {
            public int Previous;
            public int Next;
            public T Value;
        }


        readonly Stack<int> m_EmptyIds;
        readonly List<Node> m_List;
        int m_First;
        int m_Last;


        /// <summary>
        /// 链表第一个节点的 id
        /// </summary>
        public int first { get { return m_First; } }


        /// <summary>
        /// 链表最后一个节点的 id
        /// </summary>
        public int last { get { return m_Last; } }


        /// <summary>
        /// 链表节点总数
        /// </summary>
        public int count { get { return m_List.Count - m_EmptyIds.Count; } }


        /// <summary>
        /// 通过 id 访问节点的值
        /// </summary>
        public T this[int id]
        {
            get
            {
                Node node = m_List[id];
                if (node.Previous == node.Next && m_First != id)
                {
                    throw new Exception("invalid id");
                }

                return node.Value;
            }
            set
            {
                Node node = m_List[id];
                if (node.Previous == node.Next && m_First != id)
                {
                    throw new Exception("invalid id");
                }

                node.Value = value;
                m_List[id] = node;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public QuickLinkedList(int capacity)
        {
            m_EmptyIds = new Stack<int>(4);
            m_List = new List<Node>(capacity);
            m_First = -1;
            m_Last = -1;
        }


        /// <summary>
        /// 
        /// </summary>
        public Node GetNode(int id)
        {
            return m_List[id];
        }


        /// <summary>
        /// 
        /// </summary>
        public int GetPrevious(int id)
        {
            return m_List[id].Previous;
        }


        /// <summary>
        /// 
        /// </summary>
        public int GetNext(int id)
        {
            return m_List[id].Next;
        }


        int Add(ref Node node)
        {
            int index;

            if (m_EmptyIds.Count > 0)
            {
                index = m_EmptyIds.Pop();
                m_List[index] = node;
            }
            else
            {
                m_List.Add(node);
                index = m_List.Count - 1;
            }

            return index;
        }


        void Remove(int id, ref Node node)
        {
            if (m_First == id) m_First = node.Next;
            if (m_Last == id) m_Last = node.Previous;

            if (node.Previous != -1)
            {
                Node tmp = m_List[node.Previous];
                tmp.Next = node.Next;
                m_List[node.Previous] = tmp;
                node.Previous = -1;
            }
            if (node.Next != -1)
            {
                Node tmp = m_List[node.Next];
                tmp.Previous = node.Previous;
                m_List[node.Next] = tmp;
                node.Next = -1;
            }

            node.Value = default;
            m_List[id] = node;
            m_EmptyIds.Push(id);
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddFirst(T value)
        {
            Node node = new Node { Previous = -1, Next = m_First, Value = value };
            int newId = Add(ref node);

            if (m_First == -1) m_Last = newId;
            else
            {
                Node tmp = m_List[m_First];
                tmp.Previous = newId;
                m_List[m_First] = tmp;
            }
            m_First = newId;

            return newId;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddLast(T value)
        {
            Node node = new Node { Previous = m_Last, Next = -1, Value = value };
            int newId = Add(ref node);

            if (m_First == -1) m_First = newId;
            else
            {
                Node tmp = m_List[m_Last];
                tmp.Next = newId;
                m_List[m_Last] = tmp;
            }
            m_Last = newId;

            return m_Last;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddAfter(int id, T value)
        {
            Node prevNode = m_List[id];
            if (prevNode.Previous == prevNode.Next && m_First != id)
            {
                throw new Exception("invalid id");
            }

            Node node = new Node { Previous = id, Next = prevNode.Next, Value = value };
            int newId = Add(ref node);

            if (prevNode.Next == -1)
            {
                m_Last = newId;
            }
            else
            {
                Node tmp = m_List[prevNode.Next];
                tmp.Previous = newId;
                m_List[prevNode.Next] = tmp;
            }
            prevNode.Next = newId;
            m_List[id] = prevNode;

            return newId;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddBefore(int id, T value)
        {
            Node nextNode = m_List[id];
            if (nextNode.Previous == nextNode.Next && m_First != id)
            {
                throw new Exception("invalid id");
            }

            Node node = new Node { Previous = nextNode.Previous, Next = id, Value = value };
            int newId = Add(ref node);

            if (nextNode.Previous == -1)
            {
                m_First = newId;
            }
            else
            {
                Node tmp = m_List[nextNode.Previous];
                tmp.Next = newId;
                m_List[nextNode.Previous] = tmp;
            }
            nextNode.Previous = newId;
            m_List[id] = nextNode;

            return newId;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_EmptyIds.Clear();
            m_List.Clear();
            m_First = -1;
            m_Last = -1;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        public void Remove(int id)
        {
            Node node = m_List[id];
            if (node.Previous == node.Next && m_First != id)
            {
                throw new Exception("invalid id");
            }
            Remove(id, ref node);
        }


        /// <summary>
        /// O(1)
        /// </summary>
        public void RemoveFirst()
        {
            if (m_First == -1)
            {
                throw new Exception("empty list");
            }
            Node node = m_List[m_First];
            Remove(m_First, ref node);
        }


        /// <summary>
        /// O(1)
        /// </summary>
        public void RemoveLast()
        {
            if (m_Last == -1)
            {
                throw new Exception("empty list");
            }
            Node node = m_List[m_Last];
            Remove(m_Last, ref node);
        }

    } // QuickLinkedList<T>

} // namespace UnityExtensions