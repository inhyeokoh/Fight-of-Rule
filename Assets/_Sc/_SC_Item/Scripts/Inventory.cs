using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool isInventoryOn = false;
    public GameObject inventoryImg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InventoryOnOff();
    }

    private void InventoryOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOn = !isInventoryOn;
        }

        if (isInventoryOn)
        {
            inventoryImg.SetActive(true);
        }

        if (!isInventoryOn)
        {
            inventoryImg.SetActive(false);
        }
    }
}
