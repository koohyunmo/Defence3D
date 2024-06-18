using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;

public class StageManager 
{
    private float stagetTimer = STAGE_DELAY;
    private int stageCount = 1;
    public void StagetStart()
    {
        Managers.Instance.StartCoroutine(co_MonsterSpawn());
    }

    public float GetTime()
    {
        return stagetTimer;
    }

    public int GetStageCount()
    {
        return stageCount;
    }

    public void TestUpStage()
    {
        stageCount = stageCount++;
    }

    public void TestDownStage()
    {
        stageCount = Math.Min(1,stageCount--);
    }

    IEnumerator co_MonsterSpawn()
    {
        stagetTimer = STAGE_DELAY;;
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            stagetTimer -= 0.75f;
            Managers.Object.SpawnMosnter();

            // 스테이지 변경
            if (stagetTimer < 1)
            {
                stagetTimer = STAGE_DELAY;
                stageCount++;
            }
        }
    }
}
