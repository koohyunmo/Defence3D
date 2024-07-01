using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnums
{

    public enum UnitGrade
    {
        None = 0,
        Basic = 1,   // 기본 유닛
        Rare = 2,    // 레어
        Ancient = 3, // 고대
        Relic = 4,   // 유물
        Epic = 5,    // 서사
        Legendary = 6, // 전설
        Mythic = 7,    // 에픽
        Mythical = 8,  // 신화
        Primal = 9     // 태초
    }

    public enum WeaponType
    {
        None = 0,
        Sword = 1,
        Bow = 2,
        Axe = 3
    }

    public enum ProjectileType
    {
        None = 0,
        Arrow = 1,
        Slash = 2,
        Smash = 3,
    }
}
