using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    //�÷��̾��� ��ų���� �����ϱ����� ���� �Ŵ���

    //�÷��̾ �������� ���ؼ� �����ϴ°Ŷ�
    //�÷��̾ �� ���� ���ǰ� �� ��ų�� ������ �����ÿ� ���氡��

    //���� ����

    //���࿡ ��ų�� ������ �� ��ų�� ����ؾ��Ѵ�
    //�÷��̾ ĳ���� ������ ���� ���� ��ų���� �޶�����.

    private static SkillManager _skill = null;

    public static SkillManager Skill { get { return _skill; } }
    

    public Character player;

    [SerializeField]
    Skill[] warriorSkills;

    [SerializeField]
    Skill[] archerSkills;

    [SerializeField]
    Skill[] wizardSkills;


    //Dictionary<Skill,>

    /*  private List<SkillEdit> WizardSkill;*/
    public void Awake()
    {
        if (_skill == null)
        {
            _skill = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SkillLevelUp(Skill skill)
    {
        
    }


    public void PlayerData()
    {
          
        player = GameObject.FindWithTag("Player").GetComponent<Character>();

#if UNITY_EDITOR
        Debug.Log("����"); //�÷��̾� �����͸� ���������� ���� ���
#endif
    }

    public void SkillReset(Skill[] skillResetDate)
    {
        for (int i = 0; i < skillResetDate.Length; i++)
        {
            // skill
        }
    }

   
    public void SkillChanage(int skillIndex)
    {

    }

  /*  public void ClassSkillLevelCheck(SkillEdit[] check, Character player)
    {
        for (int i = 0; i < check.Length; i++)
        {
        
            
        }
    }*/
}
