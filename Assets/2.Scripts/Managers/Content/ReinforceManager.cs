using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager
{
    public bool Reinforce(Weapon selectedWeapon, Weapon target)
    {

        // 강화 여부 로직


        if(target.GetWeaponData().level >= selectedWeapon.GetWeaponData().level)
        {
            target.GetWeaponData().Upgrade();
        }
        else
        {
            selectedWeapon.GetWeaponData().Upgrade();
            target.GetWeaponData().SetWeaponData(selectedWeapon.GetWeaponData());
        }
        target.gameObject.transform.localScale *= 1.1f;
        Debug.Log("강화 성공 : " + target.GetWeaponData().level);
        return true;

    }
}
