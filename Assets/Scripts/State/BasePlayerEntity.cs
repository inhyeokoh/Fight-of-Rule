/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerEntity : SubMono<PlayerController>
{
    //�÷��̾� �г���
    private string ninkName;
    //�÷��̾� ���� ��ȣ
    private static int ID;

    private int saveHp = 5;
    private int saveMp = 5;
    private int saveExp = 5;
    private int saveDamage = 5;
    private int saveDefense = 5;
    private int saveSpeed = 5;
    private int saveLevel = 5;


    protected int SaveHp { get { return saveHp; } }
    protected int SaveMp { get { return saveMp; } }
    protected int SaveExp { get { return saveExp; } }
    protected int SaveDamage { get { return saveDamage; } }
    protected int SaveDefense { get { return saveDefense; } }
    protected int SaveSpeed { get { return saveSpeed; } }
    protected int SaveLevel { get { return saveLevel; } }

    public virtual void Setup(string name)
    {      
        ninkName = name;
        ++ID;
    }



    public abstract void FixedUpdated();

    /// <summary>
    /// GameController�� �ִ� ��� Ŭ������ ������Ʈ�� ����
    /// </summary>
    public abstract void Updated();

    /// <summary>
    /// �ϴ� �׽�Ʈ�� �� ���� �ؽ�Ʈ ���
    /// </summary>
    public void Print(string text)
    {
        print($"{name}�� : {text}  ");
    }

}
*/