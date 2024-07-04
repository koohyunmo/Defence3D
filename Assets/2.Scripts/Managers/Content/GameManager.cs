using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int gold = 100;

    public bool UseGold(int price)
    {
        if(gold >= price)
        {
            gold -= price;
            return true;
        }

        return false;
    }
    public void SetGold(int gold)
    {
        this.gold += gold;
    }
    public int GetGold()
    {
        return gold;
    }
}
