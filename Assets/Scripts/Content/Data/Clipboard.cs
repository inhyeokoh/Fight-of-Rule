using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clipboard : MonoBehaviour
{
  //  public List<WarriorSkillData> warriorSkillData = new List<WarriorSkillData>();

   /* private void Start()
    {
        List<Dictionary<string, object>> skill = CSVReader.Read("Data/SkillDBWarrior");

        for (int i = 0; i < skill.Count; i++) 
        {
            WarriorSkillData warrirSkill;

            int id = int.Parse(skill[i]["skill_id"].ToString());
            string name = skill[i]["skill_name"].ToString();
            string desc = skill[i]["skill_desc"].ToString();
            //string icon = skill[i]["skill_name"].ToString();
            WarriorSkill skillNumber = (WarriorSkill)Enum.Parse(typeof(WarriorSkill), skill[i]["skill_number"].ToString());
            int skillMaxLevel = int.Parse(skill[i]["skill_maxlevel"].ToString());

            int[] skillLevelCondition = Array.ConvertAll(skill[i]["skill_levelcondition"].ToString().Split(","), int.Parse);
            int[] skillPoint = Array.ConvertAll(skill[i]["skill_point"].ToString().Split(","), int.Parse);
            int[] skillMP = Array.ConvertAll(skill[i]["skill_mp"].ToString().Split(","), int.Parse);
            int[] skillCool = Array.ConvertAll(skill[i]["skill_cool"].ToString().Split(","), int.Parse);
            int[] skillDamage = Array.ConvertAll(skill[i]["skill_damage"].ToString().Split(","), int.Parse);


            warrirSkill = new WarriorSkillData(id, name, desc, null, skillNumber, skillMaxLevel, skillLevelCondition, skillPoint, skillMP, skillCool, skillDamage);

          
            warriorSkillData.Add(warrirSkill);
        }

        print(warriorSkillData.Count);
    }*/
}
