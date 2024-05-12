using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaw : MonoBehaviour
{
    float maxDistance;
    float minDistance;

    float maxRotation;
    float minRotation;

    float spawn = 0;

    private void Awake()
    {
        maxDistance = 10f;
        minDistance = -10f;

        maxRotation = 360f;
        minRotation = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn <= 0)
        {
            int monsterID = Random.Range(0, 2);
            MonsterSpawn(monsterID);

            spawn = 3f;
        }
        else
        {
            spawn -= Time.deltaTime;
        }
    }

    void MonsterSpawn(int monsterID)
    {
        float x = Random.Range(minDistance, maxDistance);
        float z = Random.Range(minDistance, maxDistance);

        float rotation = Random.Range(minRotation, maxRotation);

        GameObject monster = GameManager.Data.MonsterInstance(monsterID);

        Instantiate(monster, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z), Quaternion.Euler(1, rotation, 1)); 
    }
}
