using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    public Transform left;
    public Transform right;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 leftVector = left.position - transform.position.normalized;
         Vector3 rightVector = right.position - transform.position.normalized;

        Vector3 Forward = transform.forward;

        



        Debug.DrawLine(transform.position, left.position, Color.black);
        Debug.DrawLine(transform.position, right.position, Color.black);

        Vector3 f = Vector3.Cross(transform.up, rightVector);

     //   print(f); 

        Debug.DrawLine(transform.position, f,Color.grey);
        // 교차되면 무조건 마이너스로 반환 
        //print(Vector3.SignedAngle(leftVector, rightVector, Vector3.up));

    }
}
