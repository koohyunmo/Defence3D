using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class Axe : Weapon
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] UnitGrade grade;
    public override void Spawn(WeaponData data)
    {
        base.Spawn(data);
        weaponType = WeaponType.Axe;
        grade = data.grade;

    }

    protected override void PlayAnim()
    {
        anim.Play("Slash_1", -1, 0);
    }
}
