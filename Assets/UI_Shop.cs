using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Shop : UI_Entity
{
    GameObject shopSlots;
    public GameObject dragImg;
    public GameObject descrPanel;
    public GameObject dropConfirmPanel;
    public GameObject dropCountConfirmPanel;
    public GameObject basket;

    public ItemData[] shopItems;
    public int shopItemCount;

    public Rect panelRect;
    Vector2 _descrUISize;

    // 드래그 Field
    private Vector2 _shopUIPos;
    private Vector2 _dragBeginPos;
    private Vector2 _offset;

    enum Enum_UI_Shop
    {
        Interact,
        Panel,
        Panel_U,
        ShopSlots,
        Close,
        DragImg,
        DescrPanel,
        DropConfirm,
        DropCountConfirm,
        Basket,
        Purchase,
        Reset
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
        dragImg = _entities[(int)Enum_UI_Shop.DragImg].gameObject;
        descrPanel = _entities[(int)Enum_UI_Shop.DescrPanel].gameObject;
        dropConfirmPanel = _entities[(int)Enum_UI_Shop.DropConfirm].gameObject;
        shopSlots = _entities[(int)Enum_UI_Shop.ShopSlots].gameObject;
        dropCountConfirmPanel = _entities[(int)Enum_UI_Shop.DropCountConfirm].gameObject;
        basket = _entities[(int)Enum_UI_Shop.Basket].gameObject;
        panelRect = _entities[(int)Enum_UI_Shop.Panel].GetComponent<RectTransform>().rect;
        _descrUISize = _GetUISize(descrPanel);
        _DrawSlots();

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

        // 유저 정보 창 닫기
        _entities[(int)Enum_UI_Shop.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Shop);
        };

        gameObject.SetActive(false);
    }

  
    void _DrawSlots()
    {
        var item = CSVReader.Read("Data/ShopItems");
        shopItemCount = item.Count;
        shopItems = new ItemData[shopItemCount];

        for (int i = 0; i < 8; i++)
        {
            GameObject _shopSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ShopSlot", shopSlots.transform);
            _shopSlot.name = "Slot_" + i;
            _shopSlot.GetComponent<UI_ShopSlot>().index = i;

            if (i < item.Count)
            {
                int id = int.Parse(item[i]["id"]);
                int count = int.Parse(item[i]["count"]);

                // id,count받고 해당하는 id로 아이템 생성
                shopItems[i] = ItemParsing.StateItemDataReader(id);
                shopItems[i].count = count;

                _shopSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = shopItems[i].name; // 이름
                _shopSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = $"{shopItems[i].purchaseprice}"; // 구매 가격
            }
            else
            {
                // 표기 비활성화
                for (int j = 1; j < 5; j++)
                {
                    _shopSlot.transform.GetChild(j).gameObject.SetActive(false);
                }
            }
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

    // UI 갱신
    public void UpdateShopUI(int slotIndex)
    {

    }
}
