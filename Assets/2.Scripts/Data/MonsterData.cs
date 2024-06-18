using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public string monsterName;
    public float speed;
    public int maxHp;
    public int hp;

    public MonsterData(string name, int maxHp, int speed)
    {
        this.monsterName = name;
        this.speed = speed;
        this.maxHp = maxHp;
        hp = maxHp;
    }

    public MonsterData(MonsterData data)
    {
        this.monsterName = data.monsterName;
        this.speed = data.speed;
        this.maxHp = data.maxHp;
        hp = maxHp;
    }
}
