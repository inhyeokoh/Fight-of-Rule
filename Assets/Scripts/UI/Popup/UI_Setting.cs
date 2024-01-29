using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Setting : UI_Entity
{
    bool initStarted;

    Slider[] volSliders;
    TMP_Text[] volNames;
    TMP_Text[] togNames;
    Toggle[] toggles;
    
    enum Enum_UI_Settings
    {
        Panel,
        Interact,
        Panel_L,
        Panel_R,
        Scrollbar_L,
        Scrollbar_R,
        Close,
        Reset,
        Accept,
        Cancel
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Settings);
    }

    protected override void Init()
    {
        base.Init();

        SetPanel_L();
        LoadOptionsVol();

        // 버튼 할당
        _entities[(int)Enum_UI_Settings.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Setting);
        };
        _entities[(int)Enum_UI_Settings.Reset].ClickAction = (PointerEventData data) =>
        {
            ResetOptionsVol(0.5f);
        };
        _entities[(int)Enum_UI_Settings.Accept].ClickAction = (PointerEventData data) =>
        {
            SaveOptionsVol();
            GameManager.UI.ClosePopup(GameManager.UI.Setting);
        };
        _entities[(int)Enum_UI_Settings.Cancel].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Setting);
        };

        _entities[(int)Enum_UI_Settings.Interact].DragAction = (PointerEventData data) =>
        {
            transform.position = data.position;   // TODO: 드래그 수정 필요
        };

        initStarted = true;
    }

    private void OnEnable() // SetActive 때마다 호출할 함수 넣기
    {
        if (initStarted)
        {
            LoadOptionsVol();
        }
    }

    void SetPanel_L()
    {
        togNames = _entities[(int)Enum_UI_Settings.Panel_L].GetComponentsInChildren<TMP_Text>();
        toggles = _entities[(int)Enum_UI_Settings.Panel_L].GetComponentsInChildren<Toggle>();

        togNames[0].text = "Audio";
        togNames[1].text = "GamePlay";
        togNames[2].text = "ShortCut Key";

        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((value) => ToggleValueChanged(index));
        }
    }

    // Panel_L에 있는 토글에 따라서 해당되는 내용을 Panel_R 에 활성화
    void ToggleValueChanged(int toggleIndex)
    {   
        bool isToggleOn = toggles[toggleIndex].isOn;                
        Transform childObject = _entities[(int)Enum_UI_Settings.Panel_R].transform.GetChild(toggleIndex);
        childObject.gameObject.SetActive(isToggleOn);
    }

    void ResetOptionsVol(float value)
    {
        foreach (var slider in volSliders)
        {
            slider.value = value;
        }
    }

    void LoadOptionsVol()
    {
        volSliders = _entities[(int)Enum_UI_Settings.Panel_R].GetComponentsInChildren<Slider>();
        // 텍스트 수정 용도 ( 인스펙터 창에서 하는것보다 통일성 면에서 확인하기 쉬움 )
        volNames = _entities[(int)Enum_UI_Settings.Panel_R].GetComponentsInChildren<TMP_Text>();

        volSliders[0].value = GameManager.Data.setting.totalVol;
        volSliders[1].value = GameManager.Data.setting.backgroundVol;
        volSliders[2].value = GameManager.Data.setting.effectVol;

        volNames[0].text = "Total Volume";
        volNames[1].text = $"{volSliders[0].value * 100} %";
        volNames[2].text = $"{volNames[0].text} On";

        volNames[3].text = "Background Volume";
        volNames[4].text = $"{volSliders[1].value * 100} %";
        volNames[5].text = $"{volNames[3].text} On";

        volNames[6].text = "Effect Volume";
        volNames[7].text = $"{volSliders[2].value * 100} %";
        volNames[8].text = $"{volNames[6].text} On";
    }

    // 기존값이랑 비교해서 다른부분이 있을 때만 서버에 저장하도록 (float 오차 유의)
    void SaveOptionsVol()
    {
        if (GameManager.Data.setting.totalVol - volSliders[0].value > 0.001f || 
            GameManager.Data.setting.backgroundVol - volSliders[1].value > 0.001f ||
            GameManager.Data.setting.effectVol - volSliders[2].value > 0.001f) // 값의 차이가 0.1% 이상 나는 부분이 있다면,
        {
            GameManager.Data.setting.totalVol = volSliders[0].value;
            GameManager.Data.setting.backgroundVol = volSliders[1].value;
            GameManager.Data.setting.effectVol = volSliders[2].value;

            GameManager.Data.SaveData("Setting", GameManager.Data.setting); // 로컬에 저장하는 부분 -> 서버 송수신으로 변경 예정
        }
    }
    
    // TODO 사운드 매니저 제작 이후 추가작업
}
