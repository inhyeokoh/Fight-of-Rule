using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test : MonoBehaviour
{

    void Start()
    {
        // 사용 예시
        // GameManager.Resources.Instantiate("Prefabs/UI/Popup/UI_Button");
         GameManager.UI.ShowPopupUI<UI_Button>();
        // GameManager.UI.ClosePopupUI();
        // GameManager.UI.ShowSceneUI<UI_Inven>();
    }
}
