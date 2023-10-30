using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : MonoBehaviour
{
    WaitForSeconds first = new WaitForSeconds(5f);

    
    public void Skill0(Character entity)
    {
        StartCoroutine(Coroutine_Skill0(entity));
    }
    public void Skill1(Character entity)
    {
        StartCoroutine(Coroutine_Skill1(entity));
    }
    public void Skill2(Character entity)
    {
        StartCoroutine(Coroutine_Skill2(entity));
    }

    public void Skill3(Character entity)
    {
        StartCoroutine(Coroutine_Skill3(entity));
    }
    IEnumerator Coroutine_Skill0(Character entity)
    {
        entity.animator.SetBool("Move", true);
        yield return first;
    }

    IEnumerator Coroutine_Skill1(Character entity)
    {
        yield return first;

        //entity.ChangeState(Enum_CharacterState.Idle);
    }


    IEnumerator Coroutine_Skill2(Character entity)
    {
        yield return first;
    }

    IEnumerator Coroutine_Skill3(Character entity)
    {
        //entity.animator.SetFloat

        yield return first;
    }
}
