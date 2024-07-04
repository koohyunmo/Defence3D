using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class UpgradeManager
{
    private int _upgrade1Price = 20;
    private int _upgrade2Price = 40;
    private int _upgrade3Price = 80;
    private int _spawnUpgradePrice = 20;

    public bool Upgrade1(Player player)
    {
        if(player.gold < _upgrade1Price) return false;
        
        player.Upgrade1Weapon();
        player.UseGold(_upgrade1Price);
        return true;
    }
    public bool Upgrade2(Player player)
    {
        if (player.gold < _upgrade2Price) return false;

        player.Upgrade1Weapon();
        player.UseGold(_upgrade2Price);
        return true;
    }
    public bool Upgrade3(Player player)
    {
        if (player.gold < _upgrade3Price) return false;

        player.Upgrade2Weapon();
        player.UseGold(_upgrade2Price);
        return true;
    }
    public bool UpgradeSpawn(Player player)
    {
        if (player.gold < _spawnUpgradePrice) return false;

        player.UpgradeSpawn();
        player.UseGold(_spawnUpgradePrice);
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
                return player.upgrade1Level;
            case UnitGrade.Relic:
            case UnitGrade.Epic:
            case UnitGrade.Legendary:
                return player.upgrade2Level;
            case UnitGrade.Mythic:
            case UnitGrade.Mythical:
            case UnitGrade.Primal:
                return player.upgrade3Level;

        }
        return -1;
        
    }

}
