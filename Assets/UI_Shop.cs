using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UI_Shop : UI_Entity
{
    public GameObject descrPanel;
    public GameObject purchaseCountConfirmPanel;
    public GameObject notifyFull;

    Toggle[] panel_U_Buttons;

    public Rect panelRect;
    Vector2 _descrUISize;

    // 드래그 Field
    private Vector2 _shopUIPos;
    private Vector2 _dragBeginPos;
    private Vector2 _offset;

    enum Enum_UI_Shop
    {
        Interact,
        Close,
        Panel,
        Panel_U,
        DescrPanel,
        NotifyFull,
        PurchaseCountConfirm
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Shop);
    }

    private void OnDisable()
    {
        GameManager.UI.PointerOnUI(false);
    }

    protected override void Init()
    {
        base.Init();
        descrPanel = _entities[(int)Enum_UI_Shop.DescrPanel].gameObject;
        panel_U_Buttons = _entities[(int)Enum_UI_Shop.Panel_U].GetComponentsInChildren<Toggle>();
        purchaseCountConfirmPanel = _entities[(int)Enum_UI_Shop.PurchaseCountConfirm].gameObject;
        notifyFull = _entities[(int)Enum_UI_Shop.NotifyFull].gameObject;
        panelRect = _entities[(int)Enum_UI_Shop.Panel].GetComponent<RectTransform>().rect;
        _descrUISize = _GetUISize(descrPanel);

        _SetPanel_UButtons();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Shop);
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

        // 상점 창 드래그 시작
        _entities[(int)Enum_UI_Shop.Interact].BeginDragAction = (PointerEventData data) =>
        {
            _shopUIPos = transform.position;
            _dragBeginPos = data.position;
        };

        // 상점 창 드래그
        _entities[(int)Enum_UI_Shop.Interact].DragAction = (PointerEventData data) =>
        {
            _offset = data.position - _dragBeginPos;
            transform.position = _shopUIPos + _offset;
        };

        // 상점 창 닫기
        _entities[(int)Enum_UI_Shop.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Shop);
        };
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

    void _SetPanel_UButtons()
    {
        for (int i = 0; i < panel_U_Buttons.Length; i++)
        {
            int index = i;
            panel_U_Buttons[i].onValueChanged.AddListener((value) => _ToggleValueChanged(index));
        }
    }

    // panel_U_Buttons 선택에 따라서 해당되는 내용을 활성화
    void _ToggleValueChanged(int toggleIndex)
    {
        bool isToggleOn = panel_U_Buttons[toggleIndex].isOn;
        GameObject childObject = _entities[(int)Enum_UI_Shop.Panel].transform.GetChild(toggleIndex).gameObject;
        childObject.SetActive(isToggleOn);
    }
}
