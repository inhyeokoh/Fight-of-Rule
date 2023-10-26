using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Skill", menuName = "SkillEdit")]
public class SkillEdit : ScriptableObject
{
    public string skillname; //스킬 이름
    public string skillExplanation; // 스킬 설명서
    public string skillIncrease; // 스킬 공격력 || 방어력 || 마력 등등 증가량
    public int skillMP; // 드는 마나
    public int skillDamage; // 데미지
    public int skillLevel; // 필요 레벨
    public float cool; // 쿨타임
    public Sprite icon; // 스킬 아이콘
}
