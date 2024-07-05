using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using static MyEnums;

public class ObjectManager
{
    private HashSet<Monster> monsters = new HashSet<Monster>();
    public Dictionary<WeaponType, List<Weapon>> WeaponDict { get; private set; } = new Dictionary<WeaponType, List<Weapon>>()
    {
        { WeaponType.Sword, new List<Weapon>() },
        { WeaponType.Bow, new List<Weapon>() },
        { WeaponType.Axe, new List<Weapon>() }
    };

    private GameObject[,] weaponObjectDict;
    public void Init()
    {
        weaponObjectDict = new GameObject[(int)WeaponType.MAX_COUNT, (int)UnitGrade.MAX_COUNT];

        CSVParser parser = new CSVParser();
        parser.ParseData();
        WeaponCaching(parser.GetParseData());
    }

    private void WeaponCaching(List<CSVData> dataList)
    {
        if(dataList == null)
        {
            Debug.LogError("Data is Null");
        }
        foreach (var data in dataList)
        {
            if (data.DataOk() == false) continue;

            var itemPrefab = Managers.Resource.Instantiate(data.key);
            var modelPrefab = Managers.Resource.Instantiate(data.modelKey);

            // weaponPrefab 의 Mesh와 Material을 수정합니다.
            if (itemPrefab && modelPrefab)
            {
                // Material 변경
                MeshRenderer itemMeshRenderer = itemPrefab.GetComponentInChildren<MeshRenderer>();
                MeshRenderer modelMeshRenderer = modelPrefab.GetComponentInChildren<MeshRenderer>();
                if (itemMeshRenderer && modelMeshRenderer)
                {
                    itemMeshRenderer.material = modelMeshRenderer.sharedMaterial;
                }
                else
                {
                    Debug.LogError("MeshRenderer weaponPrefab is Null");
                }

                // Mesh 변경
                MeshFilter itemMeshFilter = itemPrefab.GetComponentInChildren<MeshFilter>();
                MeshFilter modelMeshFilter = modelPrefab.GetComponentInChildren<MeshFilter>();
                if (itemMeshFilter && modelMeshFilter)
                {
                    itemMeshFilter.mesh = modelMeshFilter.mesh;
                }
                else
                {
                    Debug.LogError("MeshFilter weaponPrefab is Null");
                }
            }
            else
            {
                Debug.LogError("weaponPrefab is Null");
            }
            GameObject.Destroy(modelPrefab);
            Debug.Log($"{data.nameKey} Created");
            itemPrefab.SetActive(false);
            itemPrefab.name = data.nameKey;
            weaponObjectDict[(int)data.itemType, (int)data.itemGrade] = itemPrefab;
        }
    }

    public Player Player {get; private set;}
    private static int objId = 1;

    public List<Monster> GetMonsters()
    {
        return monsters.ToList();
    }
    public int GetMonsterCount()
    {
        return monsters.Count;
    }
    public string GetMonsterCountStr()
    {
        return monsters.Count.ToString();
    }
    public void SpawnMosnter()
    {
        if(monsters.Count >= 100) return;

        string prefabName = Managers.Stage.GetStageCount() % 2 == 0 ? "Monster_1" : "Monster_2";
        var go = Managers.Resource.Instantiate(prefabName, pooling:true);
        var monster = go.GetComponent<Monster>();
        monster.transform.localScale = Vector3.one;
        monsters.Add(monster);
        monster.Spawn(new MonsterData("Slime", Managers.Data.CalculateHP(Managers.Stage.GetStageCount()), 2));
    }

    public void SpawnBoss()
    {
        string prefabName = Managers.Stage.GetStageCount() % 2 == 0 ? "Monster_1" : "Monster_2";
        var go = Managers.Resource.Instantiate(prefabName, pooling: true);
        var bossMonster = go.GetComponent<Monster>();
        bossMonster.transform.localScale = Vector3.one * 2.5f; 
        monsters.Add(bossMonster);
        bossMonster.Spawn(new MonsterData("Slime", Managers.Data.CalculateHP(Managers.Stage.GetStageCount() + 5), 2));
    }

    public GridObject SpawnWeapon()
    {
        var rankAndColor = Managers.Random.GetTestColorAndRank();
        int type = UnityEngine.Random.Range(1,4);

        WeaponType weaponType = WeaponType.None;
        GameObject prefab = null;

        if (type == (int)WeaponType.Sword)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type,rankAndColor.Item1]);
            weaponType = WeaponType.Sword;
        }
        else if(type == (int)WeaponType.Bow)
        {
           prefab = Managers.Resource.Load<GameObject>("Bow1");
            weaponType = WeaponType.Bow;
        }
        else if(type == (int)WeaponType.Axe)
        {
            prefab = Managers.Resource.Load<GameObject>("Axe1");
            weaponType = WeaponType.Axe;
        }

        Debug.Assert(weaponType != WeaponType.None);

        var go = Managers.Resource.Instantiate(prefab);
        go.SetActive(true);

        var weapon = go.GetComponent<Weapon>();

        WeaponData weaponData = null;

        switch (weaponType)
        {
            case WeaponType.Sword:
                weaponData = new WeaponData("Sword", GetPowF(rankAndColor.Item1, 1.2f, 2.5f) * 1.5f, Managers.Data.GetSwordDamage(rankAndColor.Item1), rankAndColor.Item1, (UnitGrade)rankAndColor.Item1,objId++);
                break;
            case WeaponType.Bow:
                weaponData = new WeaponData("Bow", GetPowF(rankAndColor.Item1, 1.2f, 2.5f) * 2f, Managers.Data.GetSwordDamage(rankAndColor.Item1) / 2, rankAndColor.Item1 * 5f, (UnitGrade)rankAndColor.Item1, objId++);
                break;
            case WeaponType.Axe:
                weaponData = new WeaponData("Axe", GetPowF(rankAndColor.Item1, 1.2f, 2.5f), Managers.Data.GetSwordDamage(rankAndColor.Item1) * 2, rankAndColor.Item1 * 0.5f, (UnitGrade)rankAndColor.Item1, objId++);
                break;
        }

        Debug.Assert(weaponData != null);

        weapon.Spawn(weaponData);
        WeaponDict[weaponType].Add(weapon);

        // 자식 객체에서 Renderer를 찾기
        // TODO 모델과 이펙트 교체
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = rankAndColor.Item2;
            Debug.Log($"{(UnitGrade)rankAndColor.Item1} : {weaponData.damage} : {weaponData.range}");
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the instantiated weapon.");
        }

        return weapon;
    }

    public GridObject SpawnWeapon(UnitGrade grade)
    {
        var unitColor = Managers.Random.GetColorForGrade(grade);
        var prefab = Managers.Resource.Load<GameObject>("Sword1");
        var go = GameObject.Instantiate(prefab);
        var weapon = go.GetComponent<Weapon>();

        //ew WeaponData("Sword1",2.5f,50,0.5f);
        WeaponData weaponData = new WeaponData("Sword", GetPowF((int)grade, 1.2f, 2.5f), Managers.Data.GetSwordDamage(grade), (int)grade, grade,objId++);
        weapon.Spawn(weaponData);

        // 자식 객체에서 Renderer를 찾기
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = unitColor;
            Debug.Log($"{grade} : {weaponData.damage} : {weaponData.range}");
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the instantiated weapon.");
        }

        return weapon;
    }

    public bool DespawnSweapon(GridObject gridObj)
    {
        Weapon weapon = gridObj as Weapon;
        Debug.Assert(weapon);

        if(WeaponDict[weapon.WeaponType].Remove(weapon))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DespawnMonster(Monster removeMonster)
    {
        monsters.Remove(removeMonster);
    }

    private int GetPow(int rank,float power, float baseValue)
    {
        return (int)(baseValue * Mathf.Pow(power, rank));
    }
    private float GetPowF(int rank, float power, float baseValue)
    {
        return baseValue * Mathf.Pow(power, rank);
    }

    public void SetPlayer(Player player)
    {
        Player = player;
    }
}
