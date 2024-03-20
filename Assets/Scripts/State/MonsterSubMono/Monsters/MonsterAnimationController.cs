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


    // 공격 애니메이션을 동작시키고 바꾸는 메서드
    public void ChanageAttackAnimation(int attackCombo)
    {
        _anim.SetInteger("AttackCombo", attackCombo);
        _anim.SetTrigger("Attack");
    }

    // 몬스터의 스킬 애니메이션을 동작시키는 메서드
    public void ChangeAbliltyAnimation(int abliltyNumber)
    {
        _anim.SetInteger("AbliltyNumber", abliltyNumber);
        _anim.SetTrigger("Ablilty");       
    }

    // 현재 애니메이션이 잘 작동되지않는걸 루트모션으로 체크하는건데 이거때문에도 버그가 생기는거같음
    public void RootMotion(bool check)
    {
        //print("RootMotion써짐");
        _anim.applyRootMotion = check;
    }

    // 이동하는 애니메이션을 작동시키는 메서드
    public void ChangeMoveAnimation(int stateId)
    {
        RootMotion(false);
        _anim.SetInteger("Move", stateId);
    }

    // 현재 바로 애니메이션을 전환하기위한 메서드
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

