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

        // UI_Base�� Bind�� Get���
        Bind<GameObject>(typeof(Enum_GameObjects));
        GameObject gridPanel = Get<GameObject>((int)Enum_GameObjects.GridPanel);

        // �ڽ� ������Ʈ���� �����
        foreach (Transform child in gridPanel.transform)
        {
            GameManager.Resources.Destroy(child.gameObject);
        }

        // �κ��丮 ä���
        for (int i = 0; i < 8; i++) // �ϴ� 8���� �ӽ�
        {
            GameObject item = GameManager.Resources.Instantiate("UI/Scene/UI_Inven_Item");
            item.transform.SetParent(gridPanel.transform);
        }

    }

}
