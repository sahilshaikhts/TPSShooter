using System.Collections.Generic;
using UnityEngine;
namespace Sahil.AStar
{
	public class Grid_AStar:Grid<Cell_AStar>
	{
		///<summary>
		/// GridOrigin:Bottom-Left cell's world position
		/// GridSize: Number of cells in each coordinate
		/// CellSize: Size of each cell block[Rounded]
		///</summary>
		///
		public Grid_AStar(Vector3 aGridOrigin, Vector2Int aGridSize, float aCellSize):base(aGridOrigin, aGridSize, aCellSize) { }

		public override Cell_AStar[,] CreateGrid()
		{
			grid = new Cell_AStar[m_gridSize.x, m_gridSize.y];

			for (int x = 0; x < m_gridSize.x; x++)
			{
				for (int y = 0; y < m_gridSize.y; y++)
				{
					Vector3 wPosition = m_gridOrigin+new Vector3(x,0,y)*m_cellSize;
					grid[x, y]=new Cell_AStar();

					grid[x,y].Initialize(new Vector2Int(x, y), wPosition);
				}
			}
			return grid;
		}

		public List<Cell_AStar> GetNeighbours(Cell_AStar aCell)
        {
			List<Cell_AStar> neighbours = new List<Cell_AStar>();

			return neighbours;
        }
 
	}
}