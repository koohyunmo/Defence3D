using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject[] monsterPrefabs = new GameObject[101];
    private GameObject[] bossMonsterPrefabs = new GameObject[11];
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
        WeaponCaching(parser.GetParseWeaponData());
        MonsterCaching(parser.GetParseMonsterData(),isBoss:false);
        MonsterCaching(parser.GetParseBossMonsterData(), isBoss: true);

        CreatePlayer();
    }

    /*-------------
        캐싱
    ---------------*/

    private void WeaponCaching(List<CSVWeaponData> dataList)
    {
        if (dataList == null)
        {
            Debug.LogError("Data is Null");
            return;
        }

        Material toonshader = Managers.Resource.Load<Material>("ToonShader_Outline_Shadow");

        // 데이터를 읽어와서 무기 프리팹 캐싱하기
        foreach (var data in dataList)
        {
            if (data.DataOk() == false) continue;

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
                    //itemMeshRenderer.material = modelMeshRenderer.sharedMaterial;
                    itemMeshRenderer.material = toonshader;
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
                    //itemMeshRenderer.material = modelMeshRenderer.sharedMaterial;
                    itemMeshRenderer.material = toonshader;
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

            // 자식 객체에서 Renderer를 찾기
            // TODO 모델과 이펙트 교체

            Renderer renderer = itemPrefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Managers.Data.GetColorForGrade((UnitGrade)data.itemGrade);
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the instantiated weapon.");
            }
            

            GameObject.Destroy(modelPrefab);
            //Debug.Log($"{data.nameKey} Created");
            itemPrefab.SetActive(false);
            itemPrefab.transform.SetParent(weaponRoot.transform);
            itemPrefab.name = data.nameKey;
            weaponObjectDict[(int)data.itemType, (int)data.itemGrade] = itemPrefab;

            // 미리 풀링
            Managers.Resource.PrePooling(weaponObjectDict[(int)data.itemType, (int)data.itemGrade],3);
        }
    }

    private void MonsterCaching(List<CSVMonsterData> monsterData, bool isBoss)
    {
        if(monsterData.Count <= 0) return;

        foreach(var data in monsterData)
        {
            if(data.DataOk() == false) continue;

            var monsterPrefab = Managers.Resource.Instantiate(data.key);
            var modelPrefab = Managers.Resource.Instantiate(data.modelKey);

            if(modelPrefab == null || modelPrefab == null)
            {
                Debug.LogError("Prefab is Null");
                continue;
            }

            modelPrefab.transform.SetParent(monsterPrefab.transform);
            monsterPrefab.name = data.nameKey;
            monsterPrefab.SetActive(false);

            if(isBoss)
            {
                bossMonsterPrefabs[data.spawnStage / 10] = monsterPrefab;
                data.PrintData();
            }
            else
            {
                for (int i = data.spawnStage; i <= data.endStage; i++)
                {
                    var renderer = monsterPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
                    if(renderer)
                    {
                        renderer.material.color = GetStageColor(i);
                    }
                    monsterPrefabs[i] = monsterPrefab;
                }
            }

            if(isBoss == false)
                Managers.Resource.PrePooling(monsterPrefab, 1);

            Debug.Log($"{data.nameKey} Created");
        }
    }


    /*------------
        스폰 관련
    --------------*/
    public void SpawnMosnter()
    {
        if (monsters.Count >= 100) return;

        var prefab = monsterPrefabs[Managers.Stage.GetStageCount()];
        var go = Managers.Resource.Instantiate(prefab, pooling: true);
        var monster = go.GetComponent<Monster>();
        monster.transform.localScale = Vector3.one;
        monsters.Add(monster);
        monster.Spawn(new MonsterData(prefab.name, Managers.Data.CalculateHP(Managers.Stage.GetStageCount()), 2));
    }

    public void SpawnBoss()
    {
        var prefab = bossMonsterPrefabs[Managers.Stage.GetStageCount()/10];
        var go = Managers.Resource.Instantiate(prefab);
        var bossMonster = go.GetComponent<BossMonster>();
        monsters.Add(bossMonster);
        bossMonster.Spawn(new MonsterData(prefab.name, Managers.Data.CalculateHP(Managers.Stage.GetStageCount() + 5), 2));
    }

    public GridObject SpawnWeapon(bool isGamble)
    {
        int rank = -1;

        if(isGamble)
            rank = Managers.Random.GetGambleRank();
        else
            rank = Managers.Random.GetRank();

        int type = UnityEngine.Random.Range((int)WeaponType.Sword,(int)WeaponType.MAX_COUNT);

        WeaponType weaponType = WeaponType.None;
        GameObject prefab = null;

        // 랜덤 무기 타입
        if (type == (int)WeaponType.Sword)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, rank], pooling : true);
            weaponType = WeaponType.Sword;
        }
        else if (type == (int)WeaponType.Bow)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, rank], pooling: true);
            weaponType = WeaponType.Bow;
        }
        else if (type == (int)WeaponType.Axe)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, rank], pooling: true);
            weaponType = WeaponType.Axe;
        }

        Debug.Assert(weaponType != WeaponType.None);

        var go = prefab;
        go.SetActive(true);
        var weapon = go.GetComponent<Weapon>();

        WeaponData weaponData = null;

        // 등급에 맞는 데이터 생성
        switch (weaponType)
        {
            case WeaponType.Sword:
                weaponData = new WeaponData("Sword", GetPowF(rank, 1.2f, 2.5f) * 1.5f, Managers.Data.GetSwordDamage(rank), rank, (UnitGrade)rank, objId++);
                break;
            case WeaponType.Bow:
                weaponData = new WeaponData("Bow", GetPowF(rank, 1.2f, 2.5f) * 2f, Managers.Data.GetSwordDamage(rank) / 2, rank * 5f, (UnitGrade)rank, objId++);
                break;
            case WeaponType.Axe:
                weaponData = new WeaponData("Axe", GetPowF(rank, 1.2f, 2.5f), Managers.Data.GetSwordDamage(rank) * 2, rank * 0.5f, (UnitGrade)rank, objId++);
                break;
        }

        Debug.Assert(weaponData != null);


        weapon.Spawn(weaponData);
        WeaponDict[weaponType].Add(weapon);
        weaponCounter[(int)rank]++;
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
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, (int)grade], pooling: true);
            weaponType = WeaponType.Sword;
        }
        else if (type == (int)WeaponType.Bow)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, (int)grade], pooling: true);
            weaponType = WeaponType.Bow;
        }
        else if (type == (int)WeaponType.Axe)
        {
            prefab = Managers.Resource.Instantiate(weaponObjectDict[type, (int)grade], pooling: true);
            weaponType = WeaponType.Axe;
        }

        var go = prefab;
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
        /*
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
        */


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

    private Color GetStageColor(int stage)
    {
        // 스테이지에 따라 색상 설정 (예시)
        // 스테이지를 10으로 나눈 나머지에 따라 색상 선택
        switch (stage % 10)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            case 4: return Color.magenta;
            case 5: return Color.cyan;
            case 6: return Color.white;
            case 7: return Color.gray;
            case 8: return Color.black;
            case 9: return Color.Lerp(Color.red, Color.blue, 0.5f); // 예시로 블렌딩 색상 사용
            default: return Color.white;
        }
    }

    public void Clear()
    {
        var monsterList = monsters.ToList();
        for (int i = 0; i < monsterList.Count; i++)
        {
            Managers.Resource.Destroy(monsterList[i].gameObject);
        }
        monsters.Clear();

        weaponCounter = new int[(int)UnitGrade.MAX_COUNT];

        var swordList = WeaponDict[WeaponType.Sword].ToList();
        for(int i = 0; i < swordList.Count; i++)
        {
            Managers.Resource.Destroy(swordList[i].gameObject);
        }
        WeaponDict[WeaponType.Sword].Clear();

        var bowList = WeaponDict[WeaponType.Bow].ToList();
        for (int i = 0; i < bowList.Count; i++)
        {
            Managers.Resource.Destroy(bowList[i].gameObject);
        }
        WeaponDict[WeaponType.Bow].Clear();


        var AxeList = WeaponDict[WeaponType.Axe].ToList();
        for (int i = 0; i < AxeList.Count; i++)
        {
            Managers.Resource.Destroy(AxeList[i].gameObject);
        }
        WeaponDict[WeaponType.Axe].Clear();


        UpdateUInvoke();

    }

    public void CreatePlayer()
    {
        Player newPlayer = new Player();
        Player = new Player();
    }
}
