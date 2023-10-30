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
        Debug.Log("생성"); //플레이어 데이터를 가져왔을때 생성 출력
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
