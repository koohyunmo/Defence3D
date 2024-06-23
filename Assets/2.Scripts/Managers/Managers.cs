using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static bool s_init = false;

    //------------------------//
    //          Content       //
    //------------------------//
    private GridManager _grid = new GridManager();
    private ObjectManager _obj = new ObjectManager();
    private RandomManager _rnd = new RandomManager();
    private StageManager _stage = new StageManager();
    private DataManager _data = new DataManager();
    private UpgradeManager _upgrade = new UpgradeManager();

    //------------------------//
    //          Core          //
    //------------------------//
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();

    public static GridManager Grid { get { return Instance?._grid; } }
    public static ObjectManager Object { get { return Instance?._obj; } }
    public static RandomManager Random { get { return Instance?._rnd; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static StageManager Stage { get { return Instance?._stage; } }
    public static DataManager Data { get { return Instance?._data; } }

    public static UpgradeManager Upgrade { get { return Instance?._upgrade; } }

    public static Managers Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject("@Managers");
                    DontDestroyOnLoad(go);
                    s_instance = go.AddComponent<Managers>();
                    s_instance.Initialize();
                }
                else
                {
                    s_instance = go.GetComponent<Managers>();
                }
            }
            return s_instance;
        }
    }

    private void Initialize()
    {
        // 초기화 메서드 호출
        _rnd.Init();
        _resource.Init();
        Application.targetFrameRate = 60;
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
