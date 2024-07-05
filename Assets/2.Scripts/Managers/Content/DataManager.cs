using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class DataManager
{

    public int GetSwordDamage(int grade)
    {
        Debug.Assert((int)grade >= 1);

        UnitGrade unitGrade = (UnitGrade)grade;

        switch (unitGrade)
        {
            case UnitGrade.Basic : return 16;;
            case UnitGrade.Rare: return 20;
            case UnitGrade.Ancient: return 30;
            case UnitGrade.Relic: return 60;
            case UnitGrade.Epic: return 72;
            case UnitGrade.Legendary: return 87;
            case UnitGrade.Mythic: return 45;
            case UnitGrade.Mythical: return 152;
            case UnitGrade.Primal: return 160;
        }

        return -1;
    }

    public int GetSwordDamage(UnitGrade grade)
    {
        Debug.Assert((int)grade >= 1);

        switch (grade)
        {
            case UnitGrade.Basic: return 16;
            case UnitGrade.Rare: return 20;
            case UnitGrade.Ancient: return 30;
            case UnitGrade.Relic: return 60;
            case UnitGrade.Epic: return 72;
            case UnitGrade.Legendary: return 87;
            case UnitGrade.Mythic: return 45;
            case UnitGrade.Mythical: return 152;
            case UnitGrade.Primal: return 160;
        }

        return -1;
    }

    public int CalculateHP(float level)
    {
        float a = 0.1151f;
        float b = -5.6385f;
        float c = 105.85f;
        float d = -306.88f;

        float y = a * Mathf.Pow(level, 3) + b * Mathf.Pow(level, 2) + c * level + d;
        y = Mathf.Max(10, y);
        return (int)y;
    }

    public void DisplayStageHPAndDPS(int stageCount)
    {
        float swordDPS = 0;
        foreach(var w in Managers.Object.WeaponDict[WeaponType.Sword])
        {
            swordDPS += w.GetWeaponData().damage * w.GetWeaponData().fireRate;
        }
        float bowDPS = 0;
        foreach (var w in Managers.Object.WeaponDict[WeaponType.Bow])
        {
            bowDPS += w.GetWeaponData().damage * w.GetWeaponData().fireRate;
        }
        float axeDPS = 0;
        foreach (var w in Managers.Object.WeaponDict[WeaponType.Axe])
        {
            axeDPS += w.GetWeaponData().damage * w.GetWeaponData().fireRate;
        }

        Debug.Log($"Player DPS {swordDPS + bowDPS + axeDPS} { stageCount}_STAGE HP : {CalculateHP(stageCount)} KPS : {(swordDPS + bowDPS + axeDPS) / CalculateHP(stageCount)}");
        Debug.Log($"swordDPS : {swordDPS}");
        Debug.Log($"bowDPS : {bowDPS}");
        Debug.Log($"axeDPS : {axeDPS}");
    }

    public int GetSellGold(UnitGrade grade)
    {
        Debug.Assert((int)grade >= 1);

        switch (grade)
        {
            case UnitGrade.Basic: return 5;
            case UnitGrade.Rare: return 15;
            case UnitGrade.Ancient: return 40;
            case UnitGrade.Relic: return 80;
            case UnitGrade.Epic: return 160;
            case UnitGrade.Legendary: return 320;
            case UnitGrade.Mythic: return 640;
            case UnitGrade.Mythical: return 960;
            case UnitGrade.Primal: return 1250;
        }

        return -1;
    }
}
