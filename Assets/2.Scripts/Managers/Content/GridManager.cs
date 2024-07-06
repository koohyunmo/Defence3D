using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class GridManager
{
    private GridSystem grid = null;
    private Vector3[] corners = new Vector3[4];

    public void CreateGrid(int width, int height, float cellSize, Vector3 planePosition)
    {
        grid = new GridSystem(width, height, cellSize, planePosition);
    }
    public void CreateGrid(int width, int height, float cellSizeX,float cellSizeZ, Vector3 planePosition)
    {
        grid = new GridSystem(width, height, cellSizeX, cellSizeZ, planePosition);
    }

    public int GetGridSize()
    {
        return grid.GetGridSize();
    }

    public void RemoveGridObject(Vector3 worldPosition)
    {
        grid.Remove(worldPosition);
    }

    public bool AddGridObject()
    {
        return grid.AddUnit();
    }

    public bool AddGridObject(UnitGrade grade)
    {
        return grid.AddUnit(grade);
    }


    public bool Swap(Vector3 worldPosition, Vector3 originalPos)
    {
        return grid.Swap(worldPosition,originalPos);
    }

    public GridSystem GetGrid()
    {
        return grid;
    }

    public void SetPath(Vector3[] path)
    {
        corners = path;
    }

    public Vector3[] GetPath()
    {
        return corners;
    }
    public Vector3 GetPath(int index)
    {
        return corners[index % corners.Length];
    }
}
