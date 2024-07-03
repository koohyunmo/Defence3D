using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class Axe : Weapon
{
    public override void Spawn(WeaponData data)
    {
        base.Spawn(data);
        weaponType = WeaponType.Axe;
        grade = data.grade;

    }

    protected override void AttackDetail()
    {
        var projectile = Managers.Resource.Instantiate("Smash", pooling: true);
        projectile.GetComponent<Projectile>().Fire(targetEnemy, transform.position, totalDamage, weaponData.range, this);
    }

    protected override void PlayAnim()
    {
        anim.Play("Slash_1", -1, 0);
    }
}
