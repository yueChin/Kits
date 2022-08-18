using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Kits.DevlpKit.Supplements.Collections.AvlTree
{
    /// <summary>
    /// 平衡二叉树，默认通过 Hash 值作为比较 id，也可通过一个 IdentifierDelegate 的委托类型实现比较值的覆盖方法
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed partial class AvlTree<T> : ICollection<T>
    {
        public int Count { get { return mRoot?.Count ?? 0; } }
        public int Deep { get { return mRoot?.Deep ?? 0; } }
        public bool IsReadOnly { get { return false; } }
        public NodeInfo Root
        {
            get
            {
                NodeInfo info;
                info.Data = mRoot;
                return info;
            }
        }

        public NodeInfo Min
        {
            get
            {
                if (mRoot == null)
                    return default(NodeInfo);
                NodeInfo info;
                info.Data = mRoot.Min;
                return info;
            }
        }

        public NodeInfo Max
        {
            get
            {
                if (mRoot == null)
                    return default(NodeInfo);
                NodeInfo info;
                info.Data = mRoot.Max;
                return info;
            }
        }

        public delegate int IdentifierDelegate<T>(T target);
        
        private IdentifierDelegate<T> m_Identifier;
        private Node mRoot;
        private Stack<Node> m_Cache;

        public AvlTree(IdentifierDelegate<T> identifier = null)
        {
            if (identifier == null)
                m_Identifier = (x) => x == null ? 0 : x.GetHashCode();
            else
                m_Identifier = identifier;
            m_Cache = new Stack<Node>(32);
        }

        public int GetDataId(T data)
        {
            return m_Identifier(data);
        }

        public T Add(T item, out bool replaced)
        {
            int id = m_Identifier(item);
            Node newnode = Node.GetNode(id, item);
            if (mRoot == null)
            {
                mRoot = newnode;
            }
            else
            {
                Node node = mRoot;
                while (node != null)
                {
                    if (node.ID == id)
                    {
                        replaced = true;
                        T ret = node.Value;
                        node.Value = item;
                        return ret;
                    }
                    else if (node.ID > id)
                    {
                        if (node.Left == null)
                        {
                            node.AddLeftLeaf(newnode);
                            node.UpdateTree();
                            if (node.Parent != null)
                                node.Parent.FixBalence();
                            break;
                        }
                        else
                        {
                            node = node.Left;
                        }
                    }
                    else
                    {
                        if (node.Right == null)
                        {
                            node.AddRightLeaf(newnode);
                            node.UpdateTree();
                            if (node.Parent != null)
                                node.Parent.FixBalence();
                            break;
                        }
                        else
                        {
                            node = node.Right;
                        }
                    }
                }
                while (mRoot.Parent != null)
                    mRoot = mRoot.Parent;
            }
            replaced = false;
            return default(T);
        }

        public void Add(T item)
        {
            bool replaced;
#if TEST
            var ret =
#endif
                Add(item, out replaced);
#if TEST
            if (replaced)
            {
                Debug.LogWarningFormat("AvlTree<{0}> replaced {1} to {2}", typeof(T).Name, ret, item);
            }
#endif
        }

        public void Clear()
        {
            m_Cache.Clear();
            Node node = mRoot;
            while (node != null)
            {
                m_Cache.Push(node);
                node = node.Left;
            }
            while (m_Cache.Count > 0)
            {
                Node v = m_Cache.Pop();
                Node sub = v.Right;
                v.Release();
                while (sub != null)
                {
                    m_Cache.Push(sub);
                    sub = sub.Left;
                }
            }
            mRoot = null;
        }

        public void ClearWithCallback(System.Action<T> callback)
        {
            m_Cache.Clear();
            Node node = mRoot;
            while (node != null)
            {
                m_Cache.Push(node);
                node = node.Left;
            }
            while (m_Cache.Count > 0)
            {
                Node v = m_Cache.Pop();
                Node sub = v.Right;
                callback(v.Value);
                v.Release();
                while (sub != null)
                {
                    m_Cache.Push(sub);
                    sub = sub.Left;
                }
            }
            mRoot = null;
        }

        /// <summary>
        /// 查找节点信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public NodeInfo FindNode(T item)
        {
            return FindNodeById(m_Identifier(item));
        }

        public NodeInfo FindNodeById(int id)
        {
            Node node = mRoot;
            while (node != null)
            {
                if (node.ID == id)
                {
                    NodeInfo info;
                    info.Data = node;
                    return info;
                }
                else if (node.ID < id)
                    node = node.Right;
                else
                    node = node.Left;
            }
            return default(NodeInfo);
        }
        
        /// <summary>
        /// 回传指定 id 的节点数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetData(int id)
        {
            Node node = mRoot;
            while (node != null)
            {
                if (node.ID == id)
                    return node.Value;
                else if (node.ID < id)
                    node = node.Right;
                else
                    node = node.Left;
            }
            return default(T);
        }

        public bool Contains(T item)
        {
            return ContainsId(m_Identifier(item));
        }

        public bool ContainsId(int id)
        {
            Node node = mRoot;
            while (node != null)
            {
                if (node.ID == id)
                    return true;
                else if (node.ID < id)
                    node = node.Right;
                else
                    node = node.Left;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Cache.Clear();
            Node node = mRoot;
            while (node != null)
            {
                m_Cache.Push(node);
                node = node.Left;
            }
            for (int i = arrayIndex; i < array.Length; i++)
            {
                if (m_Cache.Count > 0)
                {
                    Node v = m_Cache.Pop();
                    array[i] = v.Value;
                    v = v.Right;
                    while(v != null)
                    {
                        m_Cache.Push(v);
                        v = v.Left;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public bool RemoveById(int id)
        {
            Node node = mRoot;
            while(node != null)
            {
                if(node.ID == id)
                {
                    mRoot = node.RemoveAndReturnDirty();
                    if(mRoot != null)
                    {
                        mRoot.UpdateTree();
                        mRoot.FixBalence();
                    }
                    while (mRoot != null && mRoot.Parent != null)
                        mRoot = mRoot.Parent;
                    Node.CacheNode(node);
                    return true;
                }
                else if(node.ID < id)
                {
                    node = node.Right;
                }
                else
                {
                    node = node.Left;
                }
            }
            return false;
        }
        
        public bool Remove(T item)
        {
            return RemoveById(m_Identifier(item));
        }

        public IEnumerator<T> GetEnumerator(int startId, int endId)
        {
            return new RangeEnumerator(this, startId, endId);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetInfo(int start, int end)
        {
            StringBuilder buff = new StringBuilder();
            buff.Append("AVL[Count:").Append(Count).Append(", Deep:").Append(Deep).Append(", Root:")
                .Append(mRoot == null ? " " : mRoot.ID.ToString()).Append("]");
            m_Cache.Clear();
            Node node = mRoot;
            while (node != null)
            {
                if(node.ID < start)
                {
                    node = node.Right;
                }
                else if(node.ID > start)
                {
                    m_Cache.Push(node);
                    node = node.Left;
                }
                else
                {
                    m_Cache.Push(node);
                    break;
                }
            }
            int n = -1;
            while (m_Cache.Count > 0)
            {
                node = m_Cache.Pop();
                if (node.ID > end)
                    break;
                if (node.ID >= start)
                {
                    if (n == -1)
                        n = node.Index;
                    buff.Append("\n [").Append(n++).Append("] ").Append(node);
                }
                node = node.Right;
                while (node != null)
                {
                    m_Cache.Push(node);
                    node = node.Left;
                }
            }
            return buff.ToString();
        }
    }
}