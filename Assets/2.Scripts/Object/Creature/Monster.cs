using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;

public class Monster : MonoBehaviour
{
    TestFollowPath movement;
    protected int hp {get => data.hp; set => data.hp = value;}

    protected int maxHp {get => data.maxHp; set => data.maxHp = value;}

    protected MonsterData data = null;
    protected HpBar hpBar = null;

    public bool IsDead { get; protected set; } = false;



    public virtual void Spawn(MonsterData data) 
    {
        this.data = data;

        IsDead = false;

        if (movement == null)
            movement = gameObject.GetComponent<TestFollowPath>();
            
        if(hpBar == null)
        {
            var go = Managers.Resource.Instantiate("HpBar",transform);
            go.transform.rotation = Quaternion.identity;
            go.transform.localPosition += Vector3.up;
            hpBar = go.GetComponent<HpBar>();
        }
        else
        {
            hpBar.ResetHpBar();
        }


        movement.SetPosition();
        maxHp = data.maxHp;
        gameObject.tag = MONSTER_TAG;
    }

    public virtual void OnDamage(int damage)
    {
        hp -= damage;

        hp = Math.Max(0,hp);
        hpBar.SetHpBar(hp, maxHp);
        Managers.Effect.DamageTextParticle(damage, transform.position + Vector3.up);

        if (hp <= 0)
        {
            if(IsDead == false)
            {
                OnDead();
                IsDead = true;
            }

        }
    }

    public virtual void OnDead()
    {
        Reward();
        Managers.Object.DespawnMonster(this);
        //Debug.Log("TODO : 몬스터 죽는 사운드");
    }

    protected virtual void Reward()
    {
        Managers.Object.Player.GoldReward();
    }
    public MonsterData GetData()
    {
        return data;
    }
}
