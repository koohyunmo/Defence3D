using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MyEnums;

public class TestChoiceUnit : MonoBehaviour
{
    public UnitGrade grade;

    private void Start() 
    {
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Managers.Grid.AddGridObject(grade);
    }
}
