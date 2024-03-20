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


    // ���� �ִϸ��̼��� ���۽�Ű�� �ٲٴ� �޼���
    public void ChanageAttackAnimation(int attackCombo)
    {
        _anim.SetInteger("AttackCombo", attackCombo);
        _anim.SetTrigger("Attack");
    }

    // ������ ��ų �ִϸ��̼��� ���۽�Ű�� �޼���
    public void ChangeAbliltyAnimation(int abliltyNumber)
    {
        _anim.SetInteger("AbliltyNumber", abliltyNumber);
        _anim.SetTrigger("Ablilty");       
    }

    // ���� �ִϸ��̼��� �� �۵������ʴ°� ��Ʈ������� üũ�ϴ°ǵ� �̰Ŷ������� ���װ� ����°Ű���
    public void RootMotion(bool check)
    {
        //print("RootMotion����");
        _anim.applyRootMotion = check;
    }

    // �̵��ϴ� �ִϸ��̼��� �۵���Ű�� �޼���
    public void ChangeMoveAnimation(int stateId)
    {
        RootMotion(false);
        _anim.SetInteger("Move", stateId);
    }

    // ���� �ٷ� �ִϸ��̼��� ��ȯ�ϱ����� �޼���
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

