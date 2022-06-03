using System.Collections.Generic;
using UnityEngine;
namespace Sahil.AStar
{
	[System.Serializable]
	public class Grid_AStar:Grid<Cell_AStar>
	{
		LayerMask m_unWalkableMask;
		///<summary>
		/// GridOrigin:Bottom-Left cell's world position
		/// GridSize: Number of cells in each coordinate
		/// CellSize: Size of each cell block[Rounded]
		/// UnWalkableLayerMask: LayerMask that marks type of object that can't be walked over
		///</summary>
		///
		public Grid_AStar(Vector3 aGridOrigin, Vector2Int aGridSize, float aCellSize,LayerMask aUnWalkableLayerMask):base(aGridOrigin, aGridSize, aCellSize) { m_unWalkableMask = aUnWalkableLayerMask; }

		public override Cell_AStar[,] CreateGrid()
		{
			m_grid = new Cell_AStar[m_gridSize.x, m_gridSize.y];

			for (int x = 0; x < m_gridSize.x; x++)
			{
				for (int y = 0; y < m_gridSize.y; y++)
				{
					Vector3 wPosition = m_gridOrigin+new Vector3(x,0,y)*m_cellSize;
					m_grid[x, y]=new Cell_AStar();

					m_grid[x,y].Initialize(new Vector2Int(x, y), wPosition);
				}
			}
			return m_grid;
		}

		public List<Cell_AStar> GetNeighbours(Cell_AStar aCell)
        {
			List<Cell_AStar> neighbours = new List<Cell_AStar>();
			for(int x=-1;x<1;x++)
            {
				for(int y=-1;y<1;y++)
                {
					Cell_AStar cell=GetCellIfValid(new Vector2Int(x, y));
					if(cell!=null)
						neighbours.Add(cell);
                }
            }
			return neighbours;
        }
 
	}
}