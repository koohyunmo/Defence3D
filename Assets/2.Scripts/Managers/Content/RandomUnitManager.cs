using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class RandomManager
{
    private Dictionary<UnitGrade, float> probabilities = new Dictionary<UnitGrade, float>()
    {
        { UnitGrade.Basic, 50.0f },
        { UnitGrade.Rare, 33.1f },
        { UnitGrade.Ancient, 10.2f },
        { UnitGrade.Relic, 5.1f },
        { UnitGrade.Epic, 0.8f },
        { UnitGrade.Legendary, 0.5f },
        { UnitGrade.Mythic, 0.2f },
        { UnitGrade.Mythical, 0.08f },
        { UnitGrade.Primal, 0.019f }
    };

    public void Init() { }

    public void Upgrade()
    {
        float basicDecreaseRate = 0.9f; // Basic 확률 감소율
        float basicCurrentProbability = probabilities[UnitGrade.Basic];
        float totalOtherProbability = 100.0f - basicCurrentProbability;
        float totalCurrentOtherProbability = 100.0f - (basicCurrentProbability * basicDecreaseRate);

        probabilities[UnitGrade.Basic] *= basicDecreaseRate;

        foreach (var grade in probabilities.Keys)
        {
            if (grade != UnitGrade.Basic)
            {
                probabilities[grade] = probabilities[grade] / totalOtherProbability * totalCurrentOtherProbability;
            }
        }
    }

    private UnitGrade GetRandomGrade()
    {
        float randomValue = Random.Range(0f, 100f);
        float cumulativeProbability = 0f;

        foreach (var grade in probabilities.Keys)
        {
            cumulativeProbability += probabilities[grade];
            if (randomValue <= cumulativeProbability)
            {
                return grade;
            }
        }

        return UnitGrade.Basic; // 기본값
    }

    public (int, Color) GetTestColorAndRank()
    {
        UnitGrade selectedGrade = GetRandomGrade();
        return ((int)selectedGrade, Managers.Data.GetColorForGrade(selectedGrade));
    }

    public int TestRandom()
    {
        UnitGrade selectedGrade = GetRandomGrade();
        float probabilityPercentage = probabilities[selectedGrade];
        Debug.Log("Selected Unit Grade: " + selectedGrade + ", Probability: " + probabilityPercentage + "%");
        return (int)selectedGrade;
    }

    private void Test()
    {
        for (int i = 0; i < 100000; i++)
        {
            UnitGrade selectedGrade = GetRandomGrade();
            float probabilityPercentage = probabilities[selectedGrade];
            Debug.Log("Selected Unit Grade: " + selectedGrade + ", Probability: " + probabilityPercentage + "%");
        }
    }
}
