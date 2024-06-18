using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTimer : MonoBehaviour
{
    Text timer = null;
    void Start()
    {
        timer = GetComponent<Text>();
        timer.text = Managers.Stage.GetTime().ToString("0") + '\n' + Managers.Stage.GetStageCount();

        StartCoroutine(co_Timer());
    }

    IEnumerator co_Timer()
    {
        // 게임이 끝날때 까지
        while(true)
        {
            timer.text = Managers.Stage.GetTime().ToString("0") + '\n' + Managers.Stage.GetStageCount();
            yield return new WaitForSeconds(1f);
        }
    }
}
