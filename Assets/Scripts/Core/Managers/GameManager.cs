using System;
using System.Collections.Generic;
using UnityEngine;

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
     *     Network
     *     inhyeok
     =================*/
/*    NetworkManager _networkManager = new NetworkManager();
    /// <summary>
    /// network
    /// </summary>
    public static NetworkManager Network { get { return Instance._networkManager; } }*/

    /// <summary>
    /// ui
    /// </summary>
    UIManager _uiManager = new UIManager();
    public static UIManager UI { get { return Instance._uiManager; } }

    /// <summary>
    /// invoke in GameManager.Update();
    /// </summary>
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
/*            _networkManager,*/
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
    }
}
