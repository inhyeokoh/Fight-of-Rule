using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float offsetRotationX;
    public float offsetRotationY;
    public float offsetRotationZ;

    public float zoomSpeed = 5.0f;
    public float rotationSpeed = 4.0f;
    public float zoomOffsetY;
    public float zoomOffsetZ1;
    public float zoomOffsetZ2;
    private int zoomLvl = 1;

    [SerializeField]
    private float scrollSpeed = 1.0f;

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        if (zoomLvl == 1)
        {
            Vector3 desiredPosition =
                new Vector3(
                    target.transform.position.x + offsetX,
                    target.transform.position.y + offsetY,
                    target.transform.position.z + offsetZ);
            Vector3 zoomLv1Position = Vector3.Lerp(transform.position, desiredPosition, zoomSpeed * Time.deltaTime);
            transform.position = zoomLv1Position;
            Quaternion FixedRot = Quaternion.Euler(
                offsetRotationX, offsetRotationY, offsetRotationZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, FixedRot, rotationSpeed * Time.deltaTime);
        }
        else if (zoomLvl == 2)
        {
            Vector3 desiredPosition = new Vector3(target.transform.position.x,
            target.transform.position.y + zoomOffsetY, target.transform.position.z + zoomOffsetZ1);
            Vector3 zoomLv2Position = Vector3.Lerp(transform.position, desiredPosition, zoomSpeed * Time.deltaTime);
            transform.position = zoomLv2Position;

            Vector3 lookDirection = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else if (zoomLvl == 3)
        {
            Vector3 desiredPosition = new Vector3(target.transform.position.x,
            target.transform.position.y + zoomOffsetY, target.transform.position.z + zoomOffsetZ2);
            Vector3 zoomLv3Position = Vector3.Lerp(transform.position, desiredPosition, zoomSpeed * Time.deltaTime);
            transform.position = zoomLv3Position;

            Vector3 lookDirection = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        ScrollUpDown();

    }

    private void ScrollUpDown()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");


        if (scrollValue > 0 && zoomLvl < 3)
        {
            zoomLvl++;
            //Debug.Log(scrollValue + "" + zoomLvl);
            
        }

        if (scrollValue < 0 && zoomLvl > 1)
        {
            zoomLvl--;
            //Debug.Log(scrollValue + "" + zoomLvl);
        }
    }

}
