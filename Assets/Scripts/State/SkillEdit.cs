using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Skill", menuName = "SkillEdit")]
public class SkillEdit : ScriptableObject
{
    public string skillname; //��ų �̸�
    public string skillExplanation; // ��ų ����
    public string skillIncrease; // ��ų ���ݷ� || ���� || ���� ��� ������
    public int skillMP; // ��� ����
    public int skillDamage; // ������
    public int skillLevel; // �ʿ� ����
    public float cool; // ��Ÿ��
    public Sprite icon; // ��ų ������
}
