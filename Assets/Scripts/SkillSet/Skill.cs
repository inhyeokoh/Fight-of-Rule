using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    private int level; //�迭�� �����ϱ� ���� ��Ƽ��
    /* public int Level
     {
         get
         {
             return level;
         }
         set
         {
             level = value;
         }
     }*/


    private int skillID; //��ų ��ȣ
    public string skillName; //��ų �̸�
    public string skillDESC; // ��ų ���� 
    public int[] skillMP; // ��� ����
    public int[] skillDamage; // ������
    public int[] skillPoint; // �ʿ� ��ų ����Ʈ
    public int[] skillLevelCondition; // �ʿ� ����
    public int[] skillLevel; // ��ų ����
    public float[] cool; // ��Ÿ��

    public GameObject[] paticleObject; //����Ʈ
    public Sprite icon; // ��ų ������

    
    public abstract void Use();

    public void SkillLevelUp()
    {

    }
   
    public void OnEnable()
    {
        Use();
    }
}
