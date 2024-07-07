using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private int spawnPrice = 20;
    private int gambleSpawnPrice = 1;

    List<Action> updateUI = new List<Action>();

    public bool Spawn(Player player, out GameObject gridObj)
    {
        gridObj = null;
        if (player.Gold < spawnPrice) return false;

        var createdObj = Managers.Grid.AddGridObject();

        if (createdObj)
        {
            player.UseGold(spawnPrice);
            spawnPrice += 2;
            InvokeUI();
            gridObj = createdObj.gameObject;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GambleSpawn(Player player, out GameObject gridObj)
    {
        gridObj = null;
        if (player.Gem < gambleSpawnPrice) return false;

        var createdObj = Managers.Grid.AddGridObject(isGamble: true);

        if (createdObj)
        {
            player.UseGem(gambleSpawnPrice);
            InvokeUI();
            gridObj = createdObj.gameObject;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetSpawnPrice()
    {
        return spawnPrice;
    }
    public int GetGambleSpawnPrice()
    {
        return gambleSpawnPrice;
    }

    public void InvokeUI()
    {
        foreach (Action action in updateUI)
        {
            action?.Invoke();
        }
    }

    public void RegisterUI(Action evt)
    {
        updateUI.Add(evt);
    }
    public void RemoveUI(Action evt)
    {
        updateUI.Remove(evt);
    }

    public void Clear()
    {
        spawnPrice = 20;
        gambleSpawnPrice = 1;
        InvokeUI();
    }
}
