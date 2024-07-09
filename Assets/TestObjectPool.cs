using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectPool : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    GameObject clone;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
           clone = GameManager.Resources.Instantiate(arrow, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
           clone =  GameManager.Resources.Instantiate(arrow, gameObject.transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.PoolBeta.PoolDestroy(arrow);
        }
    }

    private void Awake()
    {
        float a = 3.42312432423f;

        float ad = a * (10 * 10 * 10 * 10 * 10 * 10);

        Debug.Log(ad);
            
    }
}
