using System.Collections.Generic;
using UnityEngine;
namespace Sahil
{
	[System.Serializable]
	public class Grid<T> where T :Cell , new()
	{
		public T[,] m_grid;

		[SerializeField] protected Vector3 m_gridOrigin;
		[SerializeField] protected Vector2Int m_gridSize;

		[SerializeField] protected float m_cellSize;


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
			m_grid = new T[m_gridSize.x, m_gridSize.y];

			for (int x = 0; x < m_gridSize.x; x++)
			{
				for (int y = 0; y < m_gridSize.y; y++)
				{
					Vector3 wPosition = m_gridOrigin+new Vector3(x,0,y)*m_cellSize;
					m_grid[x, y]=new T();

					m_grid[x,y].Initialize(new Vector2Int(x, y), wPosition);
				}
			}
			return m_grid;
		}
 
		public T GetCellFromWorldPosition(Vector3 aWorldPosition)
		{
			Vector3 localPos=new Vector3(aWorldPosition.x,aWorldPosition.y,aWorldPosition.z) - m_gridOrigin;
			return m_grid[Mathf.RoundToInt(localPos.x / m_cellSize), Mathf.RoundToInt(localPos.z / m_cellSize)];

		}

		protected T GetCellIfValid(Vector2Int aGridPosition)
        {
			if (aGridPosition.x < m_gridSize.x && aGridPosition.y < m_gridSize.y && aGridPosition.x >=0 && aGridPosition.y >=0)
			{
				return m_grid[aGridPosition.x, aGridPosition.y];
			}
			else return null;
        }

		public List<T> GetNeighbours(T aCell)
		{
			List<T> neighbours = new List<T>();
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0) continue;

					T cell = GetCellIfValid(aCell.GetGridPosition() + new Vector2Int(x, y));

					if (cell != null)
						neighbours.Add(cell);
				}
			}
			return neighbours;
		}

		public List<T> GetNeighboursInGridRadius(T aCell,int aCellRadius)
		{
			List<T> neighbours = new List<T>();
			for (int x = -aCellRadius; x <= aCellRadius; x++)
			{
				for (int y = -aCellRadius; y <= aCellRadius; y++)
				{
					if (x == 0 && y == 0) continue;

					T cell = GetCellIfValid(aCell.GetGridPosition() + new Vector2Int(x, y));

					if (cell != null)
						neighbours.Add(cell);
				}
			}
			return neighbours;
		}
	}
}