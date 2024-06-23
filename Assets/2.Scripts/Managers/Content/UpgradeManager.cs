using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyEnums;

public class UpgradeManager
{
    private int _swordUpgradeLevel = 1;

    public void SwordUpgrade()
    {
        _swordUpgradeLevel++;
    }
    public int GetSwordUpgradeLevel()
    {
        return _swordUpgradeLevel;
    }

    public int GetLevel(WeaponType weaponType)
    {
        Debug.Assert((int)weaponType >= 1);

        switch (weaponType)
        {
            case WeaponType.Sword : return _swordUpgradeLevel;
        }

        return 1;
        
    }


}
