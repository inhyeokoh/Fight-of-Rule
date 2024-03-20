using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Copy : MonoBehaviour
{
    private Skill mySkill;

    public GameObject copyObject;
    public Skill copySkill;

    private void Awake()
    {
        mySkill = gameObject.GetComponent<Skill>();
        copySkill = copyObject.GetComponent<Skill>();
    }
    void Start()
    {
        copySkill = mySkill;
        copyObject.GetComponent<Image>().sprite = copySkill.Icon;

        print(copySkill.SkillName);
    }

    
}
