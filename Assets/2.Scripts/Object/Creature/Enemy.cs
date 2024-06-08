using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    TestFollowPath movement;
    public bool isDead = false;
    int maxHp = 100;
    int hp = 0;

    public void Spawn() 
    {
        isDead = false;
        hp = maxHp;

        if (movement == null)
            movement = gameObject.GetComponent<TestFollowPath>();

        movement.SetPosition();
    }

    public void OnDamage(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
           
            if(isDead == false)
            {
                Managers.Resource.Destroy(gameObject);
                isDead = true;
            }

        }
    }
}
