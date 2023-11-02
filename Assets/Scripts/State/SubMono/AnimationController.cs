using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : SubMono<PlayerController>
{
    public Animator _anim;

    // �ִϸ��̼��� ��Ʈ���ϴ� ���
    // ĳ���͵��� �ִϸ��̼��� �������� ��� �����ؾ���

    public void ChangeAnimation(int stateId)
    {
        _anim.SetInteger("CharacterState", stateId);
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
        _anim = _board._playerEntity.GetComponent<Animator>();
        print(_anim.gameObject.name);
    }
}