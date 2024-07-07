using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MyEnums;
using static MyDefine;
using System;

public class ReinforceManager
{
    public bool Reinforce(Weapon selectedWeapon, Weapon target)
    {
        int currentLevel = Mathf.Max(target.Level, selectedWeapon.Level);
        if(currentLevel == WEAPON_REINFORCE_NAX_LEVEL) return false;

        int materialLevel1 = selectedWeapon.Level;
        int materialLevel2 = target.Level;

        if (TryEnhance(currentLevel, materialLevel1, materialLevel2) == false)
        {
            // 선택된 무기가 타겟보다 레벨이 높으면 타겟에 데이터 옮기기
            if (target.Level < selectedWeapon.Level)
            {
                target.ChangeWeaponData(selectedWeapon.GetWeaponData());
            }
            Debug.Log("강화 실패");
            return false;
        }

        if (target.Level >= selectedWeapon.Level)
        {
            target.Upgrade();
        }
        else
        {
            selectedWeapon.Upgrade();
            target.ChangeWeaponData(selectedWeapon.GetWeaponData());
        }

        // 레벨에 따른 스케일 조정 (1.1을 레벨 횟수만큼 곱한 효과를 주기)
        float scaleFactor = Mathf.Pow(1.05f, target.Level);
        target.transform.GetChild(0).localScale = target.transform.GetChild(0).localScale * scaleFactor;

        Debug.Log("강화 성공 : " + target.Level);
        Managers.Effect.UpgradeSucess(target.transform);
        return true;
    }

    private bool TryEnhance(int currentLevel, int materialLevel1, int materialLevel2)
    {
        float successProbability = CalculateSuccessProbability(currentLevel, materialLevel1, materialLevel2);
        float roll = UnityEngine.Random.Range(0f, 100f);

        Debug.Log($"성공확률 {materialLevel1} & {materialLevel2} : {successProbability}%");

        return roll < successProbability;
    }

    private float CalculateSuccessProbability(int currentLevel, int materialLevel1, int materialLevel2)
    {
        float baseProbability = REINFORCE_BASE_PROBABILITY;
        float minProbability = REINFORCE_MIN_PROBABILITY;
        float maxLevel = WEAPON_REINFORCE_NAX_LEVEL;
        float slope = (baseProbability - minProbability) / (maxLevel - 1); // 1강에서 9강까지 선형적으로 감소

        int targetLevel = currentLevel + 1;

        // Calculate average material level
        float avgMaterialLevel = (materialLevel1 + materialLevel2) / 2.0f;

        // Calculate linear reduction in probability
        float successProbability = baseProbability - (targetLevel - 1) * slope;

        // Adjust success probability based on material levels
        if (avgMaterialLevel >= targetLevel - 1)
        {
            successProbability = baseProbability - (targetLevel - 1) * slope;
        }
        else
        {
            successProbability -= (targetLevel - 1 - avgMaterialLevel) * slope;
        }

        return Mathf.Clamp(successProbability, minProbability, baseProbability); // Ensure probability is between minProbability and baseProbability
    }

    public float GetPercent(int materialLevel1, int materialLevel2)
    {
        int currentLevel = Mathf.Max(materialLevel1,materialLevel2);

        float baseProbability = 75f;
        float minProbability = 3f;
        float maxLevel = 9;
        float slope = (baseProbability - minProbability) / (maxLevel - 1); // 1강에서 9강까지 선형적으로 감소

        int targetLevel = currentLevel + 1;

        // Calculate average material level
        float avgMaterialLevel = (materialLevel1 + materialLevel2) / 2.0f;

        // Calculate linear reduction in probability
        float successProbability = baseProbability - (targetLevel - 1) * slope;

        // Adjust success probability based on material levels
        if (avgMaterialLevel >= targetLevel - 1)
        {
            successProbability = baseProbability - (targetLevel - 1) * slope;
        }
        else
        {
            successProbability -= (targetLevel - 1 - avgMaterialLevel) * slope;
        }

        return Mathf.Clamp(successProbability, minProbability, baseProbability); // Ensure probability is between minProbability and baseProbability
    }
    public void TestCode()
    {
        int tri = 500;
        for (int level = 1; level < WEAPON_REINFORCE_NAX_LEVEL; level++)
        {
            for(int level2 = 1; level2 < WEAPON_REINFORCE_NAX_LEVEL; level2++)
            {
                int successCount = 0;
                int failureCount = 0;

                int materialLevel1 = level;
                int materialLevel2 = level2;

                for (int i = 0; i < tri; i++)
                {
                    int currentLevel = Math.Max(materialLevel1, materialLevel2);
                    if(currentLevel == WEAPON_REINFORCE_NAX_LEVEL) continue;

                    if (TryEnhance(currentLevel, materialLevel1, materialLevel2))
                    {
                        successCount++;
                    }
                    else
                    {
                        failureCount++;
                    }
                }

                float successRate = (successCount / (float)tri) * 100;
                float failureRate = (failureCount / (float)tri) * 100;

                Debug.Log($"레벨 {materialLevel1} : {materialLevel2} - 성공 횟수: {successCount}, 실패 횟수: {failureCount}");
                Debug.Log($"레벨 {materialLevel1} : {materialLevel2} - 성공 확률: {successRate}%, 실패 확률: {failureRate}%");
            }
        }
    }
}
