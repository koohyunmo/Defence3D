using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class Sword : Weapon
{

    public override void Spawn(WeaponData data)
    {
        base.Spawn(data);
        weaponType = WeaponType.Sword;
        grade = data.grade;

    }
    protected override void PlayAnim()
    {
        anim.Play("Slash_1", -1, 0);
    }
    protected override void AttackDetail()
    {
        var projectile = Managers.Resource.Instantiate("Slash", pooling: true);
        projectile.GetComponent<Projectile>().Fire(targetEnemy, transform.position, totalDamage, weaponData.range);
    }
}
