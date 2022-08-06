using UnityEngine;
namespace Sahil.AStar
{
    public class Cell_AStar : Cell,IHeapItem<Cell_AStar>
    {
        public Cell_AStar parent = null;

        public int HCost, GCost;
        public int FCost { get { return HCost + GCost; } }
        public int heapIndex;
        public int index { get { return heapIndex; } set { heapIndex = value; } }

        bool m_isWalkable;

        public void Initialize(Vector2Int aGridPosition, Vector3 aWorldPosition, bool aIsWalkable)
        {
            m_gridPosition = aGridPosition;
            m_worldPosition = aWorldPosition;
            m_isWalkable = aIsWalkable;
        }

        public bool IsWalkable() { return m_isWalkable; }

        public int CompareTo(Cell_AStar other)
        {
            int priority=FCost.CompareTo(other.FCost);
            if (priority == 0)
                priority=HCost.CompareTo(other.HCost);

            return -priority;
        }
    }
}
