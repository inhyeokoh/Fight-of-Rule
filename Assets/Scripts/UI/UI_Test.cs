using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test : MonoBehaviour
{

    void Start()
    {
        // 사용 예시
        GameManager.UI.ShowPopupUI<UI_Notification>();
        // GameManager.UI.ClosePopupUI();
        // GameManager.UI.ShowSceneUI<UI_Inven>();
    }
}
