using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Notification : UI_Popup
{
    enum Enum_Buttons
    {
        Accept,
        Cancel
    }

    enum Enum_Texts
    {
        TextContents
    }

    enum Enum_GameObjects
    {
        Notification
    }

    enum Enum_Images
    {
    }

    private void Start()
    {
        Init();
    }

    // ���۽ÿ� enum ���� UI_Base ��ũ��Ʈ�� Bind �Լ��� �̿��Ͽ� ������ �޾ƿ�
    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Enum_Buttons));
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));
                
        GetText((int)Enum_Texts.TextContents).text = "Do you want to proceed like this?";

        // Drag ��� ����
        GameObject go = GetObject((int)Enum_GameObjects.Notification).gameObject;
        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, UI_Define.Enum_UIEvent.Drag);
    }

    public void OnButtonClicked(PointerEventData data)
    {
    }
}
