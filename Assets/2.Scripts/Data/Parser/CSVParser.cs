using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MyEnums;

[System.Serializable]
public class CSVItem
{

    public string key;
    public string nameKey;
    public string modelKey;

    public CSVItem(params string[] values)
    {
        key = values[1];
        nameKey = values[2];
        modelKey = values[3];
    }

    public virtual bool DataOk()
    {
        if (string.IsNullOrEmpty(key)) return false;
        if (string.IsNullOrEmpty(nameKey)) return false;
        if (string.IsNullOrEmpty(modelKey)) return false;

        return true;
    }

    public void PrintData()
    {
        Debug.Log($"{key} {nameKey} {modelKey}");
    }

}

[System.Serializable]
public class CSVWeaponData : CSVItem
{
    public CSVWeaponData(params string[] values) : base(values)
    {
        effectKey = values[4];
        itemType = int.Parse(values[5]);
        itemGrade = int.Parse(values[6]);
    }

    public override bool DataOk()
    {
        base.DataOk();
        if (string.IsNullOrEmpty(effectKey)) return false;

        return true;
    }


    public string effectKey;
    public int itemType;
    public int itemGrade;
}

[System.Serializable]
public class CSVMonsterData : CSVItem
{
    public CSVMonsterData(params string[] values) : base(values)
    {
        spawnStage = int.Parse(values[4]);
        endStage = int.Parse(values[5]);
    }
    public int spawnStage;
    public int endStage;
}

public class CSVParser
{
    private List<CSVWeaponData> weaponData = new List<CSVWeaponData>();
    private List<CSVMonsterData> monsterData = new List<CSVMonsterData>();
    private List<CSVMonsterData> bossMonsterData = new List<CSVMonsterData>();
    private readonly string weaponDataPath = "WeaponData";
    private readonly string monsterDataPath = "MonsterData";
    private readonly string bossMonsterDataPath = "BossMonsterData";

    public void ParseData()
    {
        LoadWeaponDataCSV();
        LoadMonsterDataCSV(monsterDataPath,isBoss:false);
        LoadMonsterDataCSV(bossMonsterDataPath,isBoss:true);
    }
    public List<CSVWeaponData> GetParseWeaponData()
    {
        return weaponData;
    }
    public List<CSVMonsterData> GetParseMonsterData()
    {
        return monsterData;
    }
    public List<CSVMonsterData> GetParseBossMonsterData()
    {
        return bossMonsterData;
    }

    private void LoadWeaponDataCSV()
    {
        TextAsset weaponData = Managers.Resource.Load<TextAsset>(weaponDataPath);
        
        if (weaponData)
        {
            string[] csvLines = weaponData.text.Split('\n');
            for (int i = 1; i < csvLines.Length; i++) // Skip header line
            {
                string line = csvLines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] values = csvLines[i].Split(',');

                CSVWeaponData readData = new CSVWeaponData(values);
                
                if(readData.DataOk())
                    this.weaponData.Add(readData);
            }
            Debug.Log($"{weaponDataPath} CSV Loaded Successfully");
        }
        else
        {
            Debug.LogError("CSV File not found");
        }
    }

    private void LoadMonsterDataCSV(string path, bool isBoss)
    {
        TextAsset monsterData = Managers.Resource.Load<TextAsset>(path);

        if(monsterData)
        {
            string[] csvLines = monsterData.text.Split('\n');
            for (int i = 1; i < csvLines.Length; i++) // Skip header line
            {
                string line = csvLines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] values = csvLines[i].Split(',');

                CSVMonsterData readData = new CSVMonsterData(values);

                if (readData.DataOk())
                {
                    if(isBoss)
                    {
                        bossMonsterData.Add(readData);
                    }
                    else
                    {
                        this.monsterData.Add(readData);
                    }
                }
            }
            Debug.Log($"{path} CSV Loaded Successfully");
        }
        else
        {

        }
    }
}
