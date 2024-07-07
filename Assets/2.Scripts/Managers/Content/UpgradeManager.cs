using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class UpgradeManager
{
    public int Upgrade1Price {get; private set;}= 20;
    public int Upgrade2Price { get; private set; }= 40;
    public int Upgrade3Price { get; private set; }= 80;
    public int SpawnUpgradePrice { get; private set; }= 20;

    List<Action> updateUI = new List<Action>();

    public bool Upgrade1(Player player)
    {
        if(player.Gold < Upgrade1Price) return false;
        
        player.Upgrade1Weapon();
        player.UseGold(Upgrade1Price);
        Upgrade1Price  += 2;
        InvokeUpdateUI();
        return true;
    }
    public bool Upgrade2(Player player)
    {
        if (player.Gold < Upgrade2Price) return false;

        player.Upgrade2Weapon();
        player.UseGold(Upgrade2Price);
        Upgrade2Price += 4;
        InvokeUpdateUI();
        return true;
    }
    public bool Upgrade3(Player player)
    {
        if (player.Gold < Upgrade3Price) return false;

        player.Upgrade3Weapon();
        player.UseGold(Upgrade3Price);
        Upgrade3Price += 8;
        InvokeUpdateUI();
        return true;
    }
    public bool UpgradeSpawn(Player player)
    {
        if (player.Gold < SpawnUpgradePrice) return false;
        if (Managers.Random.CanUpgrade() == false) return false;

        player.UpgradeSpawn();
        Managers.Random.Upgrade();
        player.UseGold(SpawnUpgradePrice);
        SpawnUpgradePrice += 4;
        InvokeUpdateUI();
        return true;
    }
    public int GetLevel(UnitGrade grade, Player player)
    {
        Debug.Assert(grade != UnitGrade.None && player != null);

        switch (grade)
        {
            case UnitGrade.Basic:
            case UnitGrade.Rare:
            case UnitGrade.Ancient:
                return player.Upgrade1Level;
            case UnitGrade.Relic:
            case UnitGrade.Epic:
            case UnitGrade.Legendary:
                return player.Upgrade2Level;
            case UnitGrade.Mythic:
            case UnitGrade.Mythical:
            case UnitGrade.Primal:
                return player.Upgrade3Level;

        }
        return -1;
    }

    public void RegisterUpdateUI(Action evt)
    {
        updateUI.Add(evt);
    }
    public void RemoveUpdateUI(Action evt)
    {
        updateUI.Remove(evt);
    }

    private void InvokeUpdateUI()
    {
        foreach(var evt in updateUI)
        {
            evt?.Invoke();
        }
    }

    public void Clear()
    {
        Upgrade1Price = 20;
        Upgrade2Price = 40;
        Upgrade3Price = 80;
        SpawnUpgradePrice = 20;
        InvokeUpdateUI();
    }

}
