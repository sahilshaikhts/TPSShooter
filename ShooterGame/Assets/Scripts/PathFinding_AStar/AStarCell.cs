using UnityEngine;

    public class Cell
    {
        public Cell parent=null;

        Vector3 m_worldPosition;
        Vector2Int m_gridPosition;

        public int HCost, GCost;
        public int FCost { get { return HCost + GCost; } }
        bool m_isWalkable;

        public void Initialize(Vector2Int aGridPosition, Vector3 aWorldPosition,bool aIsWalkable)
        {
            m_gridPosition = aGridPosition;
            m_worldPosition = aWorldPosition;
            m_isWalkable = aIsWalkable;
        }

        public Vector3 GetWorldPosition() { return m_worldPosition; }
        public Vector2Int GetGridPosition() { return m_gridPosition; }
        public bool IsWalkable() { return true; }
    }
