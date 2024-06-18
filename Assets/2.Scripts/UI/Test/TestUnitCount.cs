using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUnitCount : MonoBehaviour
{
    private Text unitCountText;
    
    private void Start() {
        unitCountText = gameObject.GetComponent<Text>();
    }
    private void Update()
    {
        unitCountText.text = Managers.Object.monsters.Count.ToString();
    }
}
