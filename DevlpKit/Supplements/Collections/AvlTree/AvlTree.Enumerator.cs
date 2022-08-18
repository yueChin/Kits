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
        // 迭代器
        internal class Enumerator : IEnumerator<T>
        {
            AvlTree<T> avl;
            Node current;
            Stack<Node> visitStack; //

            public T Current { get { return current == null ? default(T) : current.Value; } }

            object IEnumerator.Current { get { return Current; } }

            public Enumerator(AvlTree<T> avl)
            {
                this.avl = avl;
                if (avl != null)
                    visitStack = new Stack<Node>(avl.Deep);
                Reset();
            }

            public void Dispose()
            {
                avl = null;
                current = null;
            }

            public bool MoveNext()
            {
                if (visitStack == null || visitStack.Count == 0)
                    return false;
                current = visitStack.Pop();
                if (current != null)
                {
                    Node node = current.Right;
                    while (node != null)
                    {
                        visitStack.Push(node);
                        node = node.Left;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                if (visitStack != null)
                {
                    visitStack.Clear();
                    Node node = avl.mRoot;
                    while (node != null)
                    {
                        visitStack.Push(node);
                        node = node.Left;
                    }
                }
            }
        }
    }
}