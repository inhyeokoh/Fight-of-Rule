using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemObjects;

    public static ItemManager _item;
    void Awake()
    {
        if (_item == null)
        {
            _item = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public GameObject ItemInstance(ItemData data, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject;

        switch (data.itemGrade)
        {
            case Enum_Grade.Normal:
                gameObject = Instantiate(itemObjects[0], position, rotation);
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            case Enum_Grade.Rare:
                gameObject = Instantiate(itemObjects[1], position, rotation);
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            case Enum_Grade.Epic:
                gameObject = Instantiate(itemObjects[2], position, rotation);
                gameObject.GetComponent<ItemObject>().Setting(data);      
                break;
            case Enum_Grade.Unique:
                gameObject = Instantiate(itemObjects[3], position, rotation);
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            case Enum_Grade.Legendary:
                gameObject = Instantiate(itemObjects[4], position, rotation);
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            default:
                gameObject = null;
                break;
        }

        return gameObject;

    }
}
