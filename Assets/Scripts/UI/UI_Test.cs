using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test : MonoBehaviour
{

    void Start()
    {
        // ��� ����
        GameManager.UI.ShowSceneUI<UI_JobSelect>();
        // GameManager.UI.ShowPopupUI<UI_Notification>();
    }

    private void Update()
    {
    }
}
