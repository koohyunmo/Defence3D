using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int Gold {get; private set;}= 100;
    public int Gem { get; private set; } = 1;
    public int SpawnLevel { get; private set; } = 1;
    public int Upgrade1Level { get; private set; } = 1;
    public int Upgrade2Level { get; private set; } = 1;
    public int Upgrade3Level { get; private set; } = 1;

    public Player()
    {
        Gold = 100;
        Gem = 1;
        SpawnLevel = 1;
        Upgrade1Level = 1;
        Upgrade2Level = 1;
        Upgrade3Level = 1;
    }

    public void UpgradeSpawn()
    {
        SpawnLevel++;
    }
    public void Upgrade1Weapon()
    {
        Upgrade1Level++;
    }
    public void Upgrade2Weapon()
    {
        Upgrade2Level++;
    }
    public void Upgrade3Weapon()
    {
        Upgrade3Level++;
    }
    public void UseGold(int price)
    {
        Gold-=price;
        Debug.Assert(Gold >= 0);
        Managers.Notify.NotifyChangedGold();
    }
    public void GoldReward()
    {
        Gold++;
        Managers.Notify.NotifyChangedGold();
    }
    public void GemReward(int v = 1)
    {
        Gem += v;
        Managers.Notify.NotifyChangedGold();
    }

    public void SetGold(int v)
    {
        Gold += v;
        Managers.Notify.NotifyChangedGold();
    }
    public void SetGem(int v)
    {
        Gem += v;
        Managers.Notify.NotifyChangedGold();
    }

    public void UseGem(int gambleSpawnPrice)
    {
        Gem -= gambleSpawnPrice;
        Debug.Assert(Gem >= 0);
        Managers.Notify.NotifyChangedGold();
    }
}
