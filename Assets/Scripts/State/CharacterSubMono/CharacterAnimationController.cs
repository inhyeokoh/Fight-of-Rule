using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : SubMono<PlayerController>
{
    private Animator _anim;

    // 애니메이션을 컨트롤하는 기능
    // 캐릭터들의 애니메이션을 통합으로 묶어서 관리해야함

    public void AnimationSpeed()
    {
       
    }


    // 캐릭터의 이동 애니메이션 체크
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

    // 캐릭터의 공격 애니메이션 및 공격 순서에 따른 애니메이션 작동
    public void ChanageAttackAnimation(int attackCombo)
    {
        _anim.SetInteger("AttackCombo", attackCombo);
        _anim.SetTrigger("Attack");
    }
    
    // 루트모션 사용하는 메서드 근데 버그가 많이생김
    public void RootMotion(bool check)
    {
        //print("RootMotion써짐");
        _anim.applyRootMotion = check;          
    }

    //이동상태를 확인하기 위한 메서드

    public void ChangeMoveAnimation(int stateId)
    {
        RootMotion(false);
        _anim.SetInteger("Move", stateId);
    }

    // 트리거를 인한 애니메이션을 전환시키는 메서드
    public void ChangeTrrigerAnimation(string state)
    {
        RootMotion(false);
        _anim.SetTrigger(state);      
    }

    // 스킬 상태패턴에서 스킬 번호들을 가져와 애니메이션을 작동시키는 메서드
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