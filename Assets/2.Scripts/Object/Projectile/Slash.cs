using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;
using static MyDefine;
public class Slash : Projectile
{
    private float maxDistance = 15f; // 발사체가 날아갈 최대 거리

    public override void Fire(Monster target, Vector3 firePos, int damage, float speed)
    {
        projectileType = ProjectileType.Slash;
        base.Fire(target, firePos, damage, speed);
    }

    protected override IEnumerator MoveProjectile(Monster enemy, GameObject projectile)
    {
        float distanceCovered = 0f; // 발사체가 날아간 거리
        Vector3 startPosition = projectile.transform.position; // 발사체의 시작 위치
        Vector3 targetPosition = enemy.transform.position;
        projectile.transform.position = startPosition;

        while (distanceCovered < maxDistance)
        {
            // 현재 위치에서 목표 위치를 향하는 방향 벡터 계산
            Vector3 direction = (targetPosition - startPosition).normalized;
            direction.y = 0;

            // 발사체를 목표 위치로 이동시키기
            projectile.transform.position += direction * speed * Time.deltaTime;

            // 이번 프레임에서 발사체가 움직인 거리 누적
            distanceCovered += speed * Time.deltaTime;

            yield return null; // 한 프레임을 기다림
        }

        StopCoroutine(moveCoroutine);
        moveCoroutine = null;
        // 예시로 발사체 제거
        Managers.Resource.Destroy(projectile);

    }


    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(MONSTER_TAG))
        {
            other.GetComponent<Monster>().OnDamage(damage);
        }
    }

}
