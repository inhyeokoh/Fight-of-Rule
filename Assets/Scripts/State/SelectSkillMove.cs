using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillMove : MonoBehaviour
{
    Skill SelectSkill;

    [SerializeField]
    Image SelectSkillImage;

    public void Awake()
    {
        SelectSkillImage = GetComponent<Image>();
    }

    public void ClickSkill(Skill skill)
    {
        SelectSkill = skill;
        SelectSkillImage.sprite = skill.Icon;      
    }
}
