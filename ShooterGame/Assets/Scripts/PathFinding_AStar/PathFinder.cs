using System.Collections.Generic;
using UnityEngine;

namespace Sahil.AStar
{
    public class PathFinder
    {
        AStar_Grid m_grid;//instead of storing it,make this a static class ,make em pass the grid to the static function,and return list of positions.

        Vector3[] m_cachedPath;
        List<Vector3> m_nextPath;

        public PathFinder(AStar_Grid aGrid)
        {
            m_grid = aGrid;
        }

        public List<Cell> CalculatePath(Vector3 aOrigin, Vector3 aTargetPosition)
        {
            Cell originCell = m_grid.NodeFromWorldPoint(aOrigin);
            Cell targetCell = m_grid.NodeFromWorldPoint(aTargetPosition);

            HashSet<Cell> vistiedCells = new HashSet<Cell>();
            List<Cell> openCells = new List<Cell>();

            m_nextPath = new List<Vector3>();

            openCells.Add(originCell);
            while (openCells.Count > 0)
            {
                Cell currentCell = openCells[0];

                for (int i = 1; i < openCells.Count; i++)
                {
                    if (openCells[i].FCost < currentCell.FCost || openCells[i].FCost == currentCell.FCost && openCells[i].HCost < currentCell.HCost)
                    {
                        currentCell = openCells[i];
                    }
                }

                vistiedCells.Add(currentCell);
                openCells.Remove(currentCell);

                if (currentCell == targetCell)
                {
                    return GetPath(currentCell, originCell);
                }

                foreach (Cell neighbour in m_grid.GetNeighbours(currentCell))
                {
                    if (neighbour.IsWalkable() == false && vistiedCells.Contains(neighbour)) continue;

                    int newCostToNeighbour = currentCell.GCost + GetDistance(currentCell.GetGridPosition(), neighbour.GetGridPosition());

                    if (newCostToNeighbour < neighbour.GCost || !openCells.Contains(neighbour))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour.GetGridPosition(), targetCell.GetGridPosition());
                        neighbour.parent = currentCell;

                        if (!openCells.Contains(neighbour))
                            openCells.Add(neighbour);
                    }
                }
            }
            return null;
        }

        List<Cell> GetPath(Cell aStartCell, Cell aEndCell)
        {
            List<Cell> path = new List<Cell>();
            Cell cellToCheck = aEndCell;

            while (cellToCheck != aStartCell)
            {
                path.Add(cellToCheck);
                if (cellToCheck.parent == null)
                {
                    Debug.Log("");
                }
                cellToCheck = cellToCheck.parent;
            }

            path.Reverse();
            return path;
        }

        public int GetDistance(Vector2Int aOrigin, Vector2Int aTarget)
        {
            int distX = Mathf.Abs(aOrigin.x - aTarget.x);
            int distY = Mathf.Abs(aOrigin.y - aTarget.y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            else
                return 14 * distX + 10 * (distY - distX);
        }
    }
}
