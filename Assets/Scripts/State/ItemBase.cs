using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemBase : MonoBehaviour
{
    public int itemID; //������ ��ȣ

    private string itemName; //������ �̸�
    private string itemDescription;//������ ����
    private int itemStat; //������ �ɷ�ġ
    private bool duration; //���� ���������� �ƴ���

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

    public abstract void Use();

    //�������� ������
    //�������� ȿ��
    //�������� �̸�
    //���� �������� ������ ĳ����

    //������ ���̽����� �Һ� ��Ÿ ����� ����� ����� �ִ� ��ũ��Ʈ
    //�Һ� ��� ��Ÿ �������� ����� �޾� ���� �������߉�
    
    //���� �غ��� ��Ÿ�������� ����� ������ ���� ���µ�



    //�ֿ� ��� ���������� �̿��ض�
    //���� �ð��� �ʿ��ϴ� ���� �̿��ؼ� fixidUpdate�� �ð��� ��Ÿ����
}

