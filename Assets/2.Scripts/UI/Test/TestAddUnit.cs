using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestAddUnit : MonoBehaviour
{
    public GridSystem grid;// 배열 선언 및 초기화

    public void Add()
    {
        if (Managers.Grid.AddGridObject())
        {
            
        }
    }
}
