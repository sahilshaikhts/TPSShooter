using System.Collections.Generic;
using UnityEngine;

namespace Sahil.AStar
{
    public class PathFinder
    {
        Grid_AStar m_grid;//instead of storing it,make this a static class ,make em pass the grid to the static function,and return list of positions.

        public PathFinder(Grid_AStar aGrid)
        {
            m_grid = aGrid;
        }

        public List<Cell_AStar> CalculatePath(Vector3 aOrigin, Vector3 aTargetPosition)
        {
            Cell_AStar originCell = m_grid.GetCellFromWorldPosition(aOrigin);
            Cell_AStar targetCell = m_grid.GetCellFromWorldPosition(aTargetPosition);

            List<Cell_AStar> vistiedCells = new List<Cell_AStar>();
            List<Cell_AStar> openCells = new List<Cell_AStar>();

            //if origin or target cell is not walkable return null
            if (targetCell.IsWalkable() == false)
            {
                List<Vector3> positions = GetWalkableNeighbourCellsPosition(targetCell.GetWorldPosition(), 2);
                float squaredDist = float.MaxValue;

                Vector3? nearestSpot = null;
                float nearestSpotDist = float.MaxValue;
                List<float> nearestSpotss = new List<float>();

                for (int i = 0; i < positions.Count; i++)
                {
                    if ((aTargetPosition - positions[i]).sqrMagnitude < nearestSpotDist)
                    {
                        nearestSpot = positions[i];
                        nearestSpotDist = (aTargetPosition - positions[i]).sqrMagnitude;
                    }
                }
                targetCell = m_grid.GetCellFromWorldPosition((Vector3)nearestSpot);
            }
            
            openCells.Add(originCell);

            while (openCells.Count > 0)
            {
                Cell_AStar currentCell = openCells[0];

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
                    return GetPath(originCell, currentCell);
                }

                foreach (Cell_AStar neighbour in m_grid.GetNeighbours(currentCell))
                {
                    if (neighbour.IsWalkable() == false || vistiedCells.Contains(neighbour)) continue;

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

        public Vector3 GetMoveDirection(Vector3 aOrigin, Vector3 aTargetPosition)
        {
            List<Cell_AStar> path=CalculatePath(aOrigin, aTargetPosition);

            if(path!=null && path.Count > 0)
            {
                Vector3 direction = path[0].GetWorldPosition() - aOrigin;
                return direction.normalized;
            }

            return Vector3.zero;
        }
        private List<Cell_AStar> GetPath(Cell_AStar aStartCell, Cell_AStar aEndCell)
        {
            List<Cell_AStar> path = new List<Cell_AStar>();
            Cell_AStar cellToCheck = aEndCell;

            while (cellToCheck!=aStartCell)
            {
                path.Add(cellToCheck);
                cellToCheck = cellToCheck.parent;
            }

            path.Reverse();
            return path;
        }

        private int GetDistance(Vector2Int aOrigin, Vector2Int aTarget)
        {
            int distX = Mathf.Abs(aOrigin.x - aTarget.x);
            int distY = Mathf.Abs(aOrigin.y - aTarget.y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            else
                return 14 * distX + 10 * (distY - distX);
        }

        public List<Vector3> GetNeighbourCellsPosition(Vector3 aCenter, int aCellRadius)
        {
            List<Vector3> neighbours = new List<Vector3>();
            List<Cell_AStar> neighbourCells = m_grid.GetNeighboursInGridRadius(m_grid.GetCellFromWorldPosition(aCenter), aCellRadius);

            foreach (Cell_AStar cell in neighbourCells)
            {
                neighbours.Add(cell.GetWorldPosition());
            }

            return neighbours;

        }

        public List<Vector3> GetWalkableNeighbourCellsPosition(Vector3 aCenter,int aCellRadius)
        {
            List<Vector3> neighbours=new List<Vector3>();
            List<Cell_AStar> neighbourCells= m_grid.GetNeighboursInGridRadius(m_grid.GetCellFromWorldPosition(aCenter),aCellRadius);

            foreach (Cell_AStar cell in neighbourCells)
            {
                if(cell.IsWalkable())
                neighbours.Add(cell.GetWorldPosition());
            }

            return neighbours;

        }

        public Cell_AStar GetCellFromWorldPos(Vector3 aPosition) { return m_grid.GetCellFromWorldPosition(aPosition); }

        public bool IsWalkable(Vector3 aWorldPosition)=>m_grid.GetCellFromWorldPosition(aWorldPosition).IsWalkable();

    }
}
