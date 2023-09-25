using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : MonoBehaviour
{
    WaitForSeconds first = new WaitForSeconds(5f);


    public void Skill0(Warrior entity)
    {
        StartCoroutine(Coroutine_Skill0(entity));
    }
    public void Skill1(Warrior entity)
    {
        StartCoroutine(Coroutine_Skill1(entity));
    }
    public void Skill2(Warrior entity)
    {
        StartCoroutine(Coroutine_Skill2(entity));
    }

    public void Skill3(Warrior entity)
    {
        StartCoroutine(Coroutine_Skill3(entity));
    }
    IEnumerator Coroutine_Skill0(Warrior entity)
    {
        yield return first;
    }

    IEnumerator Coroutine_Skill1(Warrior entity)
    {
        yield return first;
    }


    IEnumerator Coroutine_Skill2(Warrior entity)
    {
        yield return first;
    }

    IEnumerator Coroutine_Skill3(Warrior entity)
    {
        //entity.animator.SetFloat

        yield return first;
    }
}
