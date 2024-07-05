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

    protected override void AttackDetail()
    {
        var projectile = Managers.Resource.Instantiate("Arrow", pooling: true);
        projectile.GetComponent<Projectile>().Fire(targetEnemy, transform.position, totalDamage, weaponData.range, this);
    }
}
