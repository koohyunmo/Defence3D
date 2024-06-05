using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private GridObject[,] gridObj;
    private Vector3 originPosition;
    private GameObject root = null;
    private Stack<int> indexStack  = new();

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        root = new GameObject(name: "GridRoot");

        gridArray = new int[width, height];
        gridObj = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                indexStack.Push(GetIndex(x,z));
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
    }

    /// <summary>
    /// 그리드 오브젝트 스왑 or 위치 옮기기
    /// @detail 움직일 칸이 빈칸인경우(move) + 움직일 칸이 빈칸이 아닌경우(swap)
    /// @bug index 관리해줘여함
    /// @version 24-06-05 v01
    /// </summary>
    /// <param name="targetPos">현재 드래그중인 오브젝트의 그리드 위치</param>
    /// <param name="selectedObj">현재 드래그중인 오브젝트</param>
    /// <returns></returns>
    public bool Swap(Vector3 targetPos,Vector3 originalPos)
    {
        int tX = -1, tZ = -1;
        GetXZ(targetPos, out tX, out tZ);
        if (OOB(tX, tZ)) return false;

        int sX = -1, sZ = -1;
        GetXZ(originalPos, out sX, out sZ);
        if (OOB(sX, sZ)) return false;

        var targetObj = gridObj[tX, tZ];
        var sObj = gridObj[sX, sZ];
        if(sObj == null)
        {
            Debug.LogError("sObj is null");
            return false;
        }

        // 스왑
        if (targetObj)
        {
            gridObj[sX,sZ].transform.position = GetGridPosition(tX,tZ);
            gridObj[tX,tZ].transform.position = GetGridPosition(sX,sZ);

            gridObj[sX, sZ] = targetObj;
            gridObj[tX, tZ] = sObj;
        }
        // Move
        else
        {
            sObj.transform.position = GetGridPosition(tX, tZ);
            gridObj[tX, tZ] = sObj;
            gridObj[sX, sZ] = null;
            indexStack.Push(GetIndex(sX,sZ));
        }

        return true;
    }

    public bool AddUnit()
    {
        while (indexStack.Count > 0)
        {
            int curIndex = indexStack.Pop();
            (int x, int z) xz = IndexToXZ(curIndex);

            int x = xz.Item1;
            int z = xz.Item2;

            if (OOB(x, z)) return false; // 밤위 밖 탈출
            if (gridObj[x, z]) continue; 

            SetValue(x, z, 1);
            CreateUnit(x, z);
            return true;
        }

        return false; 
    }


    public bool Remove(Vector3 worldPosition)
    {
        int x = -1, z = -1;
        GetXZ(worldPosition, out x, out z);
        if(OOB(x,z)) return false;
        if(gridObj[x,z] == null) return false;

        GameObject.Destroy(gridObj[x, z].gameObject);
        gridObj[x, z] = null;
        indexStack.Push(GetIndex(x,z));
        SetValue(x, z, 0);
        return true;
    }

    private void CreateUnit(int x, int z)
    {
        var prefab = Resources.Load<GameObject>("Cube");

        Vector3 newPos = GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f;
        var go = GameObject.Instantiate(prefab, root.transform);
        go.transform.position = newPos;

        gridObj[x, z] = go.GetComponent<GridObject>();
        gridObj[x, z].name = $"{x},{z} : {z + x * height}";
        SetValue(x,z,1);
    }

    private Vector3 GetGridPosition(int x, int z)
    {
        return GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f;
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void SetValue(int x, int z, int value)
    {
        if (OOB(x, z)) return;

        gridArray[x, z] = value;
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x = -1, z = -1;
        GetXZ(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    private void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }
    public int GetIndex(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetIndex(x,z);
    }
    public int GetIndex(int x, int z)
    {
        return z + x * height;
    }
    public (int, int) IndexToXZ(int index)
    {
        int x = index / height;
        int z = index % height;
        return (x,z);
    }

    public int GetValue(int x, int z)
    {
        if (OOB(x, z)) return -1;
        else
        {
            return gridArray[x, z];
        }
    }

    private bool OOB(int x, int z)
    {
        return x < 0 || x >= width || z < 0 || z >= height;
    }
}
