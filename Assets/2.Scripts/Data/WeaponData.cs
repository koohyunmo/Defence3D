using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;
// 칼 16 등급별 +4
// 활 6
// 도끼 10 
// 강화시 2배씩 증가
public class WeaponData
{
    public string weaponName;  // 타워의 이름
    public float range;  // 타워의 공격 범위
    public int damage;  // 타워의 공격력
    public float fireRate;  // 타워의 공격 속도 (초당 공격 횟수)
    public int level;  // 타워의 현재 레벨
    public UnitGrade grade;
    public int id;

    public WeaponData(string name, float range, int damage, float fireRate,UnitGrade grade, int id)
    {
        this.weaponName = name;
        this.range = range;
        this.damage = damage;
        this.fireRate = 1/fireRate;
        this.level = 1;  // 초기 레벨은 1로 설정
        this.grade = grade;
        this.id = id;
    }

    public WeaponData(WeaponData data)
    {
        this.weaponName = data.weaponName;
        this.range = data.range;
        this.damage = data.damage;
        this.fireRate = 1/data.fireRate;
        this.level = data.level;  // 초기 레벨은 1로 설정
        this.grade = data.grade;
        this.id = data.id;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
 

        WeaponData weaponData = obj as WeaponData;

        if (this.id == weaponData.id)
        {
            return false;
        }

        if (weaponData.grade == this.grade && weaponData.weaponName.Equals(this.weaponName))
        {
            return true;
        }

        return false;
    }
    
    // 타워 업그레이드 메서드
    public void Upgrade()
    {
        level++;
        damage = (int)(damage *2f);  // 공격력 200% 증가
        range *= 1.2f;  // 사거리 20% 증가
        fireRate *= 1.2f;  // 공격 속도 20% 증가
    }
}
