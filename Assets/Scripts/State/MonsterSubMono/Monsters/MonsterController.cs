using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //���� ��Ʈ�ѷ��� ���Ϳ� ������ִ� ��ɵ��� �����ϰ� ������� ����ϴ�
    //����Ʈ���� ���
    //������ ���ʹ� ĳ���Ϳ� �޸� �Ѱ��� �����ϴ°� �ƴ϶� �����Ը� �����ϴ°Ŷ�
    //�ϳ��ϳ��� �޾Ƴ����� ���ҰŰ���.
    //�׷��� ĳ���� ��Ʈ�ѷ��� ����ѰŰ����� �̰� �׳� �� ��� ������ �������� ����.

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
