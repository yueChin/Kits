using System.Collections.Generic;
using UnityEngine;

namespace Kits.DevlpKit.Supplements.Collections
{
    public enum Axis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 3,
    }

	public class BspSpace<T>
	{
        public class Space
        {
            public Axis SplitAxis { get; private set; }
            public Bounds SpaceBounds { get; private set; }
            private HashSet<T> m_Datas;

            public Space() { }

            public Space(Bounds bounds)
            {
                SpaceBounds = bounds;
            }

            // 获取空间方向 （-1，0，1）
            public int GetSide(Vector3 point)
            {
                if (SplitAxis == Axis.X)
                    return point.x <= SpaceBounds.center.x ? -1 : 1;
                else if (SplitAxis == Axis.Y)
                    return point.y <= SpaceBounds.center.y ? -1 : 1;
                else if (SplitAxis == Axis.Z)
                    return point.z <= SpaceBounds.center.z ? -1 : 1;
                else
                    return 0;
            }

            // 分割
            public void Split(Axis normalAxis, out Space left, out Space right)
            {
                Vector3 center = SpaceBounds.center;
                Vector3 size = SpaceBounds.size;
                Bounds bounds;
                if(normalAxis == Axis.X)
                {
                    SplitAxis = Axis.X;
                    size.x *= 0.5f;

                    center.x -= SpaceBounds.size.x * 0.25f;
                    bounds = new Bounds(center, size);
                    left = new Space(bounds);

                    center.x += SpaceBounds.size.x * 0.5f;
                    bounds = new Bounds(center, size);
                    right = new Space(bounds);
                }
                else if(normalAxis == Axis.Y)
                {
                    SplitAxis = Axis.Y;
                    size.y *= 0.5f;

                    center.y -= SpaceBounds.size.y * 0.25f;
                    bounds = new Bounds(center, size);
                    left = new Space(bounds);

                    center.y += SpaceBounds.size.y * 0.5f;
                    bounds = new Bounds(center, size);
                    right = new Space(bounds);
                }
                else if(normalAxis == Axis.Z)
                {
                    SplitAxis = Axis.Z;
                    size.z *= 0.5f;

                    center.z -= SpaceBounds.size.z * 0.25f;
                    bounds = new Bounds(center, size);
                    left = new Space(bounds);

                    center.z += SpaceBounds.size.z * 0.5f;
                    bounds = new Bounds(center, size);
                    right = new Space(bounds);
                }
                else
                {
                    left = null;
                    right = null;
                }
            }

            public bool Add(T data)
            {
                if (m_Datas == null)
                    m_Datas = new HashSet<T>();
                return m_Datas.Add(data);
            }

            public bool Remove(T data)
            {
                if (m_Datas != null)
                    return m_Datas.Remove(data);
                else
                    return false;
            }
        }

        Space[] m_Spaces;
        public int SpaceCount { get { return m_Spaces.Length; } }
        public Space SpaceAt(int index)
        {
            return m_Spaces[index];
        }

        public static int CalculateSpaceArraySize(int splitCount)
        {
            int i = splitCount + 1;
            return (1 << i) - 1;
        }

        public BspSpace(Bounds bounds, int splitCount, Axis[] splitOrder)
        {
            int n = Mathf.Max(0, splitCount);
            m_Spaces = new Space[CalculateSpaceArraySize(n)];
            m_Spaces[0] = new Space(bounds);
            Space left;
            Space right;
            
            int child;
            int axisIndex = 0;
            int axisP = 1;
            int floor = 0;
            for (int i = 0; i < m_Spaces.Length; i++)
            {
                if (axisP == i)
                {
                    floor++;
                    axisIndex = (axisIndex + 1) % splitOrder.Length;
                    axisP += (1 << floor);
                }
                child = GetChildIndex(i);
                if (child < m_Spaces.Length)
                {
                    m_Spaces[i].Split(splitOrder[axisIndex], out left, out right);
                    m_Spaces[child] = left;
                    m_Spaces[child + 1] = right;
                }
                else
                {
                    break;
                }
               
            }
        }

        int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) >> 1;
        }

        int GetChildIndex(int parentIndex)
        {
            return (parentIndex << 1) + 1;
        }

        public Space FindSpace(Vector3 position)
        {
            int p = 0;
            Space space = m_Spaces[p];
            if (!space.SpaceBounds.Contains(position))
                return null;
            int child;
            while (p < m_Spaces.Length)
            {
                int lr = space.GetSide(position);
                if (lr == 0)
                    break;
                else if (lr < 0)
                    child = GetChildIndex(p);
                else
                    child = GetChildIndex(p) + 1;
                p = child;
                if (p >= m_Spaces.Length)
                    break;
                space = m_Spaces[p];
            }
            return space;
        }

        public bool AddData(Vector3 position, T data)
        {
            Space space = FindSpace(position);
            if (space == null)
                return false;
            else
                return space.Add(data);
        }

        public bool RemoveData(T data, Vector3 position)
        {
            Space space = FindSpace(position);
            if (space == null)
                return false;
            return space.Remove(data);
        }

        public void UpdateData(T data, Vector3 oldPosition, Vector3 newPosition)
        {
            Space space = FindSpace(oldPosition);
            Space newspace = FindSpace(newPosition);
            if (space == newspace)
                return;
            space.Remove(data);
            newspace.Add(data);
        }
	}
}