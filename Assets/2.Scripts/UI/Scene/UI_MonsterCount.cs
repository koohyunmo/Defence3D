using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterCount : UI_Scene
{
    enum Sliders
    {
        CountSlider
    }

    enum Texts
    {
        CountText
    }

    Slider countSlider;
    Text countText;

    private void Start() 
    {
        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));

        countSlider = Get<Slider>((int)Sliders.CountSlider);
        countText = Get<Text>((int)Texts.CountText);

        StartCoroutine(co_Count());
    }

    IEnumerator co_Count()
    {
        while (true)
        {
            yield return null;
            countText.text = $"{Managers.Object.GetMonsterCountStr()}/{MyDefine.MAX_MONSTER}";
            float ratio = Managers.Object.GetMonsterCount()/ (float)MyDefine.MAX_MONSTER;
            countSlider.value = ratio;
        }
    }
}
