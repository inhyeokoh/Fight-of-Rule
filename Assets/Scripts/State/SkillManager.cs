using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private int cost; // �÷��̾� ��ų �ڽ�Ʈ
    [SerializeField]
    private List<SkillEdit> WarriorSkill; //������ ��罺ų


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


}
