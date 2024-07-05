using System;
using System.Collections;
using UnityEngine;
using static MyDefine;

public class StageManager 
{
    private float stagetTimer = STAGE_DELAY;
    private int stageCount = 1;
    private float spawnDelay = 0.5f;
    private float spawnCoolTime = 0;
    private bool bossSpawn = false;
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
        stageCount++;
    }

    public void TestDownStage()
    {
        stageCount = Math.Min(1,stageCount--);
    }

    IEnumerator co_MonsterSpawn()
    {
        stagetTimer = STAGE_DELAY;
        while (true)
        {
            if(Managers.Object.GetMonsterCount() >= 100)
            {
                Debug.Log("Game Over");
            }

            stagetTimer -= Time.deltaTime;
            if(CURRENT_TIME > spawnCoolTime)
            {
                Managers.Object.SpawnMosnter();
                spawnCoolTime = CURRENT_TIME + spawnDelay;
                if(stageCount % 10 == 0 && bossSpawn == false)
                {
                    bossSpawn = true;
                    Managers.Object.SpawnBoss();
                    Debug.Log("보스 스톤");
                }
                else if(bossSpawn && stageCount % 10 != 0)
                {
                    bossSpawn = false;
                }
            }

            // 스테이지 변경
            if (stagetTimer < 1)
            {
                stagetTimer = STAGE_DELAY;
                stageCount++;
                Managers.Data.DisplayStageHPAndDPS(stageCount);
            }

            yield return null;
        }
    }
}
