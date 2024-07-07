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
             //Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
             {
                Debug.Log("Resource Load All");
                Managers.Effect.Init();
                Managers.Object.Init();

                 Managers.UI.ShowSceneUI<UI_StageTimerAndStageCount>();
                 Managers.UI.ShowSceneUI<UI_MonsterCount>();
                 Managers.UI.ShowSceneUI<UI_HorizontalButtons>();
                 Managers.UI.ShowSceneUI<UI_StatusBar>();
                 Managers.UI.ShowSceneUI<UI_WeaponCount>();

             }
         });

        StartCoroutine(co_GameStart());
    }

    IEnumerator co_GameStart()
    {
        Managers.Stage.GameStart();
        yield break;
    }

}
