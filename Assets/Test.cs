using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    InGameStateItem item;
    void Start()
    {
        StateItemData data = GameManager.Data.StateItemDataReader(1004) as StateItemData;
        item = new InGameStateItem();
        item.Setting(data);

        PlayerController.instance._playerEquipment.EquipmentCheck(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
