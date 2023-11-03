using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Һ� ������ ���� </summary>
[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/Portion", order = 3)]
public class PortionItemData : CountableItemData
{
    /// <summary> ȿ����(ȸ���� ��) </summary>
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
