using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    public override void Fire(Monster target, Vector3 firePos, int damage, float speed, Weapon owner)
    {
        base.Fire(target, firePos, damage, speed, owner);
        projectileType = MyEnums.ProjectileType.Arrow;
    }
    protected override IEnumerator MoveProjectile(Monster enemy, GameObject projectile)
    {
        projectile.transform.position = transform.position;
        Vector3 targetPosition = enemy.transform.position;

        while (enemy != null && enemy.isDead == false && GetDistance(targetPosition, projectile) > 0.50f)
        {
            LookAtTarget(targetPosition, projectile.transform);
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
            LookAtTarget(targetPosition, projectile.transform);
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        if (projectile != null && enemy != null && enemy.isDead == false)
        {
            if(GetDistance(enemy.transform.position, projectile) < 0.50f) // 풀링때문에 뒤에 맞는 공격 방지
                enemy.GetComponent<Monster>().OnDamage(damage);
        }

        StopCoroutine(moveCoroutine);
        moveCoroutine = null;
        Managers.Resource.Destroy(projectile);
    }
}
