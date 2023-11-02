using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    public List<SubMono<PlayerController>> _controller;

    public Character _playerEntity;
    public EventHandler _eventHandler;
    public AnimationController _animationController;
    public Effector _effector;

    [SerializeField]
    private GameObject[] ClassPrefabs;

    [SerializeField]
    Transform test;

   // public List<BaseGameEntity> entitys;
    private float avoid = 0;

    public Camera camera;
  
    Ray ray;


    private void Awake()
    {
        string charcterClass = "Warrior";

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        _eventHandler = Utils.GetOrAddComponent<EventHandler>(gameObject);
        _animationController = Utils.GetOrAddComponent<AnimationController>(gameObject);
        _effector = Utils.GetOrAddComponent<Effector>(gameObject);
      
        switch (charcterClass)
        {
            case "Warrior":               
                { 
                    GameObject clone = Instantiate(ClassPrefabs[0]);
                    _playerEntity = clone.GetComponent<Warrior>();
                    break;
                }
            case "Archer":
                {
                    GameObject clone = Instantiate(ClassPrefabs[1]);
                    _playerEntity = clone.GetComponent<Warrior>();
                    break;
                }
            case "Wizard": 
                {
                    GameObject clone = Instantiate(ClassPrefabs[2]);
                    _playerEntity = clone.GetComponent<Warrior>();
                    break;
                }               
        }

        _controller = new List<SubMono<PlayerController>>
        {
            _playerEntity,
            _eventHandler,
            _animationController,
            _effector
        };

        for (int i = 0; i < _controller.Count; i++)
        {
            _controller[i].Mount(this);
            _controller[i].Init();
        }
        
        SkillManager.Skill.PlayerData();

        camera = Camera.main;

    }

    private void Start()
    {
      
    }

    private void FixedUpdate()
    {
        _playerEntity.FixedUpdated();                    
    }
    private void Update()
    {
        ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (avoid >= 0)
        {
            avoid -= Time.deltaTime;         
        }
        _playerEntity.Updated();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        //Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (_playerEntity.CharacterState == Enum_CharacterState.Avoid)
        {
            //_basePlayerEntity.GetComponent<Character>().animator.SetBool("Move", true);
            return;
        }
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100,1 << LayerMask.NameToLayer("Ground")))
        {

            print(hit.collider);
            test.position = hit.point;
            _playerEntity.InputVec = new Vector3(hit.point.x, _playerEntity.transform.position.y,
            hit.point.z);
            _playerEntity.ChangeState((int)Enum_CharacterState.Move);
        }

        /*entitys[0].GetComponent<Warrior>().InputVec = value.Get<Vector3>();
        entitys[0].GetComponent<Warrior>().
            ChangeState(Enum_WarriorState.Move);*/
    }

    public void OnAvoid()
    {
        if (avoid > 0)
        {
            return;
        }
        else
        {
            avoid = 3f;
        }
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, 1 << LayerMask.NameToLayer("Ground")))
        {
            print(hit.point);
            test.position = hit.point;
            //entitys[0].GetComponent<Character>().InputVec = new Vector3(hit.point.x, entitys[0].transform.position.y,
            //  hit.point.z);
            _playerEntity.ChangeState((int)Enum_CharacterState.Avoid);
        }
    }

    public void OnSkillInputKeyQ()
    {
       
    }

    public void OnSkillInputKeyW()
    {
      
    }
  
    public void OnSkillInputKeyE()
    {
      
    }
  
    public void OnSkillInputKeyR()
    {
       
    }






}
