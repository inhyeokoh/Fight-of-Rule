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

    public Collider[] players;   

    public static SkillManager Skill { get { return _skill; } }    

    //��ų�� ���� ����� ã������ ���� �÷��̾�
    public CharacterState PlayerState { get; private set; }

    //��ų�� ���� ����� ã������ ���� �÷��̾�
    public CharacterStatus PlayerStat { get; private set; }


    [SerializeField]
    Skill[] warriorSkills; 

    [SerializeField]
    Skill[] archerSkills;

    [SerializeField]
    Skill[] wizardSkills;
    
    [SerializeField]
    Skill[] characterSkills;

  
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

    public void Update()
    {
      // players = Physics.OverlapSphere(Player.transform.position, 30f, 1 << 7);

       
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawWireSphere(player.transform.position, 30f);
    }

    public void SkillLevelUp(Skill skill)
    {
        skill.LevelUp();
    }


    public void PlayerData()
    {
        PlayerState = PlayerController.instance._playerState;
        PlayerStat = PlayerController.instance._playerStat;
        switch (PlayerController.instance._class)
        {
            case Enum_Class.Warrior:
                {
                    characterSkills = new Skill[warriorSkills.Length];

                    for (int i = 0; i < warriorSkills.Length; i++)
                    {                       
                        characterSkills[i] = warriorSkills[i].Init();
                        characterSkills[i].SkillStat();
                    }
                    break;
                }
            case Enum_Class.Archer:
                {
                    characterSkills = new Skill[archerSkills.Length];

                    for (int i = 0; i < archerSkills.Length; i++)
                    {
                        characterSkills[i] = archerSkills[i].Init();
                        characterSkills[i].SkillStat();
                    }
                    break;
                }
            case Enum_Class.Wizard:
                {
                    characterSkills = new Skill[wizardSkills.Length];

                    for (int i = 0; i < wizardSkills.Length; i++)
                    {
                        characterSkills[i] = wizardSkills[i].Init();
                        characterSkills[i].SkillStat();
                    }
                    break;
                }
        }

#if UNITY_EDITOR
        //Debug.Log("����"); //�÷��̾� �����͸� ���������� ���� ���
#endif
    }

    public void SkillReset(Skill[] skillResetDate)
    {
        for (int i = 0; i < skillResetDate.Length; i++)
        {
            characterSkills[i].SkillReset();
        }
    }
  /*  public void ClassSkillLevelCheck(SkillEdit[] check, Character player)
    {
        for (int i = 0; i < check.Length; i++)
        {
        
            
        }
    }*/
}
