using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //몬스터 컨트롤러는 몬스터에 연결되있는 기능들을 연결하고 입출력을 담당하는
    //소프트웨어 기능
    //하지만 몬스터는 캐릭터와 달리 한개를 조종하는게 아니라 여러게를 조종하는거라
    //하나하나씩 달아놓으면 편할거같다.
    //그래도 캐릭터 컨트롤러랑 비슷한거같으니 이걸 그냥 좀 묶어서 했으면 좋았을것 같다.

    List<SubMono<MonsterController>> _monsterController;

    public MonsterState _monsterState;
    public MonsterAnimationController _animationController;
    public MonsterEventHandler _eventHandler;
    public MonsterEffector _effector;
    public MonsterMovement _monsterMovement;
    public MonsterItemDrop _monsterItemDrop;
    public Collider[] players;


    private void Awake()
    {
        _monsterState = gameObject.GetComponent<MonsterState>();
        _animationController = gameObject.GetComponent<MonsterAnimationController>();
        _eventHandler = gameObject.GetComponent<MonsterEventHandler>();
        _effector = gameObject.GetComponent<MonsterEffector>();
        _monsterMovement = gameObject.GetComponent<MonsterMovement>();
        _monsterItemDrop = gameObject.GetComponent<MonsterItemDrop>();
       

        _monsterController = new List<SubMono<MonsterController>>
        {
            _monsterState,
            _animationController,
            _eventHandler,
            _effector,
            _monsterMovement,
            _monsterItemDrop,
        };

     
        for (int i = 0; i < _monsterController.Count; i++)
        {
            _monsterController[i].Mount(this);
            _monsterController[i].Init();
        }

        _monsterState.StateAdd();
    
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
