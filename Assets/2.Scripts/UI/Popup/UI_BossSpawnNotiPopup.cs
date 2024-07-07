using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossSpawnNotiPopup : UI_Popup
{
    public void Spawn()
    {
        gameObject.SetActive(true);
        StartCoroutine(co_UiAnimation());
    }

    IEnumerator co_UiAnimation()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
