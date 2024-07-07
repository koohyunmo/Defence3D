using System;
using System.Collections;
using System.Collections.Generic;
using Necromancy.UI;
using TMPro;
using UnityEngine;
using static MyDefine;

public class EffectManager
{
    TextRendererParticleSystem textParticleSystem = null;
    TextMeshPro reinforceIndicator = null;
    public void Init()
    {
       var go = Managers.Resource.Instantiate("UIParticleSystem");
       textParticleSystem = go.GetComponent<TextRendererParticleSystem>();

        reinforceIndicator = Managers.Resource.Instantiate("ReinforcePercent_TextMesh").GetComponent<TextMeshPro>();
        reinforceIndicator.gameObject.SetActive(false);

    }

    public void DamageTextParticle(int damage, Vector3 pos)
    {
        textParticleSystem.SpawnDamageParticle(pos, damage,Color.red);
    }

    public void UpgradeSucess(Transform target)
    {
        var go = Managers.Resource.Instantiate("Up_Sprite",pooling:true);
        var pos = target.position;

        pos += Vector3.up * 2f;
        go.transform.position = pos;

        var upSprite = go.GetComponent<UpSprite>();
        upSprite.Spawn();
    }

    public void ShowReinforcePercent(float percent,Transform target)
    {
        reinforceIndicator.gameObject.SetActive(true);
        Color color = Color.Lerp(Color.red,Color.green, Utils.Normalize(percent,0, REINFORCE_BASE_PROBABILITY));
        reinforceIndicator.color = color;
        reinforceIndicator.text = $"%{percent}";

        var pos = target.position;
        pos.y = reinforceIndicator.transform.position.y;

        reinforceIndicator.transform.position = pos;

    }



    public void CloseReinforcePercent()
    {
        reinforceIndicator.gameObject.SetActive(false);
    }


}
