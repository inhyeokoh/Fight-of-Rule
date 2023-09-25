using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] warriorClassPrefabs;

    [SerializeField]
    Transform test;

    public List<BaseGameEntity> entitys;
    private float avoid = 0;

    public Camera camera;
    Ray ray;


    private void Awake()
    {
        for (int i = 0; i < warriorClassPrefabs.Length; i++)
        {
            GameObject clone = Instantiate(warriorClassPrefabs[0]);
            Warrior warrior = clone.GetComponent<Warrior>();
            entitys.Add(warrior);
        }

        entitys[0].GetComponent<Warrior>().Setup("ภüป็");
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        
        for (int i = 0; i < entitys.Count; i++)
        {
            if (entitys[i])
            {
                entitys[i].FixedUpdated();
            }
        }
    }
    private void Update()
    {
        ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (avoid >= 0)
        {
            avoid -= Time.deltaTime;         
        }


        for (int i = 0; i < entitys.Count; i++)
        {
            if (entitys[i])
            {
                entitys[i].Updated();
            }        
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        //Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (entitys[0].GetComponent<Warrior>().WarriorState == Enum_WarriorState.Avoid)
        {
            entitys[0].GetComponent<Warrior>().animator.SetBool("Move", true);
            return;
        }
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            print(hit.point);
            test.position = hit.point;
            entitys[0].GetComponent<Warrior>().InputVec = new Vector3(hit.point.x, entitys[0].transform.position.y,
                hit.point.z); 
            entitys[0].GetComponent<Warrior>().ChangeState(Enum_WarriorState.Move);
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
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            print(hit.point);
            test.position = hit.point;
            entitys[0].GetComponent<Warrior>().InputVec = new Vector3(hit.point.x, entitys[0].transform.position.y,
                hit.point.z);
            entitys[0].GetComponent<Warrior>().ChangeState(Enum_WarriorState.Avoid);
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
