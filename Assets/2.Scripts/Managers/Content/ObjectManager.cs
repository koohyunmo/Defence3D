using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public List<Enemy> monsters = new();
    public void SpawnMosnter()
    {
        var go = Managers.Resource.Instantiate("Monster",pooling:true);
        var monster = go.GetComponent<Enemy>();
        monsters.Add(monster);
        monster.Spawn();
    }
}
