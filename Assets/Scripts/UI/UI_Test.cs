using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test : MonoBehaviour
{

    void Start()
    {
        // ��� ����
        GameManager.UI.ShowPopupUI<UI_Notification>();
        // GameManager.UI.ClosePopupUI();
        // GameManager.UI.ShowSceneUI<UI_Inven>();
    }
}
