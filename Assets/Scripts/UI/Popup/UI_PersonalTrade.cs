using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PersonalTrade : UI_Entity
{
    public List<ItemData> tradeItems;

    Vector2 _descrUISize;
    TMP_Text goldText;
    public Rect panelRect;

    #region 아이템 드래그 이미지, 설명 패널
    public Image dragImg;
    public GameObject descrPanel;
    public TMP_Text descrPanelItemNameText;
    public Image descrPanelItemImage;
    public TMP_Text descrPanelDescrText;
    public int? highlightedSlotIndex = null;
    #endregion

    #region 개인거래 UI 드래그
    Vector2 _UIPos;
    Vector2 _dragBeginPos;
    Vector2 _offset;
    #endregion

    enum Enum_UI_PersonalTrade
    {
        Panel,
        Interact,
        Trade,
        Others,
        Mine,
        Tip,
        Close
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_PersonalTrade);
    }

    public override void PopupOnEnable()
    {
        GameManager.UI.BlockPlayerActions(UIManager.Enum_ControlInputAction.BlockPlayerInput, true);
    }

    public override void PopupOnDisable()
    {
        GameManager.UI.BlockPlayerActions(UIManager.Enum_ControlInputAction.BlockPlayerInput, false);
    }

    protected override void Init()
    {
        base.Init();
        #region 초기설정 및 캐싱
        
        #endregion

        // 개인 거래창 드래그
        _entities[(int)Enum_UI_PersonalTrade.Interact].BeginDragAction = (PointerEventData data) =>
        {
            _UIPos = transform.position;
            _dragBeginPos = data.position;
        };
        _entities[(int)Enum_UI_PersonalTrade.Interact].DragAction = (PointerEventData data) =>
        {
            _offset = data.position - _dragBeginPos;
            transform.position = _UIPos + _offset;
        };

        _entities[(int)Enum_UI_PersonalTrade.Trade].DragAction = (PointerEventData data) =>
        {
            // TODO tradeItems, gold 패킷 만들어서 서버로 전달
        };

        _entities[(int)Enum_UI_PersonalTrade.Close].DragAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(this);
        };

        gameObject.SetActive(false);
    }
}
