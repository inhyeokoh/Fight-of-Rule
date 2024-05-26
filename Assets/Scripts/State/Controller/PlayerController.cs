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

//현재 플레이어의 메인보드 역할을 하는 클래스
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    public ItemData item;

    public List<SubMono<PlayerController>> _controller;

    public CharacterStatus _playerStat;
    public CharacterState _playerState;    
    public CharacterMovement _playerMovement;
    public CharacterEquipment _playerEquipment;
    public CharacterExternalStatus _playerExternalStat;
    public CharacterEventHandler _eventHandler;
    public CharacterAnimationController _animationController;
    public CharacterEffector _effector;
    public Interaction _interaction;
    public LevelSystem _levelSystem;

    //현재 아래 땅이 그라운드인지 아닌지 체크
    public RaycastHit[] ground;

    // 키슬롯에 스킬이 있는지 체크
    [SerializeField]
    private UI_SkillKeySlot[] ketSlots;

    // 직업에 맞게 캐릭터를 가져오는 프리팹
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
                    _playerExternalStat = clone.GetComponent<CharacterExternalStatus>();
                    _playerEquipment = clone.GetComponent<CharacterEquipment>();
                    _playerMovement = clone.GetComponent<CharacterMovement>();
                    _eventHandler = clone.GetComponent<CharacterEventHandler>();
                    _animationController = clone.GetComponent<CharacterAnimationController>();
                    _effector = clone.GetComponent<CharacterEffector>();
                    _interaction = clone.GetComponent<Interaction>();
                    _levelSystem = clone.GetComponent<LevelSystem>();
                    break;
                }
            case Enum_Class.Archer:
                {
                    GameObject clone = Instantiate(ClassPrefabs[1]);
                    _playerStat = clone.GetComponent<CharacterStatus>();
                    _playerState = clone.GetComponent<CharacterState>();
                    _playerExternalStat = clone.GetComponent<CharacterExternalStatus>();
                    _playerEquipment = clone.GetComponent<CharacterEquipment>();
                    _playerMovement = clone.GetComponent<CharacterMovement>();
                    _eventHandler = clone.GetComponent<CharacterEventHandler>();
                    _animationController = clone.GetComponent<CharacterAnimationController>();
                    _effector = clone.GetComponent<CharacterEffector>();
                    _interaction = clone.GetComponent<Interaction>();
                    _levelSystem = clone.GetComponent<LevelSystem>();
                    break;
                }
            case Enum_Class.Wizard:
                {
                    GameObject clone = Instantiate(ClassPrefabs[2]);
                    _playerStat = clone.GetComponent<CharacterStatus>();
                    _playerState = clone.GetComponent<CharacterState>();
                    _playerExternalStat = clone.GetComponent<CharacterExternalStatus>();
                    _playerEquipment = clone.GetComponent<CharacterEquipment>();
                    _playerMovement = clone.GetComponent<CharacterMovement>();
                    _eventHandler = clone.GetComponent<CharacterEventHandler>();
                    _animationController = clone.GetComponent<CharacterAnimationController>();
                    _effector = clone.GetComponent<CharacterEffector>();
                    _interaction = clone.GetComponent<Interaction>();
                    _levelSystem = clone.GetComponent<LevelSystem>();
                    break;
                }
        }

        camera = Camera.main;

        _controller = new List<SubMono<PlayerController>>
        {
            _playerStat,
            _playerState,
            _playerExternalStat,
            _playerEquipment,
            _playerMovement,
            _eventHandler,
            _animationController,
            _effector,
            _interaction,
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

    private void Start()
    {
        
    }

    // 현재 상태패턴 그리고 장비 상태패턴 호출
    private void FixedUpdate()
    {      
        _playerState.FixedUpdated();
        _playerEquipment.EquipmentFixedStay();
    }

    //현재 상태패턴 그리고 장비 상태패턴 그리고 구르기 쿨타임(이건 다른데로 옮겨야함) 마우스 위치 체크
    private void Update()
    {
        for(int i = 0; i < _controller.Count; i++)
        {
            _controller[i].Excute();
        }

        ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (avoid >= 0)
        {
            avoid -= Time.deltaTime;
        }

        _playerState.Updated();
        _playerEquipment.EquipmentStay();

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerStat.EXP += 40000;
        }
    }

    // 레벨업을 했을때 (이것도 다른데로 옮겨야함)
    public void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defenese)
    {
        _playerStat.LevelStatUP(maxEXP, maxHP, maxMP, attack, defenese);
    }

    // 레벨업 체크했는지 확인 (이것도 다른데로 옮겨야함)
    public void LevelUpCheck(int level)
    {
        _levelSystem.LevelUpCheck(level);
    }


    //플레이어의 입출력부분을 담당하는 메서드들인데 이걸 입출력 클래스를 하나 만들어서 옮겨야 될거같음

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
            print("눌렀음");
            _playerState.ChangeState((int)Enum_CharacterState.Idle);
        }
    }



    // 이벤트들에 정보들을 받기위한 메서드들
    public void DistributeState(int Event)
    {
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
