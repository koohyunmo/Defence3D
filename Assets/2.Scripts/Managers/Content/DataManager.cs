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
            case UnitGrade.Basic: return 16; ;
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
}
