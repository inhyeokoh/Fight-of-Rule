using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : SubMono<PlayerController>
{
    Animator _anim;

    public void ChangeAnimation(int stateId)
    {
        _anim.SetInteger("CharacterState", stateId);
    }

    protected override void _Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void _Excute()
    {
        throw new System.NotImplementedException();
    }

    protected override void _Init()
    {
        throw new System.NotImplementedException();
    }
}