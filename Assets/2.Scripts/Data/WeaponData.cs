using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public string weaponName;  // 타워의 이름
    public float range;  // 타워의 공격 범위
    public int damage;  // 타워의 공격력
    public float fireRate;  // 타워의 공격 속도 (초당 공격 횟수)
    public int level;  // 타워의 현재 레벨

    public WeaponData(string name, float range, int damage, float fireRate)
    {
        this.weaponName = name;
        this.range = range;
        this.damage = damage;
        this.fireRate = fireRate;
        this.level = 1;  // 초기 레벨은 1로 설정
    }

    public WeaponData(WeaponData data)
    {
        this.weaponName = data.weaponName;
        this.range = data.range;
        this.damage = data.damage;
        this.fireRate = data.fireRate;
        this.level = data.level;  // 초기 레벨은 1로 설정
    }

    // 타워 업그레이드 메서드
    public void Upgrade()
    {
        level++;
        damage = (int)(damage *1.2f);  // 공격력 20% 증가
        range *= 1.1f;  // 사거리 10% 증가
        fireRate *= 1.1f;  // 공격 속도 10% 증가
    }
}
