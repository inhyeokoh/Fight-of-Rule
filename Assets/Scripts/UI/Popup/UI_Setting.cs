using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// ȯ�漳��â�� �������� �ʰ� �ֻ�� order��
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

        // �г� �Ʒ��� �������� ������ ����
        for (int i = 0; i < volumes; i++)
        {
            GameObject volume = GameManager.Resources.Instantiate("Prefabs/UI/Scene/UI_Volume", panelR.transform);
            UI_Volume volumeName = Utils.GetOrAddComponent<UI_Volume>(volume); // ��ũ��Ʈ ����

            volumeName.SetInfo(Enum.GetName(typeof(Enum_VolumeTypes), i)); // Enum ��ȣ ������� ���� �̸� ���
        }

        // TODO : �ʱ�ȭ ��ư
        GameObject reset = GetButton((int)Enum_Buttons.Reset).gameObject;
        AddUIEvent(reset, (PointerEventData data) =>
        {
            Slider[] sliders = GameObject.FindObjectsOfType<Slider>();
            for (int i = 0; i < volumes; i++)
            {
                sliders[i].value = 0.5f;
            }
        }, UI_Define.Enum_UIEvent.Click);

        // TODO : ���� ��ư (���� ���� �ϰ� â ����)
        /*        GameObject accept = GetButton((int)Enum_Buttons.Accept).gameObject;
                AddUIEvent(accept, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);*/

        // ��� ��ư Ŭ�� ��, â ����
        GameObject close = GetButton((int)Enum_Buttons.Close).gameObject;
        AddUIEvent(close, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);

        // ��� ��ư Ŭ�� ��, â ����
        GameObject cancel = GetButton((int)Enum_Buttons.Cancel).gameObject;
        AddUIEvent(cancel, (PointerEventData data) => { GameManager.UI.ClosePopupUI(); }, UI_Define.Enum_UIEvent.Click);
    }
}
