using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage = 0;
    private float speed = 0f;

    public void Shoot(Monster target, Vector3 firePos, int damage, float speed)
    {
        this.damage = 0;
        this.speed = Math.Max(20,speed);
        gameObject.transform.position = firePos;
        this.damage = damage;
        StartCoroutine(MoveProjectile(target, gameObject));
    }

    IEnumerator MoveProjectile(Monster enemy, GameObject projectile)
    {
        projectile.transform.position = transform.position;
        Vector3 targetPosition = enemy.transform.position;

        while (enemy != null && enemy.isDead == false && GetDistance(targetPosition, projectile) > 0.50f)
        {
            var dir = targetPosition - projectile.transform.position;
            var angleDirection = Quaternion.LookRotation(dir);
            projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, angleDirection, Time.deltaTime * 10f);
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, speed * Time.deltaTime);

            yield return null;

            // Update targetPosition if the enemy is still alive
            if (enemy != null && enemy.isDead == false)
            {
                targetPosition = enemy.transform.position;
            }
        }

        // Ensure the projectile reaches the target position even if the enemy is dead
        while (GetDistance(targetPosition, projectile) > 0.50f)
        {
            var dir = targetPosition - projectile.transform.position;
            var angleDirection = Quaternion.LookRotation(dir);
            projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, angleDirection, Time.deltaTime * 10f);
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, speed * Time.deltaTime);

            yield return null;
        }

        if (projectile != null && enemy != null && enemy.isDead == false)
        {
            enemy.GetComponent<Monster>().OnDamage(damage);
        }

        Managers.Resource.Destroy(projectile);
    }

    float GetDistance(Vector3 targetPosition, GameObject projectile)
    {
        return Vector3.Distance(targetPosition, projectile.transform.position);
    }

    private float GetDistance(GameObject target, GameObject projectile)
    {
        return (target.transform.position - projectile.transform.position).sqrMagnitude;
    }
}
