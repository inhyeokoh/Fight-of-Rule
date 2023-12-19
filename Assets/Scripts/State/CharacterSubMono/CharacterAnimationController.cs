using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : SubMono<PlayerController>
{
    private Animator _anim;

    // �ִϸ��̼��� ��Ʈ���ϴ� ���
    // ĳ���͵��� �ִϸ��̼��� �������� ��� �����ؾ���

    public void AnimationSpeed()
    {
       
    }

    public bool MoveCheck()
    {
        if (_anim.GetInteger("Move") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChanageAttackAnimation(int attackCombo)
    {
        _anim.SetInteger("AttackCombo", attackCombo);
        _anim.SetTrigger("Attack");
    }
    
    public void RootMotion(bool check)
    {
        //print("RootMotion����");
        _anim.applyRootMotion = check;          
    }

    public void ChangeMoveAnimation(int stateId)
    {
        RootMotion(false);
        _anim.SetInteger("Move", stateId);
    }

    public void ChangeTrrigerAnimation(string state)
    {
        RootMotion(false);
        _anim.SetTrigger(state);      
    }
    public void ChangeSkillAnimation(int abliltyNumber)
    {
        _anim.SetInteger("SkillNumber", abliltyNumber);
        _anim.SetTrigger("SkillAciton");
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
        //_anim = _board._playerState.GetComponent<Animator>();  
    }
}