using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager
{
    public void Reinforce(Weapon selectedWeapon, Weapon target)
    {
        target.GetWeaponData().Upgrade();
    }
}
