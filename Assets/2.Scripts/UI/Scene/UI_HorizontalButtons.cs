using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HorizontalButtons : UI_Scene
{
    enum Buttons
    {
        UpgradeButton,
        SpawnButton,
        GambleButton
    }

    enum TMPs
    {
        GamblePriceTMP,
        SpawnPriceTMP
    }

    TextMeshProUGUI gameblePriceTMP;
    TextMeshProUGUI spawnPriceTMP;

    private void Start() 
    {
        Bind<TextMeshProUGUI>(typeof(TMPs));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.SpawnButton).gameObject.BindEvent((p) => Managers.Spawn.Spawn(Managers.Object.Player));
        GetButton((int)Buttons.GambleButton).gameObject.BindEvent((p) => Managers.Spawn.Spawn(Managers.Object.Player));
        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent((p) => Managers.UI.ShowPopupUI<UI_UpgradePopup>());

        gameblePriceTMP = Get<TextMeshProUGUI>((int)TMPs.GamblePriceTMP);
        spawnPriceTMP = Get<TextMeshProUGUI>((int)TMPs.SpawnPriceTMP);

        Managers.Spawn.RegisterUI(UpdateUI);
        UpdateUI();
    }

    private void UpdateUI()
    {
        gameblePriceTMP.text = Managers.Spawn.GetGambleSpawnPrice().ToString();
        spawnPriceTMP.text = Managers.Spawn.GetSpawnPrice().ToString();
    }



}
