using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : SubMono<MonsterController>
{
    [SerializeField]
    private Animator _anim;

    public void AnimationSpeed()
    {

    }

    public void ChanageAttackAnimation(int attackCombo)
    {
        _anim.SetInteger("AttackCombo", attackCombo);
        _anim.SetTrigger("Attack");
    }

    public void ChangeAbliltyAnimation(int abliltyNumber)
    {
        _anim.SetInteger("AbliltyNumber", abliltyNumber);
        _anim.SetTrigger("Ablilty");       
    }

    public void RootMotion(bool check)
    {
        //print("RootMotion½áÁü");
        _anim.applyRootMotion = check;
    }

    public void ChangeMoveAnimation(int stateId)
    {
        RootMotion(false);
        _anim.SetInteger("Move", stateId);
    }

    public void ChangeTrrigerAnimation(string state)
    {
        _anim.SetTrigger(state);
    }

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {
        _anim = gameObject.GetComponent<Animator>();
       
    }
}

