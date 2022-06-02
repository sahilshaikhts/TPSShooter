using UnityEngine;
namespace Sahil
{
	public class Grid<T> where T :Cell , new()
	{
		protected T[,] grid;

		[SerializeField] protected Vector3 m_gridOrigin;
		[SerializeField] protected float m_cellSize;

		protected Vector2Int m_gridSize;

		///<summary>
		/// GridOrigin:Bottom-Left cell's world position
		/// GridSize: Number of cells in each coordinate
		/// CellSize: Size of each cell block[Rounded]
		///</summary>
		///
		public Grid(Vector3 aGridOrigin, Vector2Int aGridSize, float aCellSize)
		{
			m_gridOrigin = aGridOrigin;
			m_cellSize = Mathf.Round(aCellSize);
			m_gridSize = aGridSize;
		}

		public virtual T[,] CreateGrid()
		{
			grid = new T[m_gridSize.x, m_gridSize.y];

			for (int x = 0; x < m_gridSize.x; x++)
			{
				for (int y = 0; y < m_gridSize.y; y++)
				{
					Vector3 wPosition = m_gridOrigin+new Vector3(x,0,y)*m_cellSize;
					grid[x, y]=new T();

					grid[x,y].Initialize(new Vector2Int(x, y), wPosition);
				}
			}
			return grid;
		}
 
		public T GetCellFromWorldPosition(Vector3 aWorldPosition)
		{
			Vector3 localPos=new Vector3(aWorldPosition.x,aWorldPosition.y,aWorldPosition.z)- m_gridOrigin;
			//return grid[x, y];
			return null;
		}

	}
}