using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UI_MiniMap : UI_Entity
{
    public Transform player;
    public Transform miniMapCameraPosition;
    public Camera miniMapCamera;

    public bool miniMapIconCheck;
    public bool miniMapCheck;
    public bool miniMapPlayerEnemyCheck;

    enum Enum_UI_MiniMap
    {      
        Zoom_In,    
        Zoom_Out,
        MiniMapIcon,
        MiniMap,
        MiniMapPlayerEnemy
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraTransform = player.position;
        cameraTransform.y = miniMapCameraPosition.position.y;
        miniMapCameraPosition.position = cameraTransform;
        
    }

    protected override void Init()
    {
        base.Init();
        miniMapCamera = GameObject.Find("MiniMapCamera").GetComponent<Camera>();
        miniMapCameraPosition = miniMapCamera.gameObject.transform;
        player = PlayerController.instance._playerStat.gameObject.transform;

        _entities[(int)Enum_UI_MiniMap.Zoom_In].ClickAction = (PointerEventData data) => 
        {
            if (miniMapCamera.orthographicSize > 10)
            {
                miniMapCamera.orthographicSize -= 20f;       
            }
            else
            {
                return;
            }
        };
        _entities[(int)Enum_UI_MiniMap.Zoom_Out].ClickAction = (PointerEventData data) =>
        {
            if (miniMapCamera.orthographicSize < 50)
            {
                miniMapCamera.orthographicSize += 20f;
            }
            else
            {
                return;
            }
        };
        _entities[(int)Enum_UI_MiniMap.MiniMapIcon].ClickAction = (PointerEventData data) =>
        {
            print("눌렀음");
            if (!miniMapIconCheck)
            {
                miniMapIconCheck = true;
                miniMapCamera.cullingMask = miniMapCamera.cullingMask & ~(1 << LayerMask.NameToLayer("MiniMapIcon"));
            }
            else
            {
                miniMapIconCheck = false;
                miniMapCamera.cullingMask |= 1 << LayerMask.NameToLayer("MiniMapIcon");
            }
        };
        _entities[(int)Enum_UI_MiniMap.MiniMap].ClickAction = (PointerEventData data) =>
        {
            print("눌렀음");
            if (!miniMapCheck)
            {
                miniMapCheck = true;               
                int layer = LayerMask.NameToLayer("MiniMap");
                print(layer);
                miniMapCamera.cullingMask = miniMapCamera.cullingMask & ~(1 << LayerMask.NameToLayer("MiniMap"));
            }
            else
            {
                miniMapCheck = false;
                miniMapCamera.cullingMask |= 1 << LayerMask.NameToLayer("MiniMap");
            }
        };
        _entities[(int)Enum_UI_MiniMap.MiniMapPlayerEnemy].ClickAction = (PointerEventData data) =>
        {
            print("눌렀음");
            if (!miniMapPlayerEnemyCheck)
            {
                miniMapPlayerEnemyCheck = true;
                miniMapCamera.cullingMask = miniMapCamera.cullingMask & ~(1 << LayerMask.NameToLayer("MiniMapPlayerEnemy"));
            }
            else
            {
                miniMapPlayerEnemyCheck = false;
                miniMapCamera.cullingMask |= 1 << LayerMask.NameToLayer("MiniMapPlayerEnemy");
            }
        };
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_MiniMap);
    }


/*    public void OnOff(string name, out bool check)
    {
        if (!check)
        {
            check = true;
            miniMapCamera.cullingMask = miniMapCamera.cullingMask & ~(1 << LayerMask.NameToLayer("MiniMapPlayerEnemy"));
        }
        else
        {
            check = false;
            miniMapCamera.cullingMask |= 1 << LayerMask.NameToLayer("MiniMapPlayerEnemy");
        }
    }*/

}
