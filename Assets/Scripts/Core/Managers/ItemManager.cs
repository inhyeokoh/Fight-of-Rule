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
            //itemObjects = new GameObject[5];

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public GameObject ItemInstance(ItemData data, Vector3 pos, Quaternion rotation)
    {
        GameObject gameObject;

        switch (data.itemGrade)
        {
            case Enum_Grade.Normal:
                gameObject = GameManager.Resources.Instantiate("Prefabs/InGameItemObject/NormalItem");
                gameObject.transform.position = pos;
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            case Enum_Grade.Rare:
                gameObject = GameManager.Resources.Instantiate("Prefabs/InGameItemObject/RareItem");
                gameObject.transform.position = pos;
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            case Enum_Grade.Epic:
                gameObject = GameManager.Resources.Instantiate("Prefabs/InGameItemObject/EpicItem");
                gameObject.transform.position = pos;
                gameObject.GetComponent<ItemObject>().Setting(data);      
                break;
            case Enum_Grade.Unique:
                gameObject = GameManager.Resources.Instantiate("Prefabs/InGameItemObject/UniqueItem");
                gameObject.transform.position = pos;
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            case Enum_Grade.Legendary:
                gameObject = GameManager.Resources.Instantiate("Prefabs/InGameItemObject/LegendaryItem");
                gameObject.transform.position = pos;
                //                 gameObject = Instantiate(itemObjects[4], position, rotation);
                gameObject.GetComponent<ItemObject>().Setting(data);
                break;
            default:
                gameObject = null;
                break;
        }

        return gameObject;

    }
}
