using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PlayerInfo : UI_Entity
{
    bool init;
    public GameObject dragImg;
    public GameObject descrPanel;
    public GameObject dropConfirmPanel;
    public GameObject dropCountConfirmPanel;
    GameObject equipSlots;

    int _leftSlotCount = 5;

    public Vector2 playerInfoUI_leftBottom;
    public Vector2 playerInfoUI_rightTop;
    Vector2 _descrUISize;

    // 드래그 Field
    private Vector2 _playerInfoUIPos;
    private Vector2 _dragBeginPos;
    private Vector2 _offset;

    enum Enum_UI_PlayerInfo
    {
        Interact,
        Panel,
        Panel_U,
        Equipments,
        Close,
        DragImg,
        DescrPanel,
        DropConfirm,
        DropCountConfirm
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_PlayerInfo);
    }

    private void OnEnable()
    {
        if (!init)
        {
            return;
        }

        for (int i = 0; i < GameManager.Inven.equips.Count; i++)
        {
            UpdateEquipUI(i);
        }
    }

    private void OnDisable()
    {
        GameManager.UI.PointerOnUI(false);
    }

    protected override void Init()
    {
        base.Init();
        equipSlots = _entities[(int)Enum_UI_PlayerInfo.Equipments].gameObject;
        dragImg = _entities[(int)Enum_UI_PlayerInfo.DragImg].gameObject;
        descrPanel = _entities[(int)Enum_UI_PlayerInfo.DescrPanel].gameObject;
        dropConfirmPanel = _entities[(int)Enum_UI_PlayerInfo.DropConfirm].gameObject;
        dropCountConfirmPanel = _entities[(int)Enum_UI_PlayerInfo.DropCountConfirm].gameObject;
        _descrUISize = _GetUISize(descrPanel);
        _DrawSlots();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.PlayerInfo);
            };

            // UI위에 커서 있을 시 캐릭터 행동 제약
            _subUI.PointerEnterAction = (PointerEventData data) =>
            {
                GameManager.UI.PointerOnUI(true);
            };

            _subUI.PointerExitAction = (PointerEventData data) =>
            {
                GameManager.UI.PointerOnUI(false);
            };
        }

        // 유저 정보 창 드래그 시작
        _entities[(int)Enum_UI_PlayerInfo.Interact].BeginDragAction = (PointerEventData data) =>
        {
            _playerInfoUIPos = transform.position;
            _dragBeginPos = data.position;
        };

        // 유저 정보 창 드래그
        _entities[(int)Enum_UI_PlayerInfo.Interact].DragAction = (PointerEventData data) =>
        {
            _offset = data.position - _dragBeginPos;
            transform.position = _playerInfoUIPos + _offset;
        };

        // 유저 정보 창 닫기
        _entities[(int)Enum_UI_PlayerInfo.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.PlayerInfo);
        };

        init = true;
    }

    // 유저 정보 창 내 초기 장비 슬롯 생성
    void _DrawSlots()
    {
        for (int i = 0; i < _leftSlotCount; i++)
        {
            GameObject _equipSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/EquipSlot", equipSlots.transform.GetChild(1));
            _equipSlot.name = "EquipSlot_" + i;
            _equipSlot.GetComponent<UI_EquipSlot>().index = i;
        }

        for (int i = _leftSlotCount; i < GameManager.Inven.equipSlotCount; i++)
        {
            GameObject _equipSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/EquipSlot", equipSlots.transform.GetChild(2));
            _equipSlot.name = "EquipSlot_" + i;
            _equipSlot.GetComponent<UI_EquipSlot>().index = i;
        }
    }

    public void RestrictItemDescrPos()
    {
        Vector2 option = new Vector2(300f, -165f);
        StartCoroutine(RestrictUIPos(descrPanel, _descrUISize, option));
    }

    public void StopRestrictItemDescrPos(PointerEventData data)
    {
        StopCoroutine(RestrictUIPos(descrPanel, _descrUISize));
    }

    public void GetUIPos()
    {
        playerInfoUI_leftBottom = transform.TransformPoint(GetComponent<RectTransform>().rect.min);
        playerInfoUI_rightTop = transform.TransformPoint(GetComponent<RectTransform>().rect.max);
    }

    // UI 사각형 좌표의 좌측하단과 우측상단 좌표를 전역 좌표로 바꿔서 사이즈 계산
    Vector2 _GetUISize(GameObject UI)
    {
        Vector2 leftBottom = UI.transform.TransformPoint(UI.GetComponent<RectTransform>().rect.min);
        Vector2 rightTop = UI.transform.TransformPoint(UI.GetComponent<RectTransform>().rect.max);
        Vector2 UISize = rightTop - leftBottom;
        return UISize;
    }

    // UI가 화면 밖으로 넘어가지 않도록 위치 제한
    IEnumerator RestrictUIPos(GameObject UI, Vector2 UISize, Vector2? option = null)
    {
        while (true)
        {
            Vector3 mousePos = Input.mousePosition;
            float x = Math.Clamp(mousePos.x + option.Value.x, UISize.x / 2, Screen.width - (UISize.x / 2));
            float y = Math.Clamp(mousePos.y + option.Value.y, UISize.y / 2, Screen.height - (UISize.y / 2));
            UI.transform.position = new Vector2(x, y);
            yield return null;
        }
    }

    // 아이템 배열 정보에 맞게 UI 갱신 시키는 메서드
    public void UpdateEquipUI(int slotIndex)
    {
        if (slotIndex < _leftSlotCount)
        {
            UI_EquipSlot equipSlot = equipSlots.transform.GetChild(1).GetChild(slotIndex).GetComponent<UI_EquipSlot>();
            equipSlot.ItemRender();
        }
        else
        {
            UI_EquipSlot equipSlot = equipSlots.transform.GetChild(2).GetChild(slotIndex - _leftSlotCount).GetComponent<UI_EquipSlot>();
            equipSlot.ItemRender();
        }
    }
}
