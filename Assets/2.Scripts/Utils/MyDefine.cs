using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDefine
{
    /*----------------
            상수
    ------------------*/
    public static int MAX_MONSTER = 100;
    public static float CURRENT_TIME => Time.time;
    public static readonly float STAGE_DELAY = 20f;
    public static readonly Color GRID_CELL_EMPTY = Color.green;
    public static readonly Color GRID_CELL_SELECTED = Color.yellow;
    public static readonly Color GRID_CELL_FULL = Color.red;
    public static readonly string MONSTER_TAG = "Monster";


}
