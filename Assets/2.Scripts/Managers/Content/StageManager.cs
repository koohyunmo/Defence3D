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

    private Coroutine coGameCoroutine;

    public void GameStart()
    {
        coGameCoroutine = Managers.Instance.StartCoroutine(co_MonsterSpawn());
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

    IEnumerator co_MonsterSpawn()
    {
        stagetTimer = STAGE_DELAY;
        bool bossSpawn = false; // 보스 스폰 여부를 추적하기 위한 변수

        yield return new WaitForSeconds(2f);
        var popup = Managers.UI.ShowPopupUI<UI_StageNotiPopup>();
        popup.Spawn();
        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (Managers.Object.GetMonsterCount() >= 100)
            {
                GameOver();
                yield break;
            }
            if(stageCount > 100)
            {
                GameClear();
                yield break;
            }

            stagetTimer -= Time.deltaTime;

            if (CURRENT_TIME > spawnCoolTime)
            {
                Managers.Object.SpawnMosnter();
                spawnCoolTime = CURRENT_TIME + spawnDelay;
            }

            // 보스 스폰
            if (stageCount % 10 == 0 && !bossSpawn)
            {
                bossSpawn = true;
                Managers.Object.SpawnBoss();

                var bosspopup = Managers.UI.ShowPopupUI<UI_BossSpawnNotiPopup>();
                bosspopup.Spawn();
            }

            // 스테이지 변경
            if (stagetTimer < 1)
            {
                stagetTimer = STAGE_DELAY;
                stageCount++;

                if (stageCount % 10 != 0)
                {
                    bossSpawn = false; // 보스 스폰 상태를 초기화합니다.
                    Managers.Data.DisplayStageHPAndDPS(stageCount);

                    var go = Managers.UI.ShowPopupUI<UI_StageNotiPopup>();
                    go.Spawn();
                }
                else
                {
                    bossSpawn = false; // 다음 10의 배수 스테이지에서 보스가 다시 스폰될 수 있도록 초기화합니다.
                }
            }

            yield return null;
        }
    }

    public void GameOver()
    {
        //Time.timeScale = 0;
        Debug.Log("Game Over");
        // TODO 종료 Or 재시작 UI
        var endPopup = Managers.UI.ShowPopupUI<UI_EndPopup>();
        endPopup.ShowGameOver();
    }

    public void GameClear()
    {
        //Time.timeScale = 0;
        Debug.Log("Game Clear");
        var endPopup = Managers.UI.ShowPopupUI<UI_EndPopup>();
        endPopup.ShowGameClear();
    }

    public void Clear()
    {
        stageCount = 1;
        stagetTimer = STAGE_DELAY;
        spawnDelay = 0.5f;
        spawnCoolTime = 0f;
    }

    public void Resert()
    {
        // 오브젝트 관련 클리어
       Managers.Object.Clear();
       Managers.Grid.Clear();
       // 스테이지 관련 클리어
       Clear();
       // UI 관련 클리어
       Managers.UI.CloseAllPopupUI();
       // 코루틴 관리
       if (coGameCoroutine != null)
       {
            Managers.Instance.StopCoroutine(coGameCoroutine);
            coGameCoroutine = null;
       }
        coGameCoroutine = Managers.Instance.StartCoroutine(co_MonsterSpawn());
        // 플레이어 관련 클리어
        Managers.Object.CreatePlayer();
        
        Managers.Random.Clear();
        Managers.Spawn.Clear();
        Managers.Upgrade.Clear();
        Managers.Notify.NotifyChangedGold();


    }
}
