using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageTimerAndStageCount : UI_Scene
{
    enum Texts
    {
        TimerText
    }
    enum TMPs
    {
        StageTMP
    }
    Text timer;
    TextMeshProUGUI stageCounter;
    public void Start()
    {
        Bind<Text>(typeof(Texts));
        Bind<TextMeshProUGUI>(typeof(TMPs));

        timer = Get<Text>((int)Texts.TimerText);
        stageCounter = Get<TextMeshProUGUI>((int)TMPs.StageTMP);

        timer.text = Utils.FormatMinutesTime(Managers.Stage.GetTime());
        stageCounter.text = $"STAGE {Managers.Stage.GetStageCount()}";

        StartCoroutine(co_Timer());
    }
    IEnumerator co_Timer()
    {
        // 게임이 끝날때 까지
        while (true)
        {
            timer.text = Utils.FormatMinutesTime(Managers.Stage.GetTime());
            stageCounter.text = $"STAGE {Managers.Stage.GetStageCount()}";
            yield return new WaitForSeconds(1f);
        }
    }


}
