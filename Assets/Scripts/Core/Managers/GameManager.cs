using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            //필요시 부활시켜서 다시 사용.
            /*if(_Intance == null)
            {
                GameObject go = new GameObject("@@Manager");
                go.AddComponent<GameManager>();
            }*/

            return _Instance;
        }
    }

    List<SubClass<GameManager>> _managers;


    /*==================
     *     Pool
     =================*/
    PoolManager _pool = new PoolManager();
    /// <summary>
    /// object pool
    /// </summary>
    public static PoolManager Pool { get { return Instance._pool; } }


    /*==================
     *    Resource
     =================*/
    ResourceManager _resources = new ResourceManager();
    /// <summary>
    /// ResourceControler
    /// </summary>
    public static ResourceManager Resources { get { return Instance._resources; } }


    /*==================
     *  ThreadManager
     =================*/
    /// <summary>
    /// thread pool manager
    /// </summary>
    ThreadManager _threadPool = new ThreadManager();
    public static ThreadManager ThreadPool { get {  return Instance._threadPool; } }


    /*==================
     *     Network
     =================*/
    NetworkManager _networkManager = new NetworkManager();
    /// <summary>
    /// network
    /// </summary>
    public static NetworkManager Network { get { return Instance._networkManager; } }

    /*==================
     *  UIManager
     =================*/
    UIManager _uiManager = new UIManager();
    /// <summary>
    /// Control popup UI
    /// </summary>
    public static UIManager UI { get { return Instance._uiManager; } }

    /*==================
    *    DataManager
    =================*/
    DataManager _dataManager = new DataManager();
    /// <summary>
    /// Load and Save Data
    /// </summary>
    public static DataManager Data { get { return Instance._dataManager; } }

    /*==================
    *    SceneManager
    =================*/
    SceneManager2 _sceneManager = new SceneManager2();
    /// <summary>
    /// Change Scene
    /// </summary>
    public static SceneManager2 Scene { get { return Instance._sceneManager; } }

    /*==================
    *    DataManager
    =================*/
    InventoryManager _invenManager = new InventoryManager();
    /// <summary>
    /// Load and Save Data
    /// </summary>
    public static InventoryManager Inven { get { return Instance._invenManager; } }

    LockQueue<Action> _tasks = new LockQueue<Action>();
    Action _onUpdate;

    private void Awake()
    {
        _Instance = GetComponent<GameManager>();
        DontDestroyOnLoad(gameObject);

        //등록하는 순서가 중요할 수 있음
        _managers = new List<SubClass<GameManager>>()
        {
            _pool,
            _resources,
            _threadPool,
            _networkManager,
            _uiManager,
            _dataManager,
            _sceneManager,
            _invenManager
        };

        for(int i = 0; i < _managers.Count; i++)
        {
            _managers[i].Mount(this);
            _managers[i].Init();
        }

        // Action 대리자에 대해 공부하면 쉽게 이해할 수 있음
        // 추가적으로 필요하면 Func도 같이 공부하면 같이 이해하기 쉬움
        for(int i = 0; i < _managers.Count; i++)
            _onUpdate += _managers[i].GetAction();
    }

    private void Update()
    {
        _onUpdate?.Invoke();
        var tasks = _tasks.DequeueAll();

        for (int i = 0; i < tasks.Count; i++)
            tasks[i]?.Invoke();
    }

    /// <summary>
    /// unity main thread
    /// </summary>
    public void EnqueueAsync(Action lambda)
    {
        _tasks.Enqueue(lambda);
    }
}
