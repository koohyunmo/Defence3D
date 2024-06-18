using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager
{
    public HashSet<Monster> monsters = new HashSet<Monster>();
    public void SpawnMosnter()
    {
        if(monsters.Count >= 100) return;

        string prefabName = Managers.Stage.GetStageCount() % 2 == 0 ? "Monster_1" : "Monster_2";
        var go = Managers.Resource.Instantiate(prefabName, pooling:true);
        var monster = go.GetComponent<Monster>();
        monsters.Add(monster);
        monster.Spawn(new MonsterData("Slime", GetPow(Managers.Stage.GetStageCount(), 1.38f,20), 2));
    }

    public GridObject SpawnWeapon()
    {
        var rankAndColor = Managers.Random.GetTestColorAndRank();
        var prefab = Managers.Resource.Load<GameObject>("Sword1");
        var go = GameObject.Instantiate(prefab);
        var weapon = go.GetComponent<Weapon>();

        //ew WeaponData("Sword1",2.5f,50,0.5f);
        weapon.Spawn(new WeaponData("Sword", GetPowF(rankAndColor.Item1, 1.2f, 2.5f), GetPow(rankAndColor.Item1, 2f, 10), 0.25f));

        // 자식 객체에서 Renderer를 찾기
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = rankAndColor.Item2;
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
