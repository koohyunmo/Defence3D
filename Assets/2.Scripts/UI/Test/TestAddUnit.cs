using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestAddUnit : MonoBehaviour
{
    public Grid grid;
    public void Add()
    {
        if(Managers.Grid.AddGridObject())
        {
            // 성공
        }
    }
}