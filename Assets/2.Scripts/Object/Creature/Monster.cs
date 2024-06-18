using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void OnDamage(int damage)
    {
        hp -= damage;

        hp = Math.Max(0,hp);
        hpBar.SetHpBar(hp, maxHp);

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
        Managers.Resource.Destroy(gameObject);
        Managers.Object.DespawnMonster(this);
    }
}