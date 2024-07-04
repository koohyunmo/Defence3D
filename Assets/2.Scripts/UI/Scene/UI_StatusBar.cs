using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatusBar : UI_Scene
{
    enum TMPs
    {
        GemTMP,
        GoldTMP,
    }

    TextMeshProUGUI goldTMP;
    TextMeshProUGUI gemTMP;
    private void Start()
    {
        Bind<TextMeshProUGUI>(typeof(TMPs));

        gemTMP = Get<TextMeshProUGUI>((int)TMPs.GemTMP);
        goldTMP = Get<TextMeshProUGUI>((int)TMPs.GoldTMP);

        goldTMP.text = Managers.Object.Player.gold.ToString();
        gemTMP.text = Managers.Object.Player.gold.ToString();

        Managers.Notify.RegisterGoldEvent(UpdateUI);
    }

    private void UpdateUI()
    {
        goldTMP.text = Managers.Object.Player.gold.ToString();
        gemTMP.text = Managers.Object.Player.gold.ToString();
    }
}
