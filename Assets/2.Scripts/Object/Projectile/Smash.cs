using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;
using static MyDefine;

public class Smash : Projectile
{
    public override void Fire(Monster target, Vector3 firePos, int damage, float speed, Weapon owner)
    {
        projectileType = ProjectileType.Slash;
        base.Fire(target, firePos, damage, speed, owner);
    }

    protected override IEnumerator MoveProjectile(Monster enemy, GameObject projectile)
    {
        float distanceCovered = 0f; // 발사체가 날아간 거리
        Vector3 startPosition = projectile.transform.position; // 발사체의 시작 위치
        Vector3 targetPosition = enemy.transform.position;

        while (distanceCovered < 15f)
        {
            // 현재 위치에서 목표 위치를 향하는 방향 벡터 계산
            Vector3 direction = (targetPosition - startPosition).normalized;
            direction.y = 0;

            // 발사체를 목표 위치로 이동시키기
            projectile.transform.position += direction * speed * Time.deltaTime;

            // 이번 프레임에서 발사체가 움직인 거리 누적
            distanceCovered += speed * Time.deltaTime;

            // 발사체와 몬스터 간의 거리 계산
            float distanceToTarget = Vector3.Distance(projectile.transform.position, targetPosition);
            float radiusSum = 1f; // 발사체와 몬스터의 충돌 반경 합

            // 발사체가 몬스터에 닿으면 처리
            if (distanceToTarget <= radiusSum)
            {
                break;
            }

            yield return null; // 한 프레임을 기다림
        }

        projectile.transform.localScale = new Vector3(2.5f,2.5f,2.5f);
        // 중심점을 현재 오브젝트의 위치로 설정하고 반지름을 1로 설정하여 충돌체 검출
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(MONSTER_TAG))
            {
                collider.GetComponent<Monster>().OnDamage(damage);
            }
        }


        yield return new WaitForSeconds(0.5f);
        projectile.transform.localScale = Vector3.one;
        StopCoroutine(moveCoroutine);
        moveCoroutine = null;
        // 예시로 발사체 제거
        Managers.Resource.Destroy(projectile);
    }

}
