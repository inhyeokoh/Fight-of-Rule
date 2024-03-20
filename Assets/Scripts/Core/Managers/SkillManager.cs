using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    //플레이어의 스킬들을 관리하기위해 만든 매니저

    //플레이어가 조건으로 인해서 습득하는거랑
    //플레이어가 이 레벨 조건과 이 스킬을 가지고 있을시에 습득가능

    //레벨 조건

    //만약에 스킬을 얻으면 이 스킬을 사용해야한다
    //플레이어가 캐릭터 직업에 따라 쓰는 스킬들이 달라진다.

    private static SkillManager _skill = null;

    public Collider[] players;   

    public static SkillManager Skill { get { return _skill; } }    

    //스킬을 쓰는 대상을 찾기위한 현재 플레이어
    public CharacterState PlayerState { get; private set; }

    //스킬을 쓰는 대상을 찾기위한 현재 플레이어
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
        //Debug.Log("생성"); //플레이어 데이터를 가져왔을때 생성 출력
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
