using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Setting : UI_Entity
{
    // ---------왼쪽 패널--------
    int settingTypesCount;
    GameObject[] settingTypes;
    Toggle[] settingTypeToggles;

    // ---------오른쪽 패널------
    // 볼륨설정
    int volSlidersCount;
    Slider[] volSliders;
    // ------------------------

    // 드래그 Field
    Vector2 _UIPos;
    Vector2 _dragBeginPos;
    Vector2 _offset;

    enum Enum_UI_Settings
    {        
        Panel,
        Interact,
        Panel_L,
        Scrollbar_L,
        Panel_R,
        Scrollbar_R,
        Close,
        Reset,
        Accept,
        Cancel
    }

    enum Enum_SettingTypes
    {
        Audio,
        GamePlay,
        ShortCutKey,
    }

    enum Enum_SliderTypes
    {
        Master,
        BGM,
        Effect,    
        Voice
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Settings);
    }

    protected override void Init()
    {
        base.Init();

        settingTypesCount = Enum.GetValues(typeof(Enum_SettingTypes)).Length;
        volSlidersCount = Enum.GetValues(typeof(Enum_SliderTypes)).Length;

        _SetPanel_L();
        _SetVolOptions();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Settings);
            };
        }
        // 버튼 기능 할당      
        _entities[(int)Enum_UI_Settings.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Settings);
        };
        _entities[(int)Enum_UI_Settings.Reset].ClickAction = (PointerEventData data) =>
        {
            _ResetVolOptions(0.5f);
        };
        _entities[(int)Enum_UI_Settings.Accept].ClickAction = (PointerEventData data) =>
        {
            _SaveVolOptions();
            GameManager.UI.ClosePopup(GameManager.UI.Settings);
        };
        _entities[(int)Enum_UI_Settings.Cancel].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Settings);
        };

        // 팝업 드래그
        _entities[(int)Enum_UI_Settings.Interact].BeginDragAction = (PointerEventData data) =>
        {
            _UIPos = transform.position;
            _dragBeginPos = data.position;
        };

        _entities[(int)Enum_UI_Settings.Interact].DragAction = (PointerEventData data) =>
        {
            _offset = data.position - _dragBeginPos;
            transform.position = _UIPos + _offset;
        };

        gameObject.SetActive(false);
    }

    void _SetPanel_L()
    {
        settingTypes = new GameObject[settingTypesCount];
        settingTypeToggles = new Toggle[settingTypesCount];
        for (int i = 0; i < settingTypesCount; i++)
        {
            settingTypes[i] = GameManager.Resources.Instantiate("Prefabs/UI/Scene/SettingsLeftToggle", _entities[(int)Enum_UI_Settings.Panel_L].transform);
            TMP_Text togName = settingTypes[i].GetComponentInChildren<TMP_Text>();
            switch (i)
            {
                case 0:
                    togName.text = "Audio";
                    break;
                case 1:
                    togName.text = "GamePlay";
                    break;
                case 2:
                    togName.text = "ShortCut Key";
                    break;
                default:
                    break;
            }

            int index = i;
            settingTypeToggles[i] = settingTypes[i].GetComponent<Toggle>();
            settingTypeToggles[i].onValueChanged.AddListener((value) => _ToggleValueChanged(index));
        }
    }

    void _SetVolOptions()
    {
        volSliders = new Slider[volSlidersCount];
        for (int i = 0; i < volSlidersCount; i++)
        {
            GameObject volume = GameManager.Resources.Instantiate("Prefabs/UI/Scene/VolumeSlider", _entities[(int)Enum_UI_Settings.Panel_R].transform.GetChild(0).transform);
            volSliders[i] = volume.GetComponentInChildren<Slider>();
            TMP_Text[] texts = volume.GetComponentsInChildren<TMP_Text>();

            // 저장된 볼륨 설정 + 텍스트 변경
            switch (i)
            {
                case 0:
                    texts[0].text = Enum.GetName(typeof(Enum_SliderTypes), 0);
                    volSliders[i].value = GameManager.Data.settings.TotalVol;
                    break;
                case 1:
                    texts[0].text = Enum.GetName(typeof(Enum_SliderTypes), 1);
                    volSliders[i].value = GameManager.Data.settings.BackgroundVol;
                    break;
                case 2:
                    texts[0].text = Enum.GetName(typeof(Enum_SliderTypes), 2);
                    volSliders[i].value = GameManager.Data.settings.EffectVol;
                    break;
                case 3:
                    texts[0].text = Enum.GetName(typeof(Enum_SliderTypes), 3);
                    // volSliders[i].value = GameManager.Data.settings.VoiceVol;
                    break;
                default:
                    break;
            }
            texts[1].text = $"{volSliders[i].value * 100} %";
            texts[2].text = $"{texts[0].text} On";
        }  
    }

    // Panel_L에 있는 토글 선택 여부에 따라서 해당되는 내용을 Panel_R 에 활성화 (ex. Panel_L 볼륨 설정 -> Panel_R 볼륨 설정 옵션들)
    void _ToggleValueChanged(int toggleIndex)
    {   
        bool isToggleOn = settingTypeToggles[toggleIndex].isOn;                
        Transform childObject = _entities[(int)Enum_UI_Settings.Panel_R].transform.GetChild(toggleIndex);
        childObject.gameObject.SetActive(isToggleOn);
    }

    // 볼륨 초기화
    void _ResetVolOptions(float value)
    {
        foreach (var slider in volSliders)
        {
            slider.value = value;
        }
    }

    /// <summary>
    /// 기존값이랑 비교해서 1% 이상 차이날 경우 로컬 저장 + 서버 저장 요청
    /// </summary>
    void _SaveVolOptions()
    {
        if (GameManager.Data.settings.TotalVol - volSliders[0].value > 0.01f || 
            GameManager.Data.settings.BackgroundVol - volSliders[1].value > 0.01f ||
            GameManager.Data.settings.EffectVol - volSliders[2].value > 0.01f) // 값의 차이가 1% 이상 나는 부분이 있다면,
        {
            GameManager.Data.settings.TotalVol = volSliders[0].value;
            GameManager.Data.settings.BackgroundVol = volSliders[1].value;
            GameManager.Data.settings.EffectVol = volSliders[2].value;

            // 저장할 볼륨 서버로 전송
/*            C_SAVE_VOL_OPTIONS save_vol_options = new C_SAVE_VOL_OPTIONS();
            save_vol_options.GetOptions = GameManager.Data.settings;

            GameManager.Network.Send(PacketHandler.Instance.SerializePacket(save_vol_options));*/
        }
    }
}
