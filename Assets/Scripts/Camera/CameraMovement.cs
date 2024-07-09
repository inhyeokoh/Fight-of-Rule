using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
/*    public GameObject target;

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

    public enum Enum_ZoomTypes
    {
        Default,
        DialogZoom,
    }
    Enum_ZoomTypes zoomState;
    public Enum_ZoomTypes ZoomState
    {
        get { return zoomState; }
        set
        {
            if (zoomState != value)
            {
                zoomState = value;
                OnZoomStateChanged();
            }
        }
    }
    public Vector3 NpcPos { get; set; }
    Vector3 midPos;

    [SerializeField]
    private float scrollSpeed = 1.0f;

    private void Awake()
    {
        zoomState = Enum_ZoomTypes.Default;
    }

    private void Update()
    {

        if (zoomState == Enum_ZoomTypes.DialogZoom)
        {

        }
        else if (zoomLvl == 1)
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


    void OnZoomStateChanged()
    {
        switch (zoomState)
        {
            case Enum_ZoomTypes.Default:
                zoomLvl = 1;
                break;
            case Enum_ZoomTypes.DialogZoom:
                Vector3 dialogZoomPos = CalculateDialogZoomPos();
                transform.position = dialogZoomPos;
                transform.LookAt(midPos);
                //StartCoroutine(MoveCameraToPosition(dialogZoomPos));
                break;
        }
    }

    private Vector3 CalculateDialogZoomPos()
    {
        float distance = 15f;

        // 캐릭터와 Npc의 중점
        midPos = (target.transform.position + NpcPos) / 2.0f;
        // 캐릭터에서 Npc로 향하는 벡터
        Vector3 forward = (target.transform.position - NpcPos).normalized;

        // 캐릭터의 우측 방향 벡터 (캐릭터가 바라보는 방향과 하늘 방향의 외적)
        Vector3 rightDirection = Vector3.Cross(forward, Vector3.up).normalized;
        // 캐릭터의 우측과 하늘 사이 45도 방향 벡터 계산
        Vector3 direction45 = (rightDirection + Vector3.up).normalized;

        // 목표 위치 계산
        Vector3 targetPosition = midPos + direction45 * distance;
        return targetPosition;
    }


    IEnumerator MoveCameraToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 3f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 50f);

            Quaternion targetRotation = Quaternion.LookRotation(midPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 50f);

            yield return null;
        }

        Debug.Log("while문 종료");
        transform.position = targetPosition;
        Quaternion.LookRotation(midPos - transform.position);
        //transform.LookAt(midPos);
    }*/
}
