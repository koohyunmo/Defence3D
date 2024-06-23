using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class Bow : Weapon
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] UnitGrade grade;
    public override void Spawn(WeaponData data)
    {
        base.Spawn(data);
        weaponType = WeaponType.Bow;
        grade = data.grade;
    }
}
