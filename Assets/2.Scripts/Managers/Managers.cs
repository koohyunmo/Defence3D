using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static bool s_init = false;
    private static GridManager _grid = new GridManager();
    private static ObjectManager _obj = new ObjectManager();
    private static PoolManager _pool = new PoolManager();
    private static ResourceManager _resource = new ResourceManager();


    public static GridManager Grid {get {return _grid; }}
    public static ObjectManager Object { get { return _obj; } }
    public static PoolManager Pool { get { return _pool; } }
    public static ResourceManager Resource { get { return _resource; } }

    public static Managers Instance
    {
        get
        {
            if (s_init == false)
            {
                s_init = true;

                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject() { name = "@Managers" };

                }

                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();
            }

            return s_instance;
        }
    }
}
