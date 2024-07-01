using System;
using System.Collections;
using UnityEngine;
using static MyEnums;

public class Projectile : MonoBehaviour
{
    protected int damage = 0;
    protected float speed = 0f;
    protected ProjectileType projectileType = ProjectileType.None;
    protected Coroutine moveCoroutine = null;

    public virtual void Fire(Monster target, Vector3 firePos, int damage, float speed)
    {
        this.damage = 0;
        this.speed = Math.Max(20,speed);
        gameObject.transform.position = firePos;
        this.damage = damage;
        if(moveCoroutine == null)
            moveCoroutine = StartCoroutine(MoveProjectile(target, gameObject));
    }

    protected virtual IEnumerator MoveProjectile(Monster enemy, GameObject projectile)
    {
        Debug.LogError("발사체 움직임 구현 하세요");
        yield break;
    }

    protected float GetDistance(Vector3 targetPosition, GameObject projectile)
    {
        return Vector3.Distance(targetPosition, projectile.transform.position);
    }
}
