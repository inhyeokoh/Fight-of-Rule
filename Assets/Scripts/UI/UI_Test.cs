using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test : MonoBehaviour
{

    void Start()
    {
        // ��� ����
        // GameManager.Resources.Instantiate("Prefabs/UI/Popup/UI_Button");
         GameManager.UI.ShowPopupUI<UI_Button>();
        // GameManager.UI.ClosePopupUI();
        // GameManager.UI.ShowSceneUI<UI_Inven>();
    }
}
