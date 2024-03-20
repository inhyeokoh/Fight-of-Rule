using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBuffCheck : MonoBehaviour
{ 
    public static bool ra;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{gameObject.name} : {ra}");   
    }
}
