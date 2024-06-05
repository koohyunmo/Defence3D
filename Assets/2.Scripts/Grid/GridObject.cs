using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private int x,z;
    private Vector3 originalPos;
    public GridObject(int x, int z ,Vector3 originalPos)
    {
        this.x = x;
        this.z = z;
        this.originalPos = originalPos;
    }
}