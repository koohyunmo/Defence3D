using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StageNotiPopup : UI_Popup
{
    enum TMPs
    {
        Stage_TMP
    }
    TextMeshProUGUI stageTMP;
    
    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(TMPs));

        stageTMP = Get<TextMeshProUGUI>((int)TMPs.Stage_TMP);
    }

    public void Spawn()
    {
        if(stageTMP == null) Init();
        gameObject.SetActive(true);

        stageTMP.text = $"Stage {Managers.Stage.GetStageCount()}";
        StartCoroutine(co_UiAnimation());
    }

    IEnumerator co_UiAnimation()
    {
        yield return new WaitForSeconds(3f);
        if(Managers.UI.ClosePopupUI(this) == false)
        {
            gameObject.SetActive(false);
        }
    }
}
