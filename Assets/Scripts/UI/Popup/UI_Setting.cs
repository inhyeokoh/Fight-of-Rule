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

        // ��ư �Ҵ�
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
            transform.position = data.position;   // TODO: �巡�� ���� �ʿ�
        };

        initStarted = true;
    }

    private void OnEnable() // SetActive ������ ȣ���� �Լ� �ֱ�
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

    // Panel_L�� �ִ� ��ۿ� ���� �ش�Ǵ� ������ Panel_R �� Ȱ��ȭ
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
        // �ؽ�Ʈ ���� �뵵 ( �ν����� â���� �ϴ°ͺ��� ���ϼ� �鿡�� Ȯ���ϱ� ���� )
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

    // �������̶� ���ؼ� �ٸ��κ��� ���� ���� ������ �����ϵ��� (float ���� ����)
    void SaveOptionsVol()
    {
        if (GameManager.Data.setting.totalVol - volSliders[0].value > 0.001f || 
            GameManager.Data.setting.backgroundVol - volSliders[1].value > 0.001f ||
            GameManager.Data.setting.effectVol - volSliders[2].value > 0.001f) // ���� ���̰� 0.1% �̻� ���� �κ��� �ִٸ�,
        {
            GameManager.Data.setting.totalVol = volSliders[0].value;
            GameManager.Data.setting.backgroundVol = volSliders[1].value;
            GameManager.Data.setting.effectVol = volSliders[2].value;

            GameManager.Data.SaveData("Setting", GameManager.Data.setting); // ���ÿ� �����ϴ� �κ� -> ���� �ۼ������� ���� ����
        }
    }
    
    // TODO ���� �Ŵ��� ���� ���� �߰��۾�
}
