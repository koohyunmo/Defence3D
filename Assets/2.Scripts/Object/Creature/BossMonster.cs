using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Experimental.AI;
using static MyDefine;

public class BossMonster : Monster
{
    UI_BossInfoPopup bossPopup;
    Coroutine co_counter;
    public override void Spawn(MonsterData data)
    {
        // 보스 등장
        gameObject.SetActive(true);
        //gameObject.transform.GetChild(0).rotation = Quaternion.Euler(90f, 90f, 90f);
        //gameObject.transform.GetChild(0).rotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one * 2.5f;
        base.Spawn(data);

        bossPopup = Managers.UI.ShowPopupUI<UI_BossInfoPopup>();

        if(co_counter != null)
        {
            StopCoroutine(co_counter);
            co_counter = null;
        }
        co_counter = StartCoroutine(co_BossCounter());

    }

    public override void OnDead()
    {
        base.OnDead();
        bossPopup.ClosePopup();
    }
    public override void OnDamage(int damage)
    {
        hp -= damage;

        hp = Math.Max(0, hp);
        hpBar.SetHpBar(hp, maxHp);
        bossPopup.BossInfoUpdate(hp,maxHp);
        Managers.Effect.DamageTextParticle(damage, transform.position + Vector3.up);

        if (hp <= 0)
        {
            if (IsDead == false)
            {
                OnDead();
                IsDead = true;
            }

        }
    }
    protected override void Reward()
    {
        Managers.Object.Player.GemReward(Managers.Stage.GetStageCount() / 10);
    }

    IEnumerator co_BossCounter()
    {
        float timer = BOSS_DELAY;

        while(timer >= 0f)
        {
            timer -= Time.deltaTime;
            if(bossPopup)
                bossPopup.CountUpdate(timer);
            yield return null;
        }

        if(IsDead == false)
        {
            Debug.Log("Game End");
            Managers.Stage.GameOver();
        }

        StopCoroutine(co_counter);
        co_counter = null;
        yield break;

    }
}
