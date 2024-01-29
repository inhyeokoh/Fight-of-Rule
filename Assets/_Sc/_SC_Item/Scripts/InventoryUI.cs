using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("ItemSlotCreate")]
    [Range(0, 1920)]
    [SerializeField] private int _horizontalLength = 400;
    [SerializeField] private int _verticalLength = 400;
    [Range(40, 100)]
    [SerializeField] private float _slotSize = 40f;

    [SerializeField] private RectTransform _slotAreaRT;
    [SerializeField] private int _slotCount;
    [SerializeField] private List<ItemSlotUI> _slotUIList;

    [Space]
    [SerializeField] private bool _showTooltip = true;
    [SerializeField] private bool _showHighlight = true;
    [SerializeField] private bool _showRemovingPopup = true;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform _contentAreaRT; // ���Ե��� ��ġ�� ����
    [SerializeField] private GameObject _slotUiPrefab;     // ������ ���� ������
    [SerializeField] private ItemTooltipUI _itemTooltip;   // ������ ������ ������ ���� UI
    [SerializeField] private InventoryPopupUI _popup;

    private Inventory _inventory;

    private ItemSlotUI _pointerOverSlot; // ���� �����Ͱ� ��ġ�� ���� ����
    public int slotCount;

    //������ �巡�� �� ���
    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    private ItemSlotUI _beginDragSlot; // ���� �巡�׸� ������ ����
    private Transform _beginDragIconTransform; // �ش� ������ ������ Ʈ������

    private int _leftClick = 0;
    private int _rightClick = 1;

    private Vector3 _beginDragIconPoint;   // �巡�� ���� �� ������ ��ġ
    private Vector3 _beginDragCursorPoint; // �巡�� ���� �� Ŀ���� ��ġ
    private int _beginDragSlotSiblingIndex;


    private void Awake()
    {
        _slotCount = (_horizontalLength / (int)_slotSize) * (_verticalLength / (int)_slotSize);
        slotCount = _slotCount;
        Init();
        InitSlots();
    }

    private void Init()
    {
        TryGetComponent(out _gr);
        if (_gr == null)
            _gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>(10);

    }
    private void InitSlots()
    {
        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
        {
            _slotUiPrefab.AddComponent<ItemSlotUI>();
        }

        _slotUiPrefab.SetActive(false);

        _slotUIList = new List<ItemSlotUI>(_slotCount);

        for (int i = 0; i < _slotCount; i++)
        {
            int slotIndex = i;

            var slotRT = CloneSlot();
            slotRT.gameObject.SetActive(true);
            slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

            // rectTransform�ε� ItemSlotUI ������Ʈ�� ������ �� �ֳ�?
            var slotUI = slotRT.GetComponent<ItemSlotUI>();
            slotUI.SetSlotIndex(slotIndex);
            _slotUIList.Add(slotUI);

        }

        RectTransform CloneSlot()
        {
            GameObject slotInit = Instantiate(_slotUiPrefab);
            RectTransform rt = slotInit.GetComponent<RectTransform>();
            rt.SetParent(_slotAreaRT);

            return rt;
        }
    }

    /// <summary> �κ��丮 ���� ��� (�κ��丮���� ���� ȣ��) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary> ���Կ� ������ ������ ��� </summary>
    public void SetItemIcon(int index, Sprite icon)
    {
        _slotUIList[index].SetItem(icon);
    }

    /// <summary> �ش� ������ ������ ���� �ؽ�Ʈ ���� </summary>
    public void SetItemAmountText(int index, int amount)
    {
        EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount�� 1 ������ ��� �ؽ�Ʈ ��ǥ��
        _slotUIList[index].SetItemAmount(amount);
    }

    /// <summary> �ش� ������ ������ ���� �ؽ�Ʈ ���� </summary>
    public void HideItemAmountText(int index)
    {
        EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> ���Կ��� ������ ������ ����, ���� �ؽ�Ʈ ����� </summary>
    public void RemoveItem(int index)
    {
        EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].RemoveItem();
    }

    /// <summary> ���� ������ ���� ���� ���� </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }


    [Header("Editor Options")]
    [SerializeField] private bool _showDebug = true;
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void EditorLog(object message)
    {
        if (!_showDebug) return;
        UnityEngine.Debug.Log($"[InventoryUI] {message}");
    }

    private void EndDrag()
    {
        ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        // ������ ���Գ��� ������ ��ȯ �Ǵ� �̵�
        if (endDragSlot != null && endDragSlot.IsAccessible)
        {
            // ���� ������ ����
            // 1) ���콺 Ŭ�� ���� ���� ���� Ctrl �Ǵ� Shift Ű ����
            // 2) begin : �� �� �ִ� ������ / end : ����ִ� ����
            // 3) begin �������� ���� > 1
            bool isSeparatable =
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) &&
                (_inventory.IsCountableItem(_beginDragSlot.Index) && !_inventory.HasItem(endDragSlot.Index));

            // true : ���� ������, false : ��ȯ �Ǵ� �̵�
            bool isSeparation = false;
            int currentAmount = 0;

            // ���� ���� Ȯ��
            if (isSeparatable)
            {
                currentAmount = _inventory.GetCurrentAmount(_beginDragSlot.Index);
                if (currentAmount > 1)
                {
                    isSeparation = true;
                }
            }

            // 1. ���� ������
            if (isSeparation)
                TrySeparateAmount(_beginDragSlot.Index, endDragSlot.Index, currentAmount);
            // 2. ��ȯ �Ǵ� �̵�
            else
                TrySwapItems(_beginDragSlot, endDragSlot);

            // ���� ����
            UpdateTooltipUI(endDragSlot);
            return;
        }

        // ������(Ŀ���� UI ����ĳ��Ʈ Ÿ�� ���� ���� ���� ���)
        if (!IsOverUI())
        {
            int index = _beginDragSlot.Index;
            TryRemoveItem(index);
        }
    }

    /// <summary> �� ������ ������ ��ȯ </summary>
    private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to)
        {
            return;
        }

        from.SwapOrMoveIcon(to);
        _inventory.Swap(from.Index, to.Index);
    }

    ///drag & drop
    private void Update()
    {
        _ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        if (_showTooltip) ShowOrHideItemTooltip();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }
    /// <summary> ���Կ� �����Ͱ� �ö󰡴� ���, ���Կ��� �����Ͱ� ���������� ��� </summary>
    private void OnPointerEnterAndExit()
    {
        // ���� �������� ����
        var prevSlot = _pointerOverSlot;

        // ���� �������� ����
        var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        if (prevSlot == null)
        {
            // Enter
            if (curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        else
        {
            // Exit
            if (curSlot == null)
            {
                OnPrevExit();
            }

            // Change
            else if (prevSlot != curSlot)
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        // ===================== Local Methods ===============================
        void OnCurrentEnter()
        {
            if (_showHighlight)
                curSlot.Highlight(true);
        }
        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
    }
    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();

        _gr.Raycast(_ped, _rrList);

        if (_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerDown()
    {
        // Left Click : Begin Drag
        if (Input.GetMouseButtonDown(_leftClick))
        {
            _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

            // �������� ���� �ִ� ���Ը� �ش�
            if (_beginDragSlot != null && _beginDragSlot.HasItem)
            {
                EditorLog($"Drag Begin : Slot [{_beginDragSlot.Index}]");
                // ��ġ ���, ���� ���
                _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                _beginDragIconPoint = _beginDragIconTransform.position;
                _beginDragCursorPoint = Input.mousePosition;

                // �� ���� ���̱�
                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();

                // �ش� ������ ���̶���Ʈ �̹����� �����ܺ��� �ڿ� ��ġ��Ű��
                _beginDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                _beginDragSlot = null;
            }
        }

        // Right Click : Use Item
        else if (Input.GetMouseButtonDown(_rightClick))
        {
            ItemSlotUI slot = RaycastAndGetFirstComponent<ItemSlotUI>();

            if (slot != null && slot.HasItem && slot.IsAccessible)
            {
                TryUseItem(slot.Index);
            }
        }
    }
    /// <summary> �巡���ϴ� ���� </summary>
    private void OnPointerDrag()
    {
        if (_beginDragSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            // ��ġ �̵�
            _beginDragIconTransform.position =
                _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
        }
    }
    /// <summary> Ŭ���� �� ��� </summary>
    private void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // End Drag
            if (_beginDragSlot != null)
            {
                // ��ġ ����
                _beginDragIconTransform.position = _beginDragIconPoint;

                // UI ���� ����
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                // �巡�� �Ϸ� ó��
                EndDrag();

                // ���� ����
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }


    /// <summary> UI �� �κ��丮���� ������ ���� </summary>
    private void TryRemoveItem(int index)
    {
        _inventory.Remove(index);
    }

    /// <summary> ������ ��� </summary>
    private void TryUseItem(int index)
    {
        EditorLog($"UI - Try Use Item : Slot [{index}]");

        _inventory.Use(index);
    }

    /// <summary> �� �� �ִ� ������ ���� ������ </summary>
    private void TrySeparateAmount(int indexA, int indexB, int amount)
    {
        if (indexA == indexB)
        {
            EditorLog($"UI - Try Separate Amount: Same Slot [{indexA}]");
            return;
        }

        EditorLog($"UI - Try Separate Amount: Slot [{indexA} -> {indexB}]");

        string itemName = $"{_inventory.GetItemName(indexA)} x{amount}";

        _popup.OpenAmountInputPopup(
            amt => _inventory.SeparateAmount(indexA, indexB, amt),
            amount, itemName
        );
    }

    /// <summary> ���� UI�� ���� ������ ���� </summary>
    private void UpdateTooltipUI(ItemSlotUI slot)
    {
        if (!slot.IsAccessible || !slot.HasItem)
            return;

        // ���� ���� ����
        _itemTooltip.SetItemInfo(_inventory.GetItemData(slot.Index));

        // ���� ��ġ ����
        _itemTooltip.SetRectPosition(slot.SlotRect);
    }

    private void ShowOrHideItemTooltip()
    {
        // ���콺�� ��ȿ�� ������ ������ ���� �ö�� �ִٸ� ���� �����ֱ�
        bool isValid =
            _pointerOverSlot != null && _pointerOverSlot.HasItem && _pointerOverSlot.IsAccessible
            && (_pointerOverSlot != _beginDragSlot); // �巡�� ������ �����̸� �������� �ʱ�

        if (isValid)
        {
            UpdateTooltipUI(_pointerOverSlot);
            _itemTooltip.Show();
        }
        else
            _itemTooltip.Hide();
    }
    private bool IsOverUI()
        => EventSystem.current.IsPointerOverGameObject();
}
