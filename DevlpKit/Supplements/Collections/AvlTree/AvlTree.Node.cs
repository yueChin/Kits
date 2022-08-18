using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections.AvlTree
{
    /// <summary>
    /// 平衡二叉树，默认通过 Hash 值作为比较 id，也可通过一个 IdentifierDelegate 的委托类型实现比较值的覆盖方法
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed partial class AvlTree<T> : ICollection<T>
    {
        // 平衡树节点实例
        internal class Node
        {
#if NODE_POOL
            private static Node cachedNode = null; // cache0 <-- cache1 <-- cache2 <-- cache3 <-- cachedNode
#endif
            public static Node GetNode(int id, T value)
            {
#if NODE_POOL
                if(cachedNode != null)
                {
                    var node = cachedNode;
                    cachedNode = cachedNode.parent;
                    node.id = id;
                    node.value = value;
                    node.parent = null;
                    return node;
                }
                else
#endif
                    return new Node(id, value);
            }

            public static void CacheNode(Node node)
            {
#if NODE_POOL
                node.Release();
                node.parent = cachedNode;
                cachedNode = node;
#endif
            }

            public T Value;
            public Node Left;
            public Node Right;
            public Node Parent;

            public int ID; // id
            public int Deep; // 深度
            public int Count; // 节点数

            private Node(int id, T value)
            {
                this.Value = value;
                this.ID = id;
                Deep = 1;
                Count = 1;
            }

            public int BalenceFactor
            {
                get
                {
                    int l = Left?.Deep ?? 0;
                    int r = Right?.Deep ?? 0;
                    return l - r;
                }
            }

            public int Index
            {
                get
                {
                    int index = Left?.Count ?? 0;
                    Node node = this;
                    Node par = Parent;
                    while(par != null)
                    {
                        if (par.Right == node)
                        {
                            index += 1;
                            if (par.Left != null)
                                index += par.Left.Count;
                        }
                        node = par;
                        par = node.Parent;
                    }
                    return index;
                }
            }

            public Node Max
            {
                get
                {
                    Node node = this;
                    while (node.Right != null)
                        node = node.Right;
                    return node;
                }
            }

            public Node Min
            {
                get
                {
                    Node node = this;
                    while (node.Left != null)
                        node = node.Left;
                    return node;
                }
            }

            public void Release()
            {
                Left = null;
                Right = null;
                Parent = null;
                ID = 0;
                Value = default(T);
                Deep = 0;
                Count = 0;
            }

            public void FixBalence()
            {
                Node node = this;
                while (node != null)
                {
                    int balence = node.BalenceFactor;
                    // 左旋转
                    if (balence < -1)
                    {
                        Node r = node.Right;
                        if (r.BalenceFactor > 0)
                        {
                            r.RotateRight();
                            r.UpdateSelf();
                        }
                        node.RotateLeft();
                        node.UpdateTree();
                        break;
                    }
                    else if (balence > 1)
                    {
                        Node l = node.Left;
                        if (l.BalenceFactor < 0)
                        {
                            l.RotateLeft();
                            l.UpdateSelf();
                        }
                        node.RotateRight();
                        node.UpdateTree();
                        break;
                    }
                    else
                    {
                        node = node.Parent;
                    }
                }
            }
            
            public void AddLeftLeaf(Node newnode)
            {
                Left = newnode;
                newnode.Parent = this;
            }
            
            public void AddRightLeaf(Node newnode)
            {
                Right = newnode;
                newnode.Parent = this;
            }

            public void UpdateSelf()
            {
                Deep = Math.Max(Left?.Deep ?? 0, Right?.Deep ?? 0) + 1;
                Count = (Left?.Count ?? 0) + (Right?.Count ?? 0) + 1;
            }

            public void UpdateTree()
            {
                Node p = this;
                while (p != null)
                {
                    p.UpdateSelf();
                    p = p.Parent;
                }
            }

            // 左旋转
            public void RotateLeft()
            {
                Node p = Parent;
                Node rl = Right.Left;
                Right.Parent = p;
                if (p != null)
                {
                    if (p.Left == this)
                        p.Left = Right;
                    else
                        p.Right = Right;
                }
                Parent = Right;
                Right.Left = this;
                Right = rl;
                if (rl != null)
                    rl.Parent = this;
            }

            // 右旋转
            public void RotateRight()
            {
                Node p = Parent;
                Node lr = Left.Right;
                Left.Parent = p;
                if (p != null)
                {
                    if (p.Left == this)
                        p.Left = Left;
                    else
                        p.Right = Left;
                }
                Parent = Left;
                Left.Right = this;
                Left = lr;
                if (lr != null)
                    lr.Parent = this;
            }

            public Node RemoveAndReturnDirty()
            {
                Node par = Parent;
                Node l = Left;
                Node r = Right;
                bool isleft = false;
                if(par != null)
                {
                    isleft = par.Left == this;
                    if (isleft)
                        par.Left = null;
                    else
                        par.Right = null;
                }
                if (Left != null)
                    Left.Parent = null;
                if (Right != null)
                    Right.Parent = null;
                Parent = null;
                Left = null;
                Right = null;
                if (l == null && r == null)
                    return par;
                if(l == null)
                {
                    UseParent(r, par, isleft);
                    return par ?? r;
                }
                if(r == null)
                {
                    UseParent(l, par, isleft);
                    return par ?? l;
                }
                if(l.Deep < r.Deep)
                {
                    Node t = r.Min;
                    if(t == r)
                    {
                        r.Left = l;
                        l.Parent = r;
                        UseParent(r, par, isleft);
                        return r;
                    }
                    Node su = t.Parent;
                    Node tr = t.Right;
                    su.Left = tr;
                    if (tr != null)
                        tr.Parent = su;
                    t.Left = l;
                    l.Parent = t;
                    t.Right = r;
                    r.Parent = t;
                    UseParent(t, par, isleft);
                    return su;
                }
                else
                {
                    Node t = l.Max;
                    if(t == l)
                    {
                        l.Right = r;
                        r.Parent = l;
                        UseParent(l, par, isleft);
                        return l;
                    }
                    Node su = t.Parent;
                    Node tl = t.Left;
                    su.Right = tl;
                    if (tl != null)
                        tl.Parent = su;
                    t.Left = l;
                    l.Parent = t;
                    t.Right = r;
                    r.Parent = t;
                    UseParent(t, par, isleft);
                    return su;
                }
            }

            private void UseParent(Node asChild, Node asParent, bool isLeft)
            {
                asChild.Parent = asParent;
                if (asParent != null)
                {
                    if (isLeft)
                        asParent.Left = asChild;
                    else
                        asParent.Right = asChild;
                }
            }

#if TEST
            public override string ToString()
            {
                return string.Format("Avl[P:{0}\t L:{1}\t R:{2}\t Dep:{3}\t Bal:{4}\t Num:{5}]\t  \"{6}\"",
                    parent == null ? "-" : parent.id.ToString(),
                    left == null ? "-" : left.id.ToString(),
                    right == null ? "-" : right.id.ToString(),
                    deep.ToString(),
                    BalenceFactor.ToString(),
                    count.ToString(),
                    value.ToString());
            }
#endif

        }

    }
}