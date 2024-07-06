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
    public void RegisterGoldUIEvent(Action evt)
    {
        useGoldUIUpdate.Add(evt);
    }
    public void RemoveGoldUIEvent(Action evt)
    {
        useGoldUIUpdate.Remove(evt);
    }
}
