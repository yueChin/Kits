using System.Collections;
using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections.AvlTree
{
    /// <summary>
    /// 平衡二叉树，默认通过 Hash 值作为比较 id，也可通过一个 IdentifierDelegate 的委托类型实现比较值的覆盖方法
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed partial class AvlTree<T> : ICollection<T>
    {
        // 区间迭代器
        internal class RangeEnumerator : IEnumerator<T>
        {
            AvlTree<T> avl;
            Node current;
            Stack<Node> visitStack;
            int startId;
            int endId;

            public RangeEnumerator(AvlTree<T> avl, int startId, int endId)
            {
                visitStack = new Stack<Node>(avl.Deep);
                this.startId = startId;
                this.endId = endId;
                this.avl = avl;
                Reset();
            }

            public T Current { get { return current == null ? default(T) : current.Value; } }

            object IEnumerator.Current { get { return current == null ? default(T) : current.Value; } }

            public void Dispose()
            {
                visitStack.Clear();
                current = null;
                avl = null;
            }

            public bool MoveNext()
            {
                while (visitStack.Count > 0)
                {
                    Node node = visitStack.Pop();
                    if (node.ID > endId)
                        return false;
                    Node c = node;
                    node = node.Right;
                    while (node != null)
                    {
                        visitStack.Push(node);
                        node = node.Left;
                    }
                    if (c.ID >= startId)
                    {
                        current = c;
                        return true;
                    }
                }
                current = null;
                return false;
            }

            public void Reset()
            {
                visitStack.Clear();
                Node node = avl.mRoot;
                while(node != null)
                {
                    if(node.ID < startId)
                    {
                        node = node.Right;
                    }
                    else if(node.ID > startId)
                    {
                        visitStack.Push(node);
                        node = node.Left;
                    }
                    else
                    {
                        visitStack.Push(node);
                        break;
                    }
                }
            }
        }
    }
}