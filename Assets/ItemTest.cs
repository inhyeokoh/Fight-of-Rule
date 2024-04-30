//#define Test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    [SerializeField]
    StateItemData itemData;
#if Test
    void Start()
    {
        itemData = ItemData.StateItemDataReader(1011);
    }
#endif

}
