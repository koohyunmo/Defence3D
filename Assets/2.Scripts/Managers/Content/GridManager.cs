using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    Grid grid = null;

    public void CreateGrid(int width, int height, float cellSize, Vector3 planePosition)
    {
        grid = new Grid(width, height, cellSize, planePosition);
    }

    public void RemoveGridObject(Vector3 worldPosition)
    {
        grid.Remove(worldPosition);
    }

    public bool AddGridObject()
    {
        return grid.AddUnit();
    }

    public bool Swap(Vector3 worldPosition, Vector3 originalPos)
    {
        return grid.Swap(worldPosition,originalPos);
    }

    public Grid GetGrid()
    {
        return grid;
    }
}
