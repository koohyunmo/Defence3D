using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDefine
{
    /*----------------
            상수
    ------------------*/
    public static int MAX_MONSTER = 100;
    public static float REINFORCE_BASE_PROBABILITY = 75f; // 최대 강화 확률
    public static float REINFORCE_MIN_PROBABILITY = 3f; // 강화 감소율
    public static int WEAPON_REINFORCE_NAX_LEVEL = 9;
    public static float CURRENT_TIME => Time.time;
    public static readonly float STAGE_DELAY = 20f;
    public static readonly float BOSS_DELAY = 60f;
    public static readonly Color GRID_CELL_EMPTY = Color.green;
    public static readonly Color GRID_CELL_SELECTED = Color.yellow;
    public static readonly Color GRID_CELL_FULL = Color.red;
    public static readonly string MONSTER_TAG = "Monster";


}
