using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatusBar : UI_Scene
{
    enum TMPs
    {
        Gem_TMP,
        Gold_TMP,
        UnitCount_TMP
    }

    TextMeshProUGUI goldTMP;
    TextMeshProUGUI gemTMP;
    TextMeshProUGUI unitCountTMP;
    private void Start()
    {
        Bind<TextMeshProUGUI>(typeof(TMPs));

        gemTMP = Get<TextMeshProUGUI>((int)TMPs.Gem_TMP);
        goldTMP = Get<TextMeshProUGUI>((int)TMPs.Gold_TMP);
        unitCountTMP = Get<TextMeshProUGUI>((int)TMPs.UnitCount_TMP);

        Managers.Notify.RegisterGoldUIEvent(UpdateGoldGemUI);
        Managers.Object.RegisterUpdateUI(UpdateUnitCountUI);

        UpdateGoldGemUI();
        UpdateUnitCountUI();
    }

    private void UpdateGoldGemUI()
    {
        goldTMP.text = Managers.Object.Player.Gold.ToString();
        gemTMP.text = Managers.Object.Player.Gem.ToString();
    }
    private void UpdateUnitCountUI()
    {
        if(Managers.Grid.GetGrid() != null)
            unitCountTMP.text = $"{Managers.Object.GetWeaponTotalCount()} / {Managers.Grid.GetGridSize()}";
    }
}
