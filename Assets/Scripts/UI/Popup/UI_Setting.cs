using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// 환경설정창은 움직이지 않고 최상단 order로
public class UI_Setting : UI_Popup
{
    enum Enum_Buttons
    {
        Close,
        Reset,
        Accept,
        Cancel
    }

    enum Enum_Texts
    {
    }

    enum Enum_GameObjects
    {
    }

    enum Enum_Images
    {
        Panel_L,
        Panel_R
    }

    enum Enum_VolumeTypes
    {
        Total_Volume,
        System_Voice,
        Background_Music,
        Character_Dialogue,
        Environmental_Sound,
        Sound_Effect
    }

    int volumes = 6;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Enum_Buttons));
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));

        Image panelR = GetImage((int)Enum_Images.Panel_R);

        // 패널 아래에 볼륨별로 프리팹 생성
        for (int i = 0; i < volumes; i++)
        {
            GameObject volume = GameManager.Resources.Instantiate("Prefabs/UI/Scene/UI_Volume", panelR.transform);
            UI_Volume volumeName = Utils.GetOrAddComponent<UI_Volume>(volume); // 스크립트 부착

            volumeName.SetInfo(Enum.GetName(typeof(Enum_VolumeTypes), i)); // Enum 번호 순서대로 볼륨 이름 등록
        }

        // TODO : 초기화 버튼
        GameObject reset = GetButton((int)Enum_Buttons.Reset).gameObject;
        AddUIEvent(reset, (PointerEventData data) =>
        {
            Slider[] sliders = GameObject.FindObjectsOfType<Slider>();
            for (int i = 0; i < volumes; i++)
            {
                sliders[i].value = 0.5f;
            }
        }, UI_Define.Enum_UIEvent.Click);

        // TODO : 수락 버튼 (설정 저장 하고 창 닫힘)
        /*        GameObject accept = GetButton((int)Enum_Buttons.Accept).gameObject;
                AddUIEvent(accept, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);*/

        // 취소 버튼 클릭 시, 창 닫힘
        GameObject close = GetButton((int)Enum_Buttons.Close).gameObject;
        AddUIEvent(close, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);

        // 취소 버튼 클릭 시, 창 닫힘
        GameObject cancel = GetButton((int)Enum_Buttons.Cancel).gameObject;
        AddUIEvent(cancel, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);
    }
}
