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

    public void DisplayHP(int stageCount)
    {
        Debug.Log($"{stageCount}_STAGE HP : " + CalculateHP(stageCount));
    }
}
