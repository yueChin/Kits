using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections.DoubleLinkedList
{
    /// <summary>
    /// 描述：两向链表
    /// <para>创建时间：2016-3-21</para>
    /// </summary>
    public partial class DoubleLinkedList<TK, TV>
    {
        private readonly Node<TK, TV> m_Head;

        private readonly Node<TK, TV> m_Tail;

        private readonly Dictionary<TK, Node<TK, TV>> m_LinkedDict = new Dictionary<TK, Node<TK, TV>>();

        public DoubleLinkedList()
        {
            m_Head = new Node<TK, TV>();
            m_Tail = new Node<TK, TV>();

            m_Head.Next = m_Tail;
            m_Head.Next.Previous = m_Head;
        }

        /// <summary>
        /// 将结点添加到尾部
        /// </summary>
        /// <param name="node"></param>
        public void AddTail(Node<TK, TV> node)
        {
            //添加原链表包含此结点，则先将结点关系拆分
            if (m_LinkedDict.ContainsKey(node.Key))
                Detach(node);
            m_LinkedDict[node.Key] = node;

            node.Next = m_Tail;
            node.Previous = m_Tail.Previous;

            m_Tail.Previous = node;
            node.Previous.Next = node;
        }

        /// <summary>
        /// 将结点添加到头部
        /// </summary>
        /// <param name="node"></param>
        public void AddHead(Node<TK, TV> node)
        {
            //添加原链表包含此结点，则先将结点关系拆分
            if (m_LinkedDict.ContainsKey(node.Key))
                Detach(node);
            m_LinkedDict[node.Key] = node;

            node.Previous = m_Head;
            node.Next = m_Head.Next;

            m_Head.Next = node;
            node.Next.Previous = node;
        }


        /// <summary>
        /// 拆分结点
        /// </summary>
        /// <param name="node"></param>
        private void Detach(Node<TK, TV> node)
        {
            if (node == null) return;
            node.Next.Previous = node.Previous;
            node.Previous.Next = node.Next;
        }

        /// <summary>
        /// 删除结点
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(TK key)
        {
            if (!m_LinkedDict.ContainsKey(key)) return;

            Node<TK, TV> node = m_LinkedDict[key];
            Detach(node);
        }


        public void RemoveNode(Node<TK, TV> node)
        {
            if (node == null) return;
            RemoveKey(node.Key);
        }


        public Node<TK, TV> this[TK key]
        {
            get
            {
                if (!m_LinkedDict.ContainsKey(key)) return null;
                return m_LinkedDict[key];
            }
        }

        public int Size
        {
            get { return m_LinkedDict.Count; }
        }


    }
}