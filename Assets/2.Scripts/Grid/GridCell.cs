using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;

public class GridCell : MonoBehaviour
{
    Material mat;

    void Awake()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.color = GRID_CELL_EMPTY;
    }

    public void TakeUpCell()
    {
        mat.color = GRID_CELL_FULL;
    }

    public void ResetCell()
    {
        mat.color = GRID_CELL_EMPTY;
    }

    public void SelectedCell()
    {
        mat.color = GRID_CELL_SELECTED;
    }
    public void MergeCell()
    {
        mat.color = Color.magenta;
    }
}
