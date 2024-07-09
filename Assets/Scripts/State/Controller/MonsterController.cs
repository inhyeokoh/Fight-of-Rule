using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Enum_MonsterType
{
    Slime,
    Goblin,
    Orc,
    Undead,
}

public class MonsterController : MonoBehaviour
{
    //몬스터 컨트롤러는 몬스터에 연결되있는 기능들을 연결하고 입출력을 담당하는
    //메인보드 기능
    

    List<SubMono<MonsterController>> _monsterController;

    public MonsterData monsterDB;
    public MonsterItemDropData monsterItemDropDB;

    public MonsterState _monsterState;
    public MonsterStatus _monsterStatus;
    public MonsterAnimationController _animationController;
    public MonsterEventHandler _eventHandler;
    public MonsterEffector _effector;
    public MonsterMovement _monsterMovement;
    public MonsterItemDrop _monsterItemDrop;

    public bool DBRederOn;
   
    // 현재 범위안에 있는 플레이어들 확인
    public Collider[] players;


    private void Awake()
    {
        _monsterState = gameObject.GetComponent<MonsterState>();
        _monsterStatus = gameObject.GetComponent<MonsterStatus>();
        _animationController = gameObject.GetComponent<MonsterAnimationController>();
        _eventHandler = gameObject.GetComponent<MonsterEventHandler>();
        _effector = gameObject.GetComponent<MonsterEffector>();
        _monsterMovement = gameObject.GetComponent<MonsterMovement>();
        _monsterItemDrop = gameObject.GetComponent<MonsterItemDrop>();


        _monsterController = new List<SubMono<MonsterController>>
        {
            _monsterStatus,
            _monsterState,
            _animationController,
            _eventHandler,
            _effector,
            _monsterMovement,
            _monsterItemDrop,
        };       
    }

    private void OnEnable()
    {
        for (int i = 0; i < _monsterController.Count; i++)
        {
            _monsterController[i].Mount(this);
            _monsterController[i].Init();
        }

        DBRederOn = true;

        _monsterState.StateAdd();
        _monsterState.ChangeState((int)Enum_MonsterState.Idle);
    }
    private void FixedUpdate()
    {     
        _monsterState.FixedUpdated();
    }
    // Update is called once per frame
    void Update()
    {
        players = Physics.OverlapSphere(gameObject.transform.position, 40f, 1 << 7);
        _monsterState.Updated();
    }

    public void MonsterDBReader(MonsterData data, MonsterItemDropData itemDropData)
    {
        monsterDB = data;
        monsterItemDropDB = itemDropData;
    }

    public void DistributeState(int Event)
    {
        _monsterState.ChangeState(Event);
    }

    public void DistributeRootMotion()
    {
        _animationController.RootMotion(true);
    }

    public void DistributeEffectBurstOn(int Event)
    {
        _effector.EffectBurstOn(Event);
    }
    public void DistributeEffectBurstOff(int Event)
    {
        _effector.EffectBurstOff(Event);
    }
    public void DistributeEffectInstace(int Event)
    {
        _effector.EffectDurationInstance(Event);
    }
    public void DistributeEffectBurstStop()
    {
        _effector.EffectBurstStop();
    }
}
