using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/Portion", order = 3)]
public class PortionItemData : CountableItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float HpValue => _hpValue;
    [SerializeField] private float _hpValue;
    public float MpValue => _mpValue;
    [SerializeField] private float _mpValue;
    public float SpeedValue => _speedPercent;
    [SerializeField] private float _speedPercent;

    public override Item CreateItem()
    {
        return new PortionItem(this);
    }
}
