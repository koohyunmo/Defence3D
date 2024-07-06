using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MyEnums;
public class UI_WeaponCount : UI_Scene
{

    enum Images
    {
        Basic_Icon,
        Rare_Icon,
        Ancient_Icon,
        Relic_Icon,
        Epic_Icon,
        Legendary_Icon,
        Mythic_Icon,
        Mythical_Icon,
        Primal_Icon
    }
    enum Texts
    {
        Basic_Text,
        Rare_Text,
        Ancient_Text,
        Relic_Text,
        Epic_Text,
        Legendary_Text,
        Mythic_Text,
        Mythical_Text,
        Primal_Text
    }

    Text basicText;

    private void Start() 
    {
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        for(int i = 0; i <= (int)Images.Primal_Icon; i++)
        {
            Get<Image>((int)Images.Basic_Icon + i).color = Managers.Data.GetColorForGrade((UnitGrade)(i + 1));
        }

        UpdateUI();
        Managers.Object.RegisterUpdateUI(UpdateUI);
    }

    private void UpdateUI()
    {
        for(int i = 0; i <= (int)Texts.Primal_Text; i++)
        {
            Get<Text>((int)Texts.Basic_Text + i).text = Managers.Object.GetWeaponCount((UnitGrade)(i + 1)).ToString();
        }
    }
}

