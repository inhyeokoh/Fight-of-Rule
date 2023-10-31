using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Volume : UI_Scene
{
    enum Enum_Buttons
    {
    }

    enum Enum_Texts
    {
        Volume_Name,
        Volume_Value,
        Label
    }

    enum Enum_GameObjects
    {
        Slider,
        Handle
    }

    enum Enum_Images
    {
    }

    string _name;
    Slider _slider;
    TMP_Text _valueText;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<Button>(typeof(Enum_Buttons));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));

        // ���� �̸�, ��� �̸� ����
        GetText((int)Enum_Texts.Volume_Name).text = _name;
        GetText((int)Enum_Texts.Label).text = $"{_name} On";

        _slider = GetObject((int)Enum_GameObjects.Slider).GetComponent<Slider>();
        _valueText = GetText((int)Enum_Texts.Volume_Value).GetComponent<TMP_Text>();

        // �ʱ� ������ ǥ��
        AudioListener.volume = _slider.value;
        _valueText.text = $"{_slider.value}% ";


        // TODO: ���� ���� �̺�Ʈ UI eventhandler���� ���� ã�Ƽ� ����ʿ�(?)
/*        GameObject handle = GetObject((int)Enum_GameObjects.Handle).gameObject;
        AddUIEvent(handle, (PointerEventData data) => { _slider.onValueChanged.AddListener(ChangeVolume); }, UI_Define.Enum_UIEvent.Click);*/
    }

    public void SetInfo(string name)
    {
        _name = name;
    }

    void ChangeVolume(float value)
    {
        AudioListener.volume = _slider.value;
        _valueText.text = $"{_slider.value}% ";
    }
}