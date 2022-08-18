using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections.AvlTree
{
    /// <summary>
    /// 平衡二叉树，默认通过 Hash 值作为比较 id，也可通过一个 IdentifierDelegate 的委托类型实现比较值的覆盖方法
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed partial class AvlTree<T> : ICollection<T>
    {
        /// <summary>
        /// 节点属性
        /// </summary>
        public struct NodeInfo
        {
            internal Node Data;
            public bool isExists { get { return Data != null && Data.Count > 0; } }
            public T value { get { return Data == null ? default(T) : Data.Value; } }
            public int deep { get { return Data?.Deep ?? 0; } }
            public int count { get { return Data?.Count ?? 0; } }
            public int id { get { return isExists ? Data.ID : 0; } }
            public int index { get { return isExists ? Data.Index : -1; } }

            public NodeInfo parent
            {
                get
                {
                    if (Data == null)
                        return default(NodeInfo);
                    NodeInfo info;
                    info.Data = Data.Parent;
                    return info;
                }
            }

            public NodeInfo leftChild
            {
                get
                {
                    if (Data == null)
                        return default(NodeInfo);
                    NodeInfo info;
                    info.Data = Data.Left;
                    return info;
                }
            }

            public NodeInfo rightChild
            {
                get
                {
                    if (Data == null)
                        return default(NodeInfo);
                    NodeInfo info;
                    info.Data = Data.Right;
                    return info;
                }
            }

            public NodeInfo min
            {
                get
                {
                    if (Data == null)
                        return default(NodeInfo);
                    NodeInfo info;
                    info.Data = Data.Min;
                    return info;
                }
            }

            public NodeInfo max
            {
                get
                {
                    if (Data == null)
                        return default(NodeInfo);
                    NodeInfo info;
                    info.Data = Data.Max;
                    return info;
                }
            }

            public override string ToString()
            {
                if (Data == null)
                    return "Not exists Avl node.";
                else
                    return Data.ToString();
            }

            public override int GetHashCode()
            {
                return id;
            }

            public override bool Equals(object obj)
            {
                if (obj is NodeInfo)
                    return ((NodeInfo)obj).id == this.id;
                else
                    return false;
            }

            public static bool operator ==(NodeInfo a, NodeInfo b)
            {
                return a.id == b.id;
            }

            public static bool operator !=(NodeInfo a, NodeInfo b)
            {
                return a.id != b.id;
            }
        }

    }
}