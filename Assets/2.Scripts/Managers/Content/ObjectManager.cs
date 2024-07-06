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

    private GameObject weaponRoot = null;
    private GameObject[,] weaponObjectDict;
    private int[] weaponCounter;
    List<Action> updateUiList = new();

    public Player Player { get; private set; }
    private static int objId = 1; // 오브젝트 카운팅

    public void Init()
    {
        weaponObjectDict = new GameObject[(int)WeaponType.MAX_COUNT, (int)UnitGrade.MAX_COUNT];
        weaponCounter = new int[(int)UnitGrade.MAX_COUNT];
        weaponRoot = new GameObject("WeaponRoot");

        CSVParser parser = new CSVParser();
        parser.ParseData();
        WeaponCaching(parser.GetParseData());
    }

    private void WeaponCaching(List<CSVData> dataList)
    {
        if (dataList == null)
        {
            Debug.LogError("Data is Null");
            return;
        }

        foreach (var data in dataList)
        {
            if (!data.DataOk()) continue;

            var itemPrefab = Managers.Resource.Instantiate(data.key);
            var modelPrefab = Managers.Resource.Instantiate(data.modelKey);

            if (itemPrefab == null || modelPrefab == null)
            {
                Debug.LogError($"Prefab is null: itemPrefab - {itemPrefab == null}, modelPrefab - {modelPrefab == null}");
                continue;
            }

            if (data.itemType == (int)WeaponType.Bow)
            {
                var itemMeshRenderer = itemPrefab.GetComponentInChildren<MeshRenderer>();
                var itemMeshFilter = itemPrefab.GetComponentInChildren<MeshFilter>();
                var modelMeshRenderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
                if (itemMeshRenderer && modelMeshRenderer && itemMeshFilter)
                {
                    itemMeshRenderer.material = modelMeshRenderer.sharedMaterial;
                    itemMeshFilter.mesh = modelMeshRenderer.sharedMesh;
                }
                else
                {
                    Debug.LogError($"{data.key} : SkinnedMeshRenderer is null for one of the prefabs.");
                }
            }
            else
            {
                var itemMeshRenderer = itemPrefab.GetComponentInChildren<MeshRenderer>();
                var modelMeshRenderer = modelPrefab.GetComponentInChildren<MeshRenderer>();
                if (itemMeshRenderer != null && modelMeshRenderer != null)
                {
                    itemMeshRenderer.material = modelMeshRenderer.sharedMaterial;
                }
                else
                {
                    Debug.LogError($"{data.key} : MeshRenderer is null for one of the prefabs.");
                }

                var itemMeshFilter = itemPrefab.GetComponentInChildren<MeshFilter>();
                var modelMeshFilter = modelPrefab.GetComponentInChildren<MeshFilter>();
                if (itemMeshFilter != null && modelMeshFilter != null)
                {
                    itemMeshFilter.mesh = modelMeshFilter.mesh;
                }
                else
                {
                    Debug.LogError($"{data.key} : MeshFilter is null for one of the prefabs.");
                }
            }

            GameObject.Destroy(modelPrefab);
            Debug.Log($"{data.nameKey} Created");
            itemPrefab.SetActive(false);
            itemPrefab.transform.SetParent(weaponRoot.transform);
            itemPrefab.name = data.nameKey;
            weaponObjectDict[(int)data.itemType, (int)data.itemGrade] = itemPrefab;
        }
    }


    /*------------
        스폰 관련
    --------------*/
    public void SpawnMosnter()
    {
        if (monsters.Count >= 100) return;

        string prefabName = Managers.Stage.GetStageCount() % 2 == 0 ? "Monster_1" : "Monster_2";
        var go = Managers.Resource.Instantiate(prefabName, pooling: true);
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
        int type = UnityEngine.Random.Range(1, 4);

        WeaponType weaponType = WeaponType.None;
        GameObject prefab = null;

        // 랜덤 무기 타입
        if (type == (int)WeaponType.Sword)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, rankAndColor.Item1]);
            weaponType = WeaponType.Sword;
        }
        else if (type == (int)WeaponType.Bow)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, rankAndColor.Item1]);
            weaponType = WeaponType.Bow;
        }
        else if (type == (int)WeaponType.Axe)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, rankAndColor.Item1]); ;
            weaponType = WeaponType.Axe;
        }

        Debug.Assert(weaponType != WeaponType.None);

        var go = Managers.Resource.Instantiate(prefab);
        go.SetActive(true);
        var weapon = go.GetComponent<Weapon>();

        WeaponData weaponData = null;

        // 등급에 맞는 데이터 생성ㅇ
        switch (weaponType)
        {
            case WeaponType.Sword:
                weaponData = new WeaponData("Sword", GetPowF(rankAndColor.Item1, 1.2f, 2.5f) * 1.5f, Managers.Data.GetSwordDamage(rankAndColor.Item1), rankAndColor.Item1, (UnitGrade)rankAndColor.Item1, objId++);
                break;
            case WeaponType.Bow:
                weaponData = new WeaponData("Bow", GetPowF(rankAndColor.Item1, 1.2f, 2.5f) * 2f, Managers.Data.GetSwordDamage(rankAndColor.Item1) / 2, rankAndColor.Item1 * 5f, (UnitGrade)rankAndColor.Item1, objId++);
                break;
            case WeaponType.Axe:
                weaponData = new WeaponData("Axe", GetPowF(rankAndColor.Item1, 1.2f, 2.5f), Managers.Data.GetSwordDamage(rankAndColor.Item1) * 2, rankAndColor.Item1 * 0.5f, (UnitGrade)rankAndColor.Item1, objId++);
                break;
        }

        Debug.Assert(weaponData != null);
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

        weapon.Spawn(weaponData);
        WeaponDict[weaponType].Add(weapon);
        weaponCounter[(int)rankAndColor.Item1]++;
        UpdateUInvoke();
        return weapon;
    }

    public GridObject TestSpawnWeapon(UnitGrade grade)
    {
        var unitColor = Managers.Data.GetColorForGrade(grade);
        int type = UnityEngine.Random.Range(1, 4);
        WeaponType weaponType = WeaponType.None;
        GameObject prefab = null;

        // 랜덤 무기 타입
        if (type == (int)WeaponType.Sword)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, (int)grade]);
            weaponType = WeaponType.Sword;
        }
        else if (type == (int)WeaponType.Bow)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, (int)grade]);
            weaponType = WeaponType.Bow;
        }
        else if (type == (int)WeaponType.Axe)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, (int)grade]);
            weaponType = WeaponType.Axe;
        }

        var go = GameObject.Instantiate(prefab);
        go.SetActive(true);
        var weapon = go.GetComponent<Weapon>();
        WeaponData weaponData = null;

        // 등급에 맞는 데이터 생성
        switch (weaponType)
        {
            case WeaponType.Sword:
                weaponData = new WeaponData("Sword", GetPowF((int)grade, 1.2f, 2.5f) * 1.5f, Managers.Data.GetSwordDamage((int)grade), (int)grade, grade, objId++);
                break;
            case WeaponType.Bow:
                weaponData = new WeaponData("Bow", GetPowF((int)grade, 1.2f, 2.5f) * 2f, Managers.Data.GetSwordDamage((int)grade) / 2, (int)grade * 5f, grade, objId++);
                break;
            case WeaponType.Axe:
                weaponData = new WeaponData("Axe", GetPowF((int)grade, 1.2f, 2.5f), Managers.Data.GetSwordDamage((int)grade) * 2, (int)grade * 0.5f, grade, objId++);
                break;
        }
        Debug.Assert(weaponData != null);

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


        weapon.Spawn(weaponData);
        WeaponDict[weaponType].Add(weapon);
        weaponCounter[(int)grade]++;
        UpdateUInvoke();

        return weapon;
    }
    /*------------
        디스폰
    --------------*/
    public bool DespawnSweapon(GridObject gridObj)
    {
        Weapon weapon = gridObj as Weapon;
        Debug.Assert(weapon);

        if (WeaponDict[weapon.WeaponType].Remove(weapon))
        {
            weaponCounter[(int)weapon.Grade]--;
            UpdateUInvoke();
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
        Managers.Resource.Destroy(removeMonster.gameObject);
    }
    /*------------
        제곱계산
    --------------*/
    private int GetPow(int rank, float power, float baseValue)
    {
        return (int)(baseValue * Mathf.Pow(power, rank));
    }
    private float GetPowF(int rank, float power, float baseValue)
    {
        return baseValue * Mathf.Pow(power, rank);
    }
    /*------------
        게터 세터
    --------------*/
    public void SetPlayer(Player player)
    {
        Player = player;
    }

    public int GetWeaponCount(UnitGrade unitGrade)
    {
        return weaponCounter[(int)unitGrade];
    }
    public int GetWeaponTotalCount()
    {
        return WeaponDict[WeaponType.Sword].Count + WeaponDict[WeaponType.Bow].Count + WeaponDict[WeaponType.Axe].Count;
    }

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

    public void RegisterUpdateUI(Action evt)
    {
        updateUiList.Add(evt);
    }
    public void RemoveUpdateUI(Action evt)
    {
        updateUiList.Remove(evt);
    }

    /*-----------------
      UI Update
    -------------------*/
    private void UpdateUInvoke()
    {
        foreach(var evt in updateUiList)
        {
            evt.Invoke();
        }
    }

}
