using System.Collections.Generic;
using System.IO;
using UnityEngine;




public class CSVParser
{

    [System.Serializable]
    public class CSVData
    {
        public CSVData(params string[] values)
        {
            if (values.Length >= 3)
            {
                key = values[0];
                nameKey = values[1];
                modelKey = values[2];
            }
            else
            {
                Debug.LogError("Insufficient data to create CSVData.");
            }
        }

        public void PrintData()
        {
            Debug.Log($"{key} {nameKey} {modelKey}");
        }

        string key;
        string nameKey;
        string modelKey;
    }

    public List<CSVData> items = new List<CSVData>();

    public void ParseData()
    {
        LoadCSV();
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
                readData.PrintData();
            }
            Debug.Log("CSV Loaded Successfully");
        }
        else
        {
            Debug.LogError("CSV File not found");
        }


    }
}
