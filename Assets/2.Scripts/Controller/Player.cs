using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int gold = 100;
    public int spawnLevel = 1;
    public int upgrade1Level = 1;
    public int upgrade2Level = 1;
    public int upgrade3Level = 1;

    public void UpgradeSpawn()
    {
        spawnLevel++;
    }
    public void Upgrade1Weapon()
    {
        upgrade1Level++;
    }
    public void Upgrade2Weapon()
    {
        upgrade2Level++;
    }
    public void Upgrade3Weapon()
    {
        upgrade3Level++;
    }
    public void UseGold(int price)
    {
        gold-=price;
        Debug.Assert(gold >= 0);
        Managers.Notify.NotifyChangedGold();
    }
    public void GoldReward()
    {
        gold++;
        Managers.Notify.NotifyChangedGold();
    }
}
