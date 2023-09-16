using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] warriorClassPrefabs;
    [SerializeField]
    private GameObject[] MonsterClassPrefabs;

    public List<BaseGameEntity> entitys;
    private float avoid = 0;


    private void Awake()
    {
        for (int i = 0; i < warriorClassPrefabs.Length; i++)
        {
            GameObject clone = Instantiate(warriorClassPrefabs[0]);
            Warrior warrior = clone.GetComponent<Warrior>();
            entitys.Add(warrior);
        }

        entitys[0].GetComponent<Warrior>().Setup("ภüป็");
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


    private void OnMove(InputValue value)
    {
        entitys[0].GetComponent<Warrior>().InputVec = value.Get<Vector3>();
        entitys[0].GetComponent<Warrior>().
            ChangeState(Enum_WarriorState.Move);
    }


}
