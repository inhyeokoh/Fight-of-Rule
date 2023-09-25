using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private int cost; // 플레이어 스킬 코스트
    [SerializeField]
    private List<SkillEdit> WarriorSkill; //전사의 모든스킬


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


}
