using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;

public class Monster : MonoBehaviour
{
    TestFollowPath movement;
    public bool isDead = false;
    int hp {get => data.hp; set => data.hp = value;}

    [SerializeField] int maxHp;

    MonsterData data = null;

    HpBar hpBar = null;

    public void Spawn(MonsterData data) 
    {
        this.data = data;

        isDead = false;

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

    public void OnDamage(int damage)
    {
        hp -= damage;

        hp = Math.Max(0,hp);
        hpBar.SetHpBar(hp, maxHp);
        Managers.Effect.DamageTextParticle(damage, transform.position + Vector3.up);

        if (hp <= 0)
        {
            if(isDead == false)
            {
                OnDead();
                isDead = true;
            }

        }
    }

    public void OnDead()
    {
        Managers.Object.Player.GoldReward();
        Managers.Resource.Destroy(gameObject);
        Managers.Object.DespawnMonster(this);
    }
}
