using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static MyEnums;

public class ObjectManager
{
    private HashSet<Monster> monsters = new HashSet<Monster>();

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
        monsters.Add(monster);
        monster.Spawn(new MonsterData("Slime", Managers.Data.CalculateHP(Managers.Stage.GetStageCount()), 2));
    }

    public GridObject SpawnWeapon()
    {
        var rankAndColor = Managers.Random.GetTestColorAndRank();
        int type = UnityEngine.Random.Range(0,9) % 3;

        WeaponType weaponType = WeaponType.None;
        GameObject prefab = null;

        if (type == 0)
        {
            prefab = Managers.Resource.Load<GameObject>("Sword1");
            weaponType = WeaponType.Sword;
        }
        else if(type == 1)
        {
            prefab = Managers.Resource.Load<GameObject>("Bow1");
            weaponType = WeaponType.Bow;
        }
        else if(type == 2)
        {
            prefab = Managers.Resource.Load<GameObject>("Axe1");
            weaponType = WeaponType.Axe;
        }

        Debug.Assert(weaponType != WeaponType.None);

        var go = GameObject.Instantiate(prefab);
        var weapon = go.GetComponent<Weapon>();

        WeaponData weaponData = null;

        switch (weaponType)
        {
            case WeaponType.Sword:
                weaponData = new WeaponData("Sword", GetPowF(rankAndColor.Item1, 1.2f, 2.5f) * 1.5f, Managers.Data.GetSwordDamage(rankAndColor.Item1), rankAndColor.Item1, (UnitGrade)rankAndColor.Item1);
                break;
            case WeaponType.Bow:
                weaponData = new WeaponData("Bow", GetPowF(rankAndColor.Item1, 1.2f, 2.5f) * 2f, Managers.Data.GetSwordDamage(rankAndColor.Item1) / 2, rankAndColor.Item1, (UnitGrade)rankAndColor.Item1);
                break;
            case WeaponType.Axe:
                weaponData = new WeaponData("Axe", GetPowF(rankAndColor.Item1, 1.2f, 2.5f), Managers.Data.GetSwordDamage(rankAndColor.Item1) * 2, rankAndColor.Item1, (UnitGrade)rankAndColor.Item1);
                break;
        }

        Debug.Assert(weaponData != null);

        weapon.Spawn(weaponData);

        // 자식 객체에서 Renderer를 찾기
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
        WeaponData weaponData = new WeaponData("Sword", GetPowF((int)grade, 1.2f, 2.5f), Managers.Data.GetSwordDamage(grade), (int)grade, grade);
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
}
