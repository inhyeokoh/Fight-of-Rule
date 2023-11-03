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
    [SerializeField] private GameObject _slotUiPrefab;
    [SerializeField] private int _slotCount;
    [SerializeField] private List<ItemSlotUI> _slotUIList;


    
    private void Start()
    {
        _slotCount = (_horizontalLength / (int)_slotSize) * (_verticalLength / (int)_slotSize);
        InitSlots();
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
            int slotIndex = 1 + i;

            var slotRT = CloneSlot();
            slotRT.gameObject.SetActive(true);
            slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

            // rectTransform인데 ItemSlotUI 컴포넌트를 가져올 수 있나?
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

}
