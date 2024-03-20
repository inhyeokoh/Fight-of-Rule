using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public enum Enum_Class
{
    Default,
    Warrior,
    Archer,
    Wizard
}

//���� �÷��̾��� ���κ��� ������ �ϴ� Ŭ����
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    public List<SubMono<PlayerController>> _controller;

    public CharacterStatus _playerStat;
    public CharacterState _playerState;    
    public CharacterMovement _playerMovement;
    public CharacterEquipment _playerEquipment;
    public CharacterPotion _playerpotion;
    public CharacterEventHandler _eventHandler;
    public CharacterAnimationController _animationController;
    public CharacterEffector _effector;
    public LevelSystem _levelSystem;

    //���� �Ʒ� ���� �׶������� �ƴ��� üũ
    public RaycastHit[] ground;

    // Ű���Կ� ��ų�� �ִ��� üũ
    [SerializeField]
    private UI_SkillKeySlot[] ketSlots;

    // ������ �°� ĳ���͸� �������� ������
    [SerializeField]
    private GameObject[] ClassPrefabs;

    [SerializeField]
    Transform test;

    // public List<BaseGameEntity> entitys;
    private float avoid = 0;

    public Camera camera;

    Ray ray;

    public Enum_Class _class;


    private void Awake()
    {
        _class = Enum_Class.Warrior;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
        switch (_class)
        {
            case Enum_Class.Warrior:
                {
                    GameObject clone = Instantiate(ClassPrefabs[0]);
                    _playerStat = clone.GetComponent<CharacterStatus>();
                    _playerState = clone.GetComponent<CharacterState>();
                    _playerEquipment = clone.GetComponent<CharacterEquipment>();
                    _playerMovement = clone.GetComponent<CharacterMovement>();
                    _eventHandler = clone.GetComponent<CharacterEventHandler>();
                    _animationController = clone.GetComponent<CharacterAnimationController>();
                    _effector = clone.GetComponent<CharacterEffector>();
                    _levelSystem = clone.GetComponent<LevelSystem>();
                    break;
                }
            case Enum_Class.Archer:
                {
                    GameObject clone = Instantiate(ClassPrefabs[1]);
                    _playerStat = clone.GetComponent<CharacterStatus>();
                    _playerState = clone.GetComponent<CharacterState>();
                    _playerEquipment = clone.GetComponent<CharacterEquipment>();
                    _playerMovement = clone.GetComponent<CharacterMovement>();
                    _eventHandler = clone.GetComponent<CharacterEventHandler>();
                    _animationController = clone.GetComponent<CharacterAnimationController>();
                    _effector = clone.GetComponent<CharacterEffector>();
                    _levelSystem = clone.GetComponent<LevelSystem>();
                    break;
                }
            case Enum_Class.Wizard:
                {
                    GameObject clone = Instantiate(ClassPrefabs[2]);
                    _playerStat = clone.GetComponent<CharacterStatus>();
                    _playerState = clone.GetComponent<CharacterState>();
                    _playerEquipment = clone.GetComponent<CharacterEquipment>();
                    _playerMovement = clone.GetComponent<CharacterMovement>();
                    _eventHandler = clone.GetComponent<CharacterEventHandler>();
                    _animationController = clone.GetComponent<CharacterAnimationController>();
                    _effector = clone.GetComponent<CharacterEffector>();
                    _levelSystem = clone.GetComponent<LevelSystem>();
                    break;
                }
        }

        camera = Camera.main;

        _controller = new List<SubMono<PlayerController>>
        {
            _playerStat,
            _playerState,
            _playerEquipment,
            _playerMovement,
            _eventHandler,
            _animationController,
            _effector,
            _levelSystem
        };

        for (int i = 0; i < _controller.Count; i++)
        {
            _controller[i].Mount(this);
            _controller[i].Init();
        }

        _playerState.StateAdd();
        SkillManager.Skill.PlayerData();
    }

    // ���� �������� �׸��� ��� �������� ȣ��
    private void FixedUpdate()
    {      
        _playerState.FixedUpdated();
        _playerEquipment.EquipmentFixedStay();
    }

    //���� �������� �׸��� ��� �������� �׸��� ������ ��Ÿ��(�̰� �ٸ����� �Űܾ���) ���콺 ��ġ üũ
    private void Update()
    {
        ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (avoid >= 0)
        {
            avoid -= Time.deltaTime;
        }

        _playerState.Updated();
        _playerEquipment.EquipmentStay();
    }

    // �������� ������ (�̰͵� �ٸ����� �Űܾ���)
    public void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defenese, bool firstLevel)
    {
        _playerStat.LevelStatUP(maxEXP, maxHP, maxMP, attack, defenese, firstLevel);
    }

    // ������ üũ�ߴ��� Ȯ�� (�̰͵� �ٸ����� �Űܾ���)
    public void LevelUpCheck(int level)
    {
        _levelSystem.LevelUpCheck(level);
    }


    //�÷��̾��� ����ºκ��� ����ϴ� �޼�����ε� �̰� ����� Ŭ������ �ϳ� ���� �Űܾ� �ɰŰ���

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {

            if (_playerState.CharacterStates == Enum_CharacterState.Avoid ||
                _playerState.CharacterStates == Enum_CharacterState.Hit ||
                _playerState.CharacterStates == Enum_CharacterState.Fall ||
                _playerState.CharacterStates == Enum_CharacterState.Dead ||
                _playerState.CharacterStates == Enum_CharacterState.Attack ||
                _playerState.SkillUseCheck)
            {
                if (Physics.Raycast(ray, out RaycastHit hi, 100, 1 << 6))
                {                  
                    test.position = hi.point;
                    _playerMovement.TargetPosition = new Vector3(hi.point.x, _playerMovement.playerTransform.position.y,
                    hi.point.z);
                    return;
                }
            }
            if (Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 6))
            {
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
                hit.point.z);

                _playerState.ChangeState((int)Enum_CharacterState.Move);

            }
        }     
    } 

    public void OnAvoid(InputAction.CallbackContext context)
    {

        if (context.action.phase == InputActionPhase.Started)
        {
            if (_playerState.CharacterStates == Enum_CharacterState.Fall ||
          _playerState.CharacterStates == Enum_CharacterState.Dead)
            {
                return;
            }
            
            
            if (avoid > 0)
            {
                return;
            }
            else
            {
                avoid = 3f;
            }
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << 6))
            {
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
               hit.point.z);
                _playerState.ChangeState((int)Enum_CharacterState.Avoid);
            }
        }         
    }

    public void OnTextExp(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {
            _playerStat.EXP += 2000000;      
        }


    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {
            if (_playerState.CharacterStates == Enum_CharacterState.Attack ||
                _playerState.CharacterStates == Enum_CharacterState.Hit ||
                _playerState.CharacterStates == Enum_CharacterState.Avoid ||
          _playerState.CharacterStates == Enum_CharacterState.Fall ||
          _playerState.CharacterStates == Enum_CharacterState.Dead ||
           _playerState.SkillUseCheck)
            {
                return;
            }

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << 6))
            {               
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
               hit.point.z);
                _playerState.ChangeState((int)Enum_CharacterState.Attack);

            }           
        }
    }

    public void OnSkillQ(InputAction.CallbackContext context)
    {
        if (_playerState.CharacterStates == Enum_CharacterState.Avoid ||
            _playerState.CharacterStates == Enum_CharacterState.Hit ||
          _playerState.CharacterStates == Enum_CharacterState.Fall ||
          _playerState.CharacterStates == Enum_CharacterState.Dead ||
          _playerState.CharacterStates == Enum_CharacterState.Delay)
        {
            _playerMovement.Stop();
            _animationController.ChangeMoveAnimation(0);
            return;
        }

        if (context.action.phase == InputActionPhase.Started)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << 6))
            {              
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
               hit.point.z);                              
                ketSlots[0].Use(_playerState, _playerStat);
            }
        }           
    }

    public void OnSkillW(InputAction.CallbackContext context)
    {
        if (_playerState.CharacterStates == Enum_CharacterState.Avoid ||
            _playerState.CharacterStates == Enum_CharacterState.Hit ||
          _playerState.CharacterStates == Enum_CharacterState.Fall ||
          _playerState.CharacterStates == Enum_CharacterState.Dead ||
          _playerState.CharacterStates == Enum_CharacterState.Delay)
        {
            _playerMovement.Stop();
            _animationController.ChangeMoveAnimation(0);
            return;
        }

        if (context.action.phase == InputActionPhase.Started)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << 6))
            {
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
               hit.point.z);              
                ketSlots[1].Use(_playerState, _playerStat);
            }          
        }
    }
    public void OnSkillE(InputAction.CallbackContext context)
    {
        if (_playerState.CharacterStates == Enum_CharacterState.Avoid ||
            _playerState.CharacterStates == Enum_CharacterState.Hit ||
          _playerState.CharacterStates == Enum_CharacterState.Fall ||
          _playerState.CharacterStates == Enum_CharacterState.Dead ||
          _playerState.CharacterStates == Enum_CharacterState.Delay)
        {
            _playerMovement.Stop();
            _animationController.ChangeMoveAnimation(0);
            return;
        }

        if (context.action.phase == InputActionPhase.Started)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << 6))
            {              
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
               hit.point.z);
                ketSlots[2].Use(_playerState, _playerStat);
            }    
        }
    }
    public void OnSkillR(InputAction.CallbackContext context)
    {
        if (_playerState.CharacterStates == Enum_CharacterState.Avoid ||
            _playerState.CharacterStates == Enum_CharacterState.Hit ||
          _playerState.CharacterStates == Enum_CharacterState.Fall ||
          _playerState.CharacterStates == Enum_CharacterState.Dead ||
          _playerState.CharacterStates == Enum_CharacterState.Delay)
        {
            _playerMovement.Stop();
            _animationController.ChangeMoveAnimation(0);
            return;
        }

        if (context.action.phase == InputActionPhase.Started)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << 6))
            {               
                test.position = hit.point;
                _playerMovement.TargetPosition = new Vector3(hit.point.x, _playerMovement.playerTransform.position.y,
               hit.point.z);
                ketSlots[3].Use(_playerState, _playerStat);
            }  
        }
    }

    public void OnDeadCheck(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {
            _playerState.ChangeState((int)Enum_CharacterState.Dead);
        }       
    }

    public void OnAliveCheck(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Started)
        {
            print("������");
            _playerState.ChangeState((int)Enum_CharacterState.Idle);
        }
    }



    // �̺�Ʈ�鿡 �������� �ޱ����� �޼����
    public void DistributeState(int Event)
    {
        print("�۵���");
        _playerState.ChangeState(Event);
    }

    public void DistributeRootMotion()
    {
        _animationController.RootMotion(true);
    }
    public void DistributeEffectDurationOn(int Event)
    {
        _effector.EffectDurationOn(Event);
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
