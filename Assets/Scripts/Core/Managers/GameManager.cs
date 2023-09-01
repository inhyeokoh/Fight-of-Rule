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
            //�ʿ�� ��Ȱ���Ѽ� �ٽ� ���.
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
    NetworkManager _networkManager = new NetworkManager();
    /// <summary>
    /// network
    /// </summary>
    public static NetworkManager Network { get { return Instance._networkManager; } }


    /// <summary>
    /// invoke in GameManager.Update();
    /// </summary>
    Action _onUpdate;

    private void Awake()
    {
        _Instance = GetComponent<GameManager>();
        DontDestroyOnLoad(gameObject);

        //����ϴ� ������ �߿��� �� ����
        _managers = new List<SubClass<GameManager>>()
        {
            _resources,
            _pool,
            _networkManager,
        };

        for(int i = 0; i < _managers.Count; i++)
        {
            _managers[i].Mount(this);
            _managers[i].Init();
        }

        for(int i = 0; i < _managers.Count; i++)
            _onUpdate += _managers[i].GetAction();
    }

    private void Update()
    {
        _onUpdate?.Invoke();
    }
}
