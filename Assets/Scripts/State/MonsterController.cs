using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public static MonsterController instance = null;
    public List<IBaseMonstersPart> monsters;

  /*  public MonsterController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                return null;
            }

        }
        set
        {
            instance = value;    
        }
    }*/
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        monsters = new List<IBaseMonstersPart>();

    }
    private void FixedUpdate()
    {
        print(monsters.Count);

        if (monsters != null)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].FixedUpdated();
            }
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        if (monsters != null)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].Updated();
            }
        }
    }
}
