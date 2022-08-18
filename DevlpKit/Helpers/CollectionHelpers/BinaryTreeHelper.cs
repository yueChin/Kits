using System.Collections.Generic;
using Kits.DevlpKit.Supplements.Collections;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static class BinaryTreeHelper
    {
        public static IList<int> InOrderTraversal(BinaryTreeNode root, IList<int> outList = null)
        {
            if (outList == null)
                outList = new List<int>();
            if (root == null)
                return outList;
            Stack<BinaryTreeNode> stack = new Stack<BinaryTreeNode>();
            while (stack.Count > 0 || root != null)
            {
                while (root != null)
                {
                    stack.Push(root);
                    root = root.Left;
                }

                root = stack.Pop();
                outList.Add(root.Value);
                root = root.Right;
            }

            return outList;
        }

        public static IList<int> PreOrderTraversal(BinaryTreeNode root, IList<int> outList = null)
        {
            if (outList == null)
                outList = new List<int>();
            if (root == null)
                return outList;

            Stack<BinaryTreeNode> stack = new Stack<BinaryTreeNode>();
            stack.Push(root);

            while (stack.Count != 0)
            {
                BinaryTreeNode node = stack.Pop();
                outList.Add(node.Value);

                if (node.Right != null)
                    stack.Push(node.Right);
                if (node.Left != null)
                    stack.Push(node.Left);
            }

            return outList;
        }

        public static IList<int> PostOrderTraversal(BinaryTreeNode root, IList<int> outList = null)
        {
            if (outList == null)
                outList = new List<int>();
            if (root == null)
                return outList;

            Stack<BinaryTreeNode> stack = new Stack<BinaryTreeNode>();
            BinaryTreeNode node = root;
            BinaryTreeNode lastVisit = root;
            while (node != null || stack.Count == 0)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.Left;
                }

                //查看当前栈顶元素
                node = stack.Peek();
                //如果其右子树也为空，或者右子树已经访问
                //则可以直接输出当前节点的值
                if (node.Right == null || node.Right == lastVisit)
                {
                    outList.Add(node.Value);
                    stack.Pop();
                    lastVisit = node;
                    node = null;
                }
                else
                {
                    //否则，继续遍历右子树
                    node = node.Right;
                }
            }

            return outList;
        }

        public static IList<int> LevelOrderTraversal(BinaryTreeNode root, IList<int> outList = null)
        {
            if (outList == null)
                outList = new List<int>();
            if (root == null)
                return outList;
            Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                BinaryTreeNode node = queue.Dequeue();
                if (node.Left != null)
                    queue.Enqueue(node.Left);
                if (node.Right != null)
                    queue.Enqueue(node.Right);
                outList.Add(node.Value);
            }

            return outList;
        }
    }
}
