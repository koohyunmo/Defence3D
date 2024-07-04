using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotiManager 
{
    List<Action> useGoldUIUpdate = new();

    public void NotifyChangedGold()
    {
        foreach(var evt in useGoldUIUpdate)
        {
            evt?.Invoke();
        }
    }
    public void RegisterGoldEvent(Action evt)
    {
        useGoldUIUpdate.Add(evt);
    }
    public void RemoveGoldEvent(Action evt)
    {
        useGoldUIUpdate.Remove(evt);
    }
}
