using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    private int level; //배열을 조작하기 위한 인티져
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


    private int skillID; //스킬 번호
    public string skillName; //스킬 이름
    public string skillDESC; // 스킬 설명서 
    public int[] skillMP; // 드는 마나
    public int[] skillDamage; // 데미지
    public int[] skillPoint; // 필요 스킬 포인트
    public int[] skillLevelCondition; // 필요 레벨
    public int[] skillLevel; // 스킬 레벨
    public float[] cool; // 쿨타임

    public GameObject[] paticleObject; //이펙트
    public Sprite icon; // 스킬 아이콘

    
    public abstract void Use();

    public void SkillLevelUp()
    {

    }
   
    public void OnEnable()
    {
        Use();
    }
}
