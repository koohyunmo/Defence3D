using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class RandomManager
{
    short[] probability = new short[10000];

    public void Init()
    {
        // 배열 초기화 (예시)
        for (int i = 0; i < 5000; i++) probability[i] = 1; // 기본 유닛
        for (int i = 5000; i < 8310; i++) probability[i] = 2; // 레어
        for (int i = 8310; i < 9330; i++) probability[i] = 3; // 고대
        for (int i = 9330; i < 9840; i++) probability[i] = 4; // 유물
        for (int i = 9840; i < 9920; i++) probability[i] = 5; // 서사
        for (int i = 9920; i < 9970; i++) probability[i] = 6; // 전설
        for (int i = 9970; i < 9990; i++) probability[i] = 7; // 에픽
        for (int i = 9990; i < 9998; i++) probability[i] = 8; // 신화
        for (int i = 9998; i < 10000; i++) probability[i] = 9; // 태초
    }

    private float GetProbabilityPercentage(UnitGrade grade)
    {
        switch (grade)
        {
            case UnitGrade.Basic: return 50.0f;
            case UnitGrade.Rare: return 33.1f;
            case UnitGrade.Ancient: return 10.2f;
            case UnitGrade.Relic: return 5.1f;
            case UnitGrade.Epic: return 0.8f;
            case UnitGrade.Legendary: return 0.5f;
            case UnitGrade.Mythic: return 0.2f;
            case UnitGrade.Mythical: return 0.08f;
            case UnitGrade.Primal: return 0.019f;
            default: return 0.0f;
        }
    }

    public Color GetColorForGrade(UnitGrade grade)
    {
        switch (grade)
        {
            case UnitGrade.Basic: return Color.white; // White
            case UnitGrade.Rare: return Color.green; // Green
            case UnitGrade.Ancient: return new Color(0f, 0.75f, 1f); // Sky Blue
            case UnitGrade.Relic: return new Color(0.5f, 0f, 0.5f); // Purple
            case UnitGrade.Epic: return new Color(1f, 0.5f, 0f); // Orange
            case UnitGrade.Legendary: return new Color(1f, 0.84f, 0f); // Gold
            case UnitGrade.Mythic: return new Color(0.8f, 0f, 0f); // Dark Red
            case UnitGrade.Mythical: return new Color(1f, 0.2f, 0.6f); // Hot Pink
            case UnitGrade.Primal: return new Color(0f, 0.5f, 1f); // Deep Cyan
            default: return Color.black; // Black
        }
    }

    public (int, Color) GetTestColorAndRank()
    {
        int rnd = Random.Range(0, probability.Length);
        UnitGrade selectedGrade = (UnitGrade)probability[rnd];

        return ((int)selectedGrade, GetColorForGrade(selectedGrade));
    }

    public int TestRandom()
    {
        int rnd = Random.Range(0, probability.Length);
        UnitGrade selectedGrade = (UnitGrade)probability[rnd];
        float probabilityPercentage = GetProbabilityPercentage(selectedGrade);
        Debug.Log("Selected Unit Grade: " + selectedGrade + ", Probability: " + probabilityPercentage + "%");
        return (int)selectedGrade;
    }

    private void Test()
    {
        for (int i = 0; i < 100000; i++)
        {
            int rnd = Random.Range(0, probability.Length);
            UnitGrade selectedGrade = (UnitGrade)probability[rnd];
            float probabilityPercentage = GetProbabilityPercentage(selectedGrade);
            Debug.Log("Selected Unit Grade: " + selectedGrade + ", Probability: " + probabilityPercentage + "%");
        }
    }
}
