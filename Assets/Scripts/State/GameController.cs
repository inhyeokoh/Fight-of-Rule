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

    private List<BaseGameEntity> entitys;

    private bool isDie;


    private void Awake()
    {
        for (int i = 0; i < warriorClassPrefabs.Length; i++)
        {
            GameObject clone = Instantiate(warriorClassPrefabs[0]);
            Warrior warrior = clone.GetComponent<Warrior>();
            entitys.Add(warrior);
        }
    }


    private void Update()
    {
        for (int i = 0; i < entitys.Count; i++)
        {
            if (entitys[i])
            {
                entitys[i].Updated();
            }
          
        }
    }


}
