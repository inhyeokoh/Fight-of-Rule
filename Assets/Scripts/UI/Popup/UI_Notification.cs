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
        Interactable
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

        // ���� �ؽ�Ʈ, ����, ��� ���� ����
        GetText((int)Enum_Texts.TextContents).text = "Do you want to proceed like this?";
        /*GetText((int)Enum_Texts.AcceptText).text = "��ȭ ����";
        GetText((int)Enum_Texts.CancelText).text = "��ȭ ���";*/


        // �巡�� ���
        GameObject interact = GetImage((int)Enum_Images.Interactable).gameObject;
        AddUIEvent(interact, (PointerEventData data) => { interact.transform.parent.position = data.position; }, UI_Define.Enum_UIEvent.Drag);

        // ��� ��ư Ŭ�� ��, â ����
        GameObject cancel = GetButton((int)Enum_Buttons.Cancel).gameObject;
        AddUIEvent(cancel, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);

        // TODO : ������ư
/*        GameObject accept = GetButton((int)Enum_Buttons.Accept).gameObject;
        AddUIEvent(accept, (PointerEventData data) => { *//* ���� ���� *//*}, *//* �̺�Ʈ ���� ����*//* );*/

    }
}
