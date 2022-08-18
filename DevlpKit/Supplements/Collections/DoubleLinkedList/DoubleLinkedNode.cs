
namespace Kits.DevlpKit.Supplements.Collections.DoubleLinkedList
{
    /// <summary>
    /// 描述：
    /// <para>创建时间：</para>
    /// </summary>
    public class Node<TK, TV>
    {
        private readonly TK m_Key;
        public TV Value;

        public Node<TK, TV> Previous;
        public Node<TK, TV> Next;

        public Node()
        {
        }

        public Node(TK key, TV value)
        {
            m_Key = key;
            Value = value;
        }

        public TK Key
        {
            get { return m_Key; }
        }
    }
}