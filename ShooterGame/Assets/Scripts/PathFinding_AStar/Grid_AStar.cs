using System.Collections.Generic;
using UnityEngine;
namespace Sahil.AStar
{
	[System.Serializable]
	public class Grid_AStar:Grid<Cell_AStar>
	{
		[SerializeField]LayerMask m_unWalkableMask;

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
					bool walkable=!Physics.CheckSphere(wPosition,m_cellSize/2,m_unWalkableMask);
					m_grid[x,y].Initialize(new Vector2Int(x, y), wPosition, walkable);
				}
			}
			return m_grid;
		}


		public List<Cell_AStar> GetCellsInADirection(Vector3 aWorldPosition, Vector3 aDirection, int aCount)
		{
			List<Cell_AStar> cells = new List<Cell_AStar>();

			//Get the exact center world position of the the origin cell for better accuracy.
			Vector3 originCellWorldPos = GetCellFromWorldPosition(aWorldPosition).GetWorldPosition();

			Cell_AStar cell;
			for (int i = 1; i <= aCount; i++)
			{
				cell = GetCellFromWorldPosition(originCellWorldPos + aDirection * i* m_cellSize);
				if(cell != null)
					cells.Add(cell);
			}

			return cells;
		}

		public bool CheckIfCellExits(Vector3 aWPosition)=>GetCellFromWorldPosition(aWPosition)==null?false:true;

	}
}