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

    private Dictionary<UnitGrade, float> gambleProbabilities; // 고급 확률표

    private List<UnitGrade> grades;
    private List<float> cumulativeProbabilities; // 기본 누적확률
    private List<float> cumulativeGambleProbabilities; // 고급 누적확률

    public void Init()
    {
        grades = new List<UnitGrade>(probabilities.Keys);
        cumulativeProbabilities = new List<float>(grades.Count);
        cumulativeGambleProbabilities = new List<float>(grades.Count);
        gambleProbabilities = new Dictionary<UnitGrade, float>(probabilities); // 확률 복사

        float cumulative = 0f;
        foreach (var grade in grades)
        {
            cumulative += probabilities[grade];
            cumulativeProbabilities.Add(cumulative);
            cumulativeGambleProbabilities.Add(cumulative);
        }

        for (int i = 0; i < 40; i++)
        {
            GambleUpgrade();
        }
    }

    /*----------------
      확률 업그레이드
    ------------------*/
    /// <summary>
    /// 기본 확률 업그레이드
    /// </summary>
    public void Upgrade()
    {
        float basicDecreaseRate = 0.9f; // Basic 확률 감소율
        float basicCurrentProbability = probabilities[UnitGrade.Basic];
        float totalOtherProbability = 100.0f - basicCurrentProbability;
        float totalCurrentOtherProbability = 100.0f - (basicCurrentProbability * basicDecreaseRate);

        probabilities[UnitGrade.Basic] *= basicDecreaseRate;

        foreach (var grade in grades)
        {
            if (grade != UnitGrade.Basic)
            {
                probabilities[grade] = probabilities[grade] / totalOtherProbability * totalCurrentOtherProbability;
            }
        }

        // 누적 확률 재계산
        cumulativeProbabilities.Clear();
        float cumulative = 0f;
        foreach (var grade in grades)
        {
            cumulative += probabilities[grade];
            cumulativeProbabilities.Add(cumulative);
            Debug.Log($"{grade} 확률 : {probabilities[grade]}");
        }
    }

    /// <summary>
    /// 고급 확률 업그레이드
    /// </summary>
    private void GambleUpgrade()
    {
        float basicDecreaseRate = 0.9f; // Basic 확률 감소율
        float basicCurrentProbability = gambleProbabilities[UnitGrade.Basic];
        float totalOtherProbability = 100.0f - basicCurrentProbability;
        float totalCurrentOtherProbability = 100.0f - (basicCurrentProbability * basicDecreaseRate);

        gambleProbabilities[UnitGrade.Basic] *= basicDecreaseRate;

        foreach (var grade in grades)
        {
            if (grade != UnitGrade.Basic)
            {
                gambleProbabilities[grade] = gambleProbabilities[grade] / totalOtherProbability * totalCurrentOtherProbability;
            }
        }

        // 누적 확률 재계산
        cumulativeGambleProbabilities.Clear();
        float cumulative = 0f;
        foreach (var grade in grades)
        {
            cumulative += gambleProbabilities[grade];
            cumulativeGambleProbabilities.Add(cumulative);
            //Debug.Log($"도박 {grade} 확률 : {gambleProbabilities[grade]}");
        }
    }

    /*----------------
        확률 검색
    ------------------*/
    private UnitGrade GetRandomGrade()
    {
        float randomValue = Random.Range(0f, 100f);

        int index = cumulativeProbabilities.BinarySearch(randomValue);
        if (index < 0)
        {
            index = ~index; //못찾은 경우
        }

        return grades[index];
    }

    private UnitGrade GetGambleRandomGrade()
    {
        float randomValue = Random.Range(0f, 100f);

        int index = cumulativeGambleProbabilities.BinarySearch(randomValue);
        if (index < 0)
        {
            index = ~index; //못찾은 경우
        }

        return grades[index];
    }

    /*----------------
        게터 세터
    ------------------*/

    public bool CanUpgrade()
    {
        return probabilities[UnitGrade.Basic] > 1f;
    }

    public int GetGambleRank()
    {
        return (int)GetGambleRandomGrade();
    }

    public int GetRank()
    {
        return (int)GetRandomGrade();
    }

    /*----------------
        확률 테스트
    ------------------*/

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

    /*----------------
            클리어
    ------------------*/

    public void Clear()
    {
        probabilities.Clear();

        probabilities = new Dictionary<UnitGrade, float>()
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

        grades = new List<UnitGrade>(probabilities.Keys);
        cumulativeProbabilities = new List<float>(grades.Count);
        cumulativeGambleProbabilities = new List<float>(grades.Count);
        gambleProbabilities = new Dictionary<UnitGrade, float>(probabilities); // 확률 복사

        float cumulative = 0f;
        foreach (var grade in grades)
        {
            cumulative += probabilities[grade];
            cumulativeProbabilities.Add(cumulative);
            cumulativeGambleProbabilities.Add(cumulative);
        }

        for (int i = 0; i < 40; i++)
        {
            GambleUpgrade();
        }

    }
}
