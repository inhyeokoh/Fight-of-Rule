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

    [SerializeField]
    private KeySlotUISetting keySlotUISetting;

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

    
    // 현재 캐릭터 스킬 리턴
    public Skill[] CharacterSkillSet()
    {
        return characterSkills;
    }

    // 스킬창 스킬들 레벨 올리셋
    public void SkillAllReset()
    {
        keySlotUISetting.KeySlotAllReset();

        for (int i = 0; i < characterSkills.Length; i++)
        {
            characterSkills[i].LevelReset();
        }
    }

    // 현재 스킬 레벨업
    public void SkillLevelUp(Skill skill)
    {
        skill.LevelUp();
    }
 
    // 현재 스킬 리셋
    public void SkillReset(Skill skill)
    {
        keySlotUISetting.KeySlotSkillReset(skill);

        skill.LevelReset();
    }

}
