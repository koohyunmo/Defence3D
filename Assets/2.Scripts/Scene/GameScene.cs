using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("prefab", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Debug.Log("Load All");
            }
        });

        StartCoroutine(co_MonsterSpawn());
    }

    IEnumerator co_MonsterSpawn()
    {
        yield return new WaitForSeconds(5f);

        while(true)
        {
            yield return new WaitForSeconds(0.75f);
            Managers.Object.SpawnMosnter();
        }
    }

}
