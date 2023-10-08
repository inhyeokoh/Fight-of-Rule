using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum Enum_GameObjects
    {
        GridPanel,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        // UI_Base의 Bind와 Get사용
        Bind<GameObject>(typeof(Enum_GameObjects));
        GameObject gridPanel = Get<GameObject>((int)Enum_GameObjects.GridPanel);

        // 자식 오브젝트들을 지우기
        foreach (Transform child in gridPanel.transform)
        {
            GameManager.Resources.Destroy(child.gameObject);
        }

        // 인벤토리 채우기
        for (int i = 0; i < 8; i++) // 일단 8개로 임시
        {
            GameObject item = GameManager.Resources.Instantiate("UI/Scene/UI_Inven_Item");
            item.transform.SetParent(gridPanel.transform);
        }

    }

}
