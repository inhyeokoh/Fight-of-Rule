using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Data
{
    public string itemType; //������ Ÿ��
    public string itemName; //������ �̸�
    public string itemDescription;//������ ����
    public string itemNumber; //������ ����
    public bool isUsing; //������ ����� ����

    // public string itemID; //������ ��ȣ
    // private int itemStat; //������ �ɷ�ġ
    // private bool duration; //���� ���������� �ƴ���

    public enum Enum_ItemType
    {
        Consumption,
        Equipment,
        Etc
    }

    private void OnEnable()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // public void Use();

    //�������� ������
    //�������� ȿ��
    //�������� �̸�
    //���� �������� ������ ĳ����

    //������ ���̽����� �Һ� ��Ÿ ����� ����� ����� �ִ� ��ũ��Ʈ
    //�Һ� ��� ��Ÿ �������� ����� �޾� ���� �������߉�

    //���� �غ��� ��Ÿ�������� ����� ������ ���� ���µ�



    //�ֿ� ��� ���������� �̿��ض�
    //���� �ð��� �ʿ��ϴ� ���� �̿��ؼ� fixidUpdate�� �ð��� ��Ÿ����

    public Item(string _itemType, string _itemName, string _itemDescription, string _itemNumber, bool _isUsing)
    {
        itemType = _itemType;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemNumber = _itemNumber;
        isUsing = _isUsing;
    }

}
