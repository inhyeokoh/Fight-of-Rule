using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator anim;
    public GameObject inventoryOutline;


    private void Start()
    {
        anim = GetComponent<Animator>();
        inventoryOutline.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventdata)
    {
        anim.SetTrigger("isMouseDetected");
        inventoryOutline.transform.SetParent(GameObject.Find("Canvas").transform);
        inventoryOutline.transform.SetAsLastSibling();
        inventoryOutline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        inventoryOutline.SetActive(false);
    }
}
