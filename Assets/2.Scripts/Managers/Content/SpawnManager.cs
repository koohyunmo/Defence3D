using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private int spawnPrice = 20;
    private int gambleSpawnPrice = 50;

    List<Action> updateUI = new List<Action>();

    public bool Spawn(Player player)
    {
        if(player.gold < spawnPrice) return false;

        if(Managers.Grid.AddGridObject())
        {
            player.UseGold(spawnPrice);
            spawnPrice+=2;
            InvokeUI();
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
        foreach(Action action in updateUI)
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
}
