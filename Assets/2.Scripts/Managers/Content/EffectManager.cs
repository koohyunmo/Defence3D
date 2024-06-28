using System;
using System.Collections;
using System.Collections.Generic;
using Necromancy.UI;
using UnityEngine;

public class EffectManager
{
    TextRendererParticleSystem textParticleSystem = null;
    public void Init()
    {
       var go = Managers.Resource.Instantiate("UIParticleSystem");
       textParticleSystem = go.GetComponent<TextRendererParticleSystem>();
    }

    public void DamageTextParticle(int damage, Vector3 pos)
    {
        textParticleSystem.SpawnDamageParticle(pos, damage,Color.red);
    }

}
