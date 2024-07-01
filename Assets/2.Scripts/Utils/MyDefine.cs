using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDefine
{
    public static float CURRENT_TIME => Time.time;
    public static readonly float STAGE_DELAY = 60f;
    public static readonly Color GRID_CELL_EMPTY = Color.green;
    public static readonly Color GRID_CELL_SELECTED = Color.yellow;
    public static readonly Color GRID_CELL_FULL = Color.red;


}
