using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUpgrade : MonoBehaviour
{
    Text upgradeText;

    private void Start() 
    {
        upgradeText = gameObject.GetComponentInChildren<Text>();
        UpdateUI();
    }
    public void Upgrade()
    {
        Managers.Upgrade.SwordUpgrade();
        UpdateUI();
    }

    private void UpdateUI()
    {
        upgradeText.text = "+" + Managers.Upgrade.GetSwordUpgradeLevel().ToString();
    }
}
