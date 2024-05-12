using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Enum_DetailType
{
    Default,
    Weapon,
    Head,
    Body,
    Hand,
    Foot,
    Potion,
    Box
}

public enum Enum_Grade
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary,
}
  
public enum Enum_ItemType  
{    
    Equipment, 
    Consumption,    
    ETC
}
public class ItemObject : MonoBehaviour
{
    // 아이템 정보
    public ItemData item;
    //public TMP_Text ItemName;

    public Camera cam;

    [SerializeField]
    GameObject itemText;

    protected CharacterStatus player;   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerController.instance._interaction.InGameItemEnter(gameObject);
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance._interaction.InGameItemExit(gameObject);
        }
    }

    private void Update()
    {
        if(cam != null)
        {
            itemText.transform.LookAt(itemText.transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
        }
    }

    public void Setting(ItemData item)
    {
        Cam();
        this.item = item;
        itemText.GetComponent<TMP_Text>().text = this.item.name;       
    }

    public void Cam()
    {
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }
 
    //아이템의 정보들
    //아이템의 효과
    //아이템의 이름
    //현재 아이템을 적용할 캐릭터

    //아이템 베이스에는 소비 기타 장비의 공통된 기능을 넣는 스크립트
    //소비 장비 기타 아이템은 상속을 받아 따로 만들어줘야됌

    //주요 기능 상태패턴을 이용해라
}

