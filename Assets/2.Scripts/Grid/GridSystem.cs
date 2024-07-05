using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class GridSystem
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSizeX;
    private float cellSizeZ;
    private GridObject[,] gridObj;
    private GridCell[,] gridCell;
    private Vector3 originPosition;
    private GameObject root = null;
    private GameObject cellRoot = null;
    private Queue<int> indexStack = new();
    private GridCell selectedCell = null;

    public GridSystem(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSize;
        this.cellSizeZ = cellSize;
        this.originPosition = originPosition;
        root = new GameObject(name: "GridRoot");
        cellRoot = new GameObject(name: "GridCellRoot");

        gridArray = new int[width, height];
        gridObj = new GridObject[width, height];
        gridCell = new GridCell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                indexStack.Enqueue(GetIndex(x, z));
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
        MakeCell();
    }

    public GridSystem(int width, int height, float cellSizeX, float cellSizeZ, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSizeX;
        this.cellSizeZ = cellSizeZ;
        this.originPosition = originPosition;
        root = new GameObject(name: "GridRoot");
        cellRoot = new GameObject(name: "GridCellRoot");

        gridArray = new int[width, height];
        gridObj = new GridObject[width, height];
        gridCell = new GridCell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                indexStack.Enqueue(GetIndex(x, z));
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);

            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
        MakeCell();
    }

    private void MakeCell()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                var cell = Managers.Resource.Instantiate("GridCell");
                cell.transform.position = GetGridCellPosition(x, z, 0.25f);
                cell.transform.SetParent(cellRoot.transform);
                gridCell[x, z] = cell.GetComponent<GridCell>();
            }
        }

        cellRoot.SetActive(false);
    }

    /*-----------------
    Grid Unit 관련 함수
    ------------------*/

    /// <summary>
    /// 그리드 오브젝트 스왑 or 위치 옮기기 + 강화
    /// @detail 움직일 칸이 빈칸인경우(move) + 움직일 칸이 빈칸이 아닌경우(swap) + 같은 무기일경우 강화
    /// @version 24-06-27 v02
    /// </summary>
    /// <param name="targetPos">현재 드래그중인 오브젝트의 그리드 위치</param>
    /// <param name="selectedObj">현재 드래그중인 오브젝트</param>
    /// <returns></returns>
    public bool Swap(Vector3 targetPos, Vector3 originalPos)
    {
        int tX = -1, tZ = -1;
        GetXZ(targetPos, out tX, out tZ);
        if (OutOfBounds(tX, tZ)) return false;

        int sX = -1, sZ = -1;
        GetXZ(originalPos, out sX, out sZ);
        if (OutOfBounds(sX, sZ)) return false;

        var targetObj = gridObj[tX, tZ];
        var selectedObj = gridObj[sX, sZ];
        if (selectedObj == null)
        {
            Debug.LogError("selectedObj is null");
            return false;
        }

        Weapon targetWeapon = null;
        Weapon selectedWeapon = null;
        if (targetObj)
        {
            targetWeapon = targetObj.GetComponent<Weapon>();
            selectedWeapon = selectedObj.GetComponent<Weapon>();
        }

        // 강화
        if (targetWeapon && selectedWeapon &&
            targetWeapon.GetWeaponData().Equals(selectedWeapon.GetWeaponData()))
        {
            Managers.Reinforce.Reinforce(selectedWeapon, targetWeapon);
            Remove(originalPos);
            return false;
        }

        // 스왑
        if (targetObj)
        {
            gridObj[sX, sZ].transform.position = GetGridCellPosition(tX, tZ,2);
            gridObj[tX, tZ].transform.position = GetGridCellPosition(sX, sZ,2);

            gridObj[sX, sZ] = targetObj;
            gridObj[tX, tZ] = selectedObj;
        }
        // Move
        else
        {
            selectedObj.transform.position = GetGridCellPosition(tX, tZ,2);
            gridObj[tX, tZ] = selectedObj;
            gridObj[sX, sZ] = null;
            indexStack.Enqueue(GetIndex(sX, sZ));

            gridCell[tX, tZ].TakeUpCell();
            gridCell[sX, sZ].ResetCell();
        }

        return true;
    }

    public bool AddUnit()
    {
        while (indexStack.Count > 0)
        {
            int curIndex = indexStack.Dequeue();
            (int x, int z) xz = IndexToXZ(curIndex);

            int x = xz.Item1;
            int z = xz.Item2;

            if (OutOfBounds(x, z)) return false; // 밤위 밖 탈출
            if (gridObj[x, z]) continue;

            SetValue(x, z, 1);
            CreateUnit(x, z);
            gridCell[x, z].TakeUpCell();
            return true;
        }

        return false;
    }

    public bool AddUnit(UnitGrade grade)
    {
        while (indexStack.Count > 0)
        {
            int curIndex = indexStack.Dequeue();
            (int x, int z) xz = IndexToXZ(curIndex);

            int x = xz.Item1;
            int z = xz.Item2;

            if (OutOfBounds(x, z)) return false; // 밤위 밖 탈출
            if (gridObj[x, z]) continue;

            SetValue(x, z, 1);
            CreateUnit(x, z, grade);
            gridCell[x, z].TakeUpCell();
            return true;
        }

        return false;
    }
    private void CreateUnit(int x, int z)
    {
        var weapon = Managers.Object.SpawnWeapon();
        weapon.transform.SetParent(root.transform);
        Vector3 newPos = GetGridCellPosition(x, z, 2);
        weapon.transform.position = newPos;

        gridObj[x, z] = weapon;
        gridObj[x, z].name = $"{x},{z} : {z + x * height}";
        SetValue(x, z, 1);
    }
    private void CreateUnit(int x, int z, UnitGrade grade)
    {
        var weapon = Managers.Object.SpawnWeapon(grade);
        weapon.transform.SetParent(root.transform);
        Vector3 newPos = GetGridCellPosition(x, z, 2);
        weapon.transform.position = newPos;

        gridObj[x, z] = weapon;
        gridObj[x, z].name = $"{x},{z} : {z + x * height}";
        SetValue(x, z, 1);
    }


    public bool Remove(Vector3 worldPosition)
    {
        int x = -1, z = -1;
        GetXZ(worldPosition, out x, out z);
        if (OutOfBounds(x, z)) return false;
        if (gridObj[x, z] == null) return false;

        indexStack.Enqueue(GetIndex(x, z));
        SetValue(x, z, 0);
        gridCell[x, z].ResetCell();

        var weapon = gridObj[x, z] as Weapon;
        Managers.Object.Player.SetGold(Managers.Data.GetSellGold(weapon.GetWeaponData().grade));

        Managers.Object.DespawnSweapon(gridObj[x, z]);
        Managers.Resource.Destroy(gridObj[x, z].gameObject);
        gridObj[x, z] = null;
        return true;
    }

    /*-----------
    그리드 UX 함수
    -------------*/
    public void GridCellViewerOn()
    {
        cellRoot.SetActive(true);
    }
    public void GridCellViewerOff()
    {
        cellRoot.SetActive(false);
    }
    /// <summary>
    /// 선택된 무기가 움직일 곳(마우스 포인터 위치)을 보여줄 UX 함수
    /// @date 24-07-01
    /// @version 2
    /// </summary>
    /// <param name="pointPos">마우스 포인터 위치</param>
    /// <param name="originalPos">오리지널 쉘 위치</param>
    public void GetPointToGrid(Vector3 pointPos, Vector3 originalPos)
    {
        int pX = -1, pZ = -1;
        GetXZ(pointPos, out pX, out pZ);
        int oX = -1, oZ = -1;
        GetXZ(originalPos, out oX, out oZ);

        if (OutOfBounds(pX, pZ) || OutOfBounds(oX, oZ))
        {
            ResetCell();
            return;
        }

        // 선택된 무기의 원래 그리드 위치와 현재 마우스 포인터의 그리드 위치가 같은 경우
        if (pX == oX && pZ == oZ)
        {
            ResetCell();
            return;
        }

        // 이미 선택된 셀의 색상을 원래대로 복원
        if (selectedCell != null)
        {
            int sX = -1, sZ = -1;
            GetXZ(selectedCell.transform.position, out sX, out sZ);
            if (gridObj[sX, sZ] != null)
            {
                selectedCell.TakeUpCell();
            }
            else
            {
                selectedCell.ResetCell();
            }
            selectedCell = null;
        }

        // 선택된 그리드 셀을 새로운 셀로 업데이트
        if (gridObj[pX, pZ] == null)
        {
            selectedCell = gridCell[pX, pZ];
            gridCell[pX, pZ].SelectedCell();
        }
        // 선택된 셀에 이미 타워가 있는 경우 병합 색상을 표시
        else
        {
            var selectedweapon = gridObj[oX, oZ] as Weapon;
            var pointedWeapon = gridObj[pX, pZ] as Weapon;
            // 같은 경우 TODO 강화 가능 UI
            if (selectedweapon.GetWeaponData().Equals(pointedWeapon.GetWeaponData()))
            {
                selectedCell = gridCell[pX, pZ];
                gridCell[pX, pZ].MergeCell();
            }
            else
            {
                selectedCell = gridCell[pX, pZ];
                gridCell[pX, pZ].ConflictCell();
            }

        }
    }

    private void ResetCell()
    {
        if (selectedCell == null) return;

        int x = -1, z = -1;
        GetXZ(selectedCell.transform.position, out x, out z);

        if (OutOfBounds(x, z)) return;

        selectedCell.ResetCell();
        selectedCell = null;

        if (gridObj[x, z])
        {
            gridCell[x, z].TakeUpCell();
        }
    }


    /*-------------
    Grid Helper 함수
    --------------*/
    private Vector3 GetGridCellPosition(int x, int z, float y)
    {
        return GetWorldPosition(x, z) + new Vector3(cellSizeX, y, cellSizeZ) * 0.5f;
    }

    private Vector3 GetGridCellPosition(int x, int z)
    {
        return GetWorldPosition(x, z) + new Vector3(cellSizeX, 0, cellSizeZ) * 0.5f;
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSizeX + originPosition;
    }

    public void SetValue(int x, int z, int value)
    {
        if (OutOfBounds(x, z)) return;

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
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSizeX);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSizeZ);
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
        return GetIndex(x, z);
    }
    public int GetIndex(int x, int z)
    {
        return z + x * height;
    }
    public (int, int) IndexToXZ(int index)
    {
        int x = index / height;
        int z = index % height;
        return (x, z);
    }

    public int GetValue(int x, int z)
    {
        if (OutOfBounds(x, z)) return -1;
        else
        {
            return gridArray[x, z];
        }
    }

    private bool OutOfBounds(int x, int z)
    {
        return x < 0 || x >= width || z < 0 || z >= height;
    }
}
