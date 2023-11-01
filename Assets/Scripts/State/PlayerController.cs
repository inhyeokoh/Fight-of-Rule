using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    public BasePlayerEntity _basePlayerEntity;
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

    Character character;
    Ray ray;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        string charcterClass = "Warrior";

        switch (charcterClass)
        {
            case "Warrior":               
                { 
                    GameObject clone = Instantiate(ClassPrefabs[0]);
                    character = clone.GetComponent<Warrior>();                
                    break;
                }
            case "Archer":
                {
                    GameObject clone = Instantiate(ClassPrefabs[1]);
                    character = clone.GetComponent<Archer>();                  
                    break;
                }
            case "Wizard": 
                {
                    GameObject clone = Instantiate(ClassPrefabs[2]);
                    character = clone.GetComponent<Wizard>();                
                    break;
                }               
        }

        character.Setup("±Ã¼ö");
        SkillManager.Skill.PlayerData();

        camera = Camera.main;

    }

    private void Start()
    {
      
    }

    private void FixedUpdate()
    {
        character.FixedUpdated();                    
    }
    private void Update()
    {
        ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (avoid >= 0)
        {
            avoid -= Time.deltaTime;         
        }
        character.Updated();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        //Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (character.GetComponent<Character>().CharacterState == Enum_CharacterState.Avoid)
        {
            character.GetComponent<Character>().animator.SetBool("Move", true);
            return;
        }
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100,1 << LayerMask.NameToLayer("Ground")))
        {

            print(hit.collider);
            test.position = hit.point;
            character.GetComponent<Character>().InputVec = new Vector3(hit.point.x, character.transform.position.y,
            hit.point.z); 
            character.GetComponent<Character>().ChangeState((int)Enum_CharacterState.Move);
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
            character.GetComponent<Character>().ChangeState((int)Enum_CharacterState.Avoid);
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
