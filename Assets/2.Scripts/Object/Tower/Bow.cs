using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class Bow : Weapon
{
    [SerializeField] float rotationSpeed = 360f; // 회전 속도 증가

    public override void Spawn(WeaponData data)
    {
        base.Spawn(data);
        WeaponType = WeaponType.Bow;
        Grade = data.grade;

        LookAtPosition(Managers.Grid.GetPath(0), transform.GetChild(0));
    }

    protected override void PlayAnim()
    {
        // 타겟이 없으면 리턴
        if (targetEnemy == null)
        {
            Debug.LogWarning("TargetEnemy is null");
            return;
        }
        if(anim == null)
        {
            anim = gameObject.GetComponentInChildren<Animator>();
        }
        anim.speed = weaponData.fireRate;
        // 활의 매쉬 모델을 가져옴
        Transform bowMesh = transform.GetChild(0);
        LookAtTarget(targetEnemy.transform, bowMesh);
    }

    protected override void LookAtTarget(Transform target, Transform model)
    {
        if (target == null) return;
        if (model == null) return;

        // 타겟의 방향을 계산
        Vector3 directionToTarget = (target.position - model.position).normalized;

        // 타겟을 향한 회전값 계산
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        // 모델의 회전
        model.rotation = targetRotation;

    }

    protected override void AttackDetail()
    {
        var projectile = Managers.Resource.Instantiate("Arrow", pooling: true);
        projectile.GetComponent<Projectile>().Fire(targetEnemy, transform.position, totalDamage, weaponData.range, this);
    }
}
