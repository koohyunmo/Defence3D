using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

         Managers.Resource.LoadAllAsync<Object>("prefab", (key, count, totalCount) =>
         {
             Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
             {
                Debug.Log("Load All");
                Managers.Effect.Init();

                Managers.Data.Init();

                Player player = new Player();
                Managers.Object.SetPlayer(player);

                 Managers.UI.ShowSceneUI<UI_StageTimerAndStageCount>();
                 Managers.UI.ShowSceneUI<UI_MonsterCount>();
                 Managers.UI.ShowSceneUI<UI_HorizontalButtons>();
                 Managers.UI.ShowSceneUI<UI_StatusBar>();
             }
         });

        StartCoroutine(co_MonsterSpawn());
    }

    IEnumerator co_MonsterSpawn()
    {
        yield return new WaitForSeconds(5f);
        Managers.Stage.StagetStart();
    }

}
