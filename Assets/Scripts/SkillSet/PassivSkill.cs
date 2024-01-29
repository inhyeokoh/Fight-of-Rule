using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivSkill : Skill
{
    [SerializeField]
    private int[] statsUp;

    public override Skill Init()
    {
        switch (PlayerController.instance._class)
        {
            case Enum_Class.Warrior:
                {
                    Instantiate(gameObject).transform.parent = SkillManager.Skill.transform.GetChild(0);
                    gameObject.SetActive(false);
                    break;
                }
            case Enum_Class.Archer:
                {
                    Instantiate(gameObject).transform.parent = SkillManager.Skill.transform.GetChild(1);
                    gameObject.SetActive(false);
                    break;
                }
            case Enum_Class.Wizard:
                {
                    Instantiate(gameObject).transform.parent = SkillManager.Skill.transform.GetChild(2);
                    gameObject.SetActive(false);
                    break;
                }
        }

        return default;
    }
    public override void LevelUp()
    {
        
    }

    public override void Use()
    {
        BuffOn(statsUp[level]);
    }

    public override void BuffOn(int statsUp)
    {
        SkillManager.Skill.PlayerStat.HP += statsUp;
    }
    public override void BuffOff(int statsDown)
    {
        SkillManager.Skill.PlayerStat.HP -= statsDown;
    }

    public override void SkillStat()
    {
        
    }
}
