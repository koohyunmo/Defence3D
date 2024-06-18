using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage = 0;

    public void Shoot(Monster target, Vector3 firePos,int damage)
    {
        this.damage = 0;
        gameObject.transform.position = firePos;
        this.damage = damage;
        StartCoroutine(MoveProjectile(target,gameObject));
    }

    IEnumerator MoveProjectile(Monster enemy,GameObject projectile)
    {
        projectile.transform.position = transform.position;

        while (enemy != null && enemy.isDead == false && GetDistance(enemy.gameObject, projectile) > 0.50f)
        {
            var dir = enemy.transform.position - projectile.transform.position;
            var angleDirection = Quaternion.LookRotation(dir);
            projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, angleDirection, Time.deltaTime * 10f);
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, enemy.transform.position, 5f * Time.deltaTime);

            yield return null;
        }

        if (projectile != null && enemy.isDead == false)
        {
            enemy.GetComponent<Monster>().OnDamage(damage);
            Managers.Resource.Destroy(projectile);
        }
        else
        {
            Managers.Resource.Destroy(projectile);
        }
    }


    private float GetDistance(GameObject target, GameObject projectile)
    {
        return (target.transform.position - projectile.transform.position).sqrMagnitude;
    }


}