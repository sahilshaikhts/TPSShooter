using UnityEngine;
namespace Sahil.AStar
{
    public class Cell_AStar : Cell
    {
        public Cell_AStar parent = null;

        public int HCost, GCost;
        public int FCost { get { return HCost + GCost; } }
        bool m_isWalkable;

        public void Initialize(Vector2Int aGridPosition, Vector3 aWorldPosition, bool aIsWalkable)
        {
            m_gridPosition = aGridPosition;
            m_worldPosition = aWorldPosition;
            m_isWalkable = aIsWalkable;
        }

        public bool IsWalkable() { return m_isWalkable; }
    }
}
