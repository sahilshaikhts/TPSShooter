using UnityEngine;
namespace Sahil
{
    public class Cell
    {
        protected Vector3 m_worldPosition;
        protected Vector2Int m_gridPosition;

        public virtual void Initialize(Vector2Int aGridPosition, Vector3 aWorldPosition)
        {
            m_gridPosition = aGridPosition;
            m_worldPosition = aWorldPosition;
        }

        public Vector3 GetWorldPosition() { return m_worldPosition; }
        public Vector2Int GetGridPosition() { return m_gridPosition; }
    }
}