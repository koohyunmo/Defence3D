using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossInfoPopup : UI_Popup
{
    public enum Texts
    {
        Hp_Text,
        Count_Text
    }

    enum Sliders
    {
        BossSlider
    }

    Text hpText;
    Text timerText;
    Slider bossMonsterHpSlider;

    public override void Init() 
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));

        hpText = Get<Text>((int)Texts.Hp_Text);
        timerText = Get<Text>((int)Texts.Count_Text);
        bossMonsterHpSlider = Get<Slider>((int)Sliders.BossSlider);
    }

    public void BossInfoUpdate(int hp, int maxHp)
    {
        if(hpText)
            hpText.text = $"{hp}/{maxHp}";

        float ratio = hp/ (float)maxHp;

        if(bossMonsterHpSlider)
            bossMonsterHpSlider.value = ratio;
    }
    public void CountUpdate(float timer)
    {
        if(timerText)
            timerText.text = Utils.FormatSecondsTime(timer);
    }

    public void ClosePopup()
    {
        if(Managers.UI.ClosePopupUI(this) == false)
        {
            gameObject.SetActive(false);
        }
    }

}
