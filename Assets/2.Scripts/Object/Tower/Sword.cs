using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class Sword : Weapon
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] UnitGrade grade;
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
}
