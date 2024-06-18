using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDefine
{
    public static float CURRENT_TIME => Time.time;
    public static readonly float STAGE_DELAY = 30f;
    public static float SetDelay(float delay)
    {
        return CURRENT_TIME + delay;
    }
}
