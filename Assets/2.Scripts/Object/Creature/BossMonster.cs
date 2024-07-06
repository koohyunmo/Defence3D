using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{

    public override void Spawn(MonsterData data)
    {
        base.Spawn(data);
        // 보스 등장
    }

    protected override void Reward()
    {
        // TODO Reward
    }
}
