using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpgradePopup : UI_Popup
{
    enum ButtonIMG
    {
        BG,
        CloseIMG,

        SpawnUpgrade
    }

    enum Buttons
    {
        Upgrade1_Button,
        Upgrade2_Button,
        Upgrade3_Button,
        SpawnUpgrade_Button
    }

    enum TMPs
    {
        Upgrade1_TMP,
        Upgrade2_TMP,
        Upgrade3_TMP,
        SpawnUpgrade_TMP,

    }
    enum Texts
    {
        Upgrade1Level_Text,
        Upgrade2Level_Text,
        Upgrade3Level_Text,
        SpawnUpgradeLevel_Text
    }


    TextMeshProUGUI upgrade1PriceTMP;
    TextMeshProUGUI upgrade2PriceTMP;
    TextMeshProUGUI upgrade3PriceTMP;
    TextMeshProUGUI spawnUpgradePriceTMP;

    Text upgrade1LevelTMP;
    Text upgrade2LevelTMP;
    Text upgrade3LevelTMP;
    Text spawnUpgradeLevelTMP;

    private void OnEnable() 
    {

        if(upgrade1PriceTMP && spawnUpgradeLevelTMP)
        {
            UpdateUI();
        }
        
    }


    private void Start() 
    {
        Bind<Image>(typeof(ButtonIMG));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPs));
        Bind<Text>(typeof(Texts));

        Get<Image>((int)ButtonIMG.BG).gameObject.BindEvent(ClosePopup);
        Get<Image>((int)ButtonIMG.CloseIMG).gameObject.BindEvent(ClosePopup);

        Get<Button>((int)Buttons.Upgrade1_Button).gameObject.BindEvent((p) => Managers.Upgrade.Upgrade1(Managers.Object.Player));
        Get<Button>((int)Buttons.Upgrade2_Button).gameObject.BindEvent((p) => Managers.Upgrade.Upgrade2(Managers.Object.Player));
        Get<Button>((int)Buttons.Upgrade3_Button).gameObject.BindEvent((p) => Managers.Upgrade.Upgrade3(Managers.Object.Player));
        Get<Button>((int)Buttons.SpawnUpgrade_Button).gameObject.BindEvent(ClickUpdateSpawn);

        upgrade1PriceTMP = Get<TextMeshProUGUI>((int)TMPs.Upgrade1_TMP);
        upgrade2PriceTMP = Get<TextMeshProUGUI>((int)TMPs.Upgrade2_TMP);
        upgrade3PriceTMP = Get<TextMeshProUGUI>((int)TMPs.Upgrade3_TMP);
        spawnUpgradePriceTMP = Get<TextMeshProUGUI>((int)TMPs.SpawnUpgrade_TMP);

        upgrade1LevelTMP = Get<Text>((int)Texts.Upgrade1Level_Text);
        upgrade2LevelTMP = Get<Text>((int)Texts.Upgrade2Level_Text);
        upgrade3LevelTMP = Get<Text>((int)Texts.Upgrade3Level_Text);
        spawnUpgradeLevelTMP = Get<Text>((int)Texts.SpawnUpgradeLevel_Text);

        UpdateUI();

        Managers.Upgrade.RegisterUpdateUI(UpdateUI);
    }

    private void ClickUpdateSpawn(PointerEventData data)
    {
        if (Managers.Random.CanUpgrade())
        {
            Managers.Upgrade.UpgradeSpawn(Managers.Object.Player);
        }
        else
        {
            spawnUpgradeLevelTMP.text = $"MAX";
            Get<Button>((int)Buttons.SpawnUpgrade_Button).interactable = false;
        }
    }

    private void UpdateUI()
    {
        if(gameObject.activeSelf == false) return;
        
        upgrade1PriceTMP.text = Managers.Upgrade.Upgrade1Price.ToString();
        upgrade2PriceTMP.text = Managers.Upgrade.Upgrade2Price.ToString();
        upgrade3PriceTMP.text = Managers.Upgrade.Upgrade3Price.ToString();
        spawnUpgradePriceTMP.text = Managers.Upgrade.SpawnUpgradePrice.ToString();

        upgrade1LevelTMP.text = $" LV{Managers.Upgrade.GetLevel(MyEnums.UnitGrade.Basic, Managers.Object.Player)} ";
        upgrade2LevelTMP.text = $" LV{Managers.Upgrade.GetLevel(MyEnums.UnitGrade.Relic, Managers.Object.Player)} ";
        upgrade3LevelTMP.text = $" LV{Managers.Upgrade.GetLevel(MyEnums.UnitGrade.Mythic, Managers.Object.Player)} ";

        if (Managers.Random.CanUpgrade())
        {
            spawnUpgradeLevelTMP.text = $" LV{Managers.Object.Player.SpawnLevel} ";
        }
        else
        {
            spawnUpgradeLevelTMP.text = $"MAX";
            Get<Button>((int)Buttons.SpawnUpgrade_Button).interactable = false;
        }

    }

    private void ClosePopup(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
}
