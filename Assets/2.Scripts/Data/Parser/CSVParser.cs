using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MyEnums;


[System.Serializable]
public class CSVData
{
    public CSVData(params string[] values)
    {
        key = values[1];
        nameKey = values[2];
        modelKey = values[3];
        effectKey = values[4];
        itemType = int.Parse(values[5]);
        itemGrade = int.Parse(values[6]);
    }

    public bool DataOk()
    {
        if (string.IsNullOrEmpty(key)) return false;
        if (string.IsNullOrEmpty(nameKey)) return false;
        if (string.IsNullOrEmpty(modelKey)) return false;
        if (string.IsNullOrEmpty(effectKey)) return false;

        return true;
    }

    public void PrintData()
    {
        Debug.Log($"{key} {nameKey} {modelKey} {effectKey} {itemType} {itemGrade}");
    }

    public string key;
    public string nameKey;
    public string modelKey;
    public string effectKey;
    public int itemType;
    public int itemGrade;
}

public class CSVParser
{
    public List<CSVData> items = new List<CSVData>();

    public void ParseData()
    {
        LoadCSV();
    }
    public List<CSVData> GetParseData()
    {
        return items;
    }

    void LoadCSV()
    {
        TextAsset weaponData = Managers.Resource.Load<TextAsset>("WeaponData");
        
        if (weaponData)
        {
            string[] csvLines = weaponData.text.Split('\n');
            for (int i = 1; i < csvLines.Length; i++) // Skip header line
            {
                string line = csvLines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] values = csvLines[i].Split(',');

                CSVData readData = new CSVData(values);
                
                if(readData.DataOk())
                    items.Add(readData);
            }
            Debug.Log("CSV Loaded Successfully");
        }
        else
        {
            Debug.LogError("CSV File not found");
        }


    }
}
