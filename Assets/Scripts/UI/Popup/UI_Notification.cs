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

    // 시작시에 enum 별로 UI_Base 스크립트의 Bind 함수를 이용하여 정보를 받아옴
    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Enum_Buttons));
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));

        // 메인 텍스트, 수락, 취소 내용 변경
        GetText((int)Enum_Texts.TextContents).text = "Do you want to proceed like this?";
        /*GetText((int)Enum_Texts.AcceptText).text = "강화 진행";
        GetText((int)Enum_Texts.CancelText).text = "강화 취소";*/


        // 드래그 기능
        GameObject interact = GetImage((int)Enum_Images.Interactable).gameObject;
        AddUIEvent(interact, (PointerEventData data) => { interact.transform.parent.position = data.position; }, UI_Define.Enum_UIEvent.Drag);

        // 취소 버튼 클릭 시, 창 닫힘
        GameObject cancel = GetButton((int)Enum_Buttons.Cancel).gameObject;
        AddUIEvent(cancel, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);

        // TODO : 수락버튼
/*        GameObject accept = GetButton((int)Enum_Buttons.Accept).gameObject;
        AddUIEvent(accept, (PointerEventData data) => { *//* 내용 적기 *//*}, *//* 이벤트 내용 적기*//* );*/

    }
}
