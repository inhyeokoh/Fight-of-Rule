using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    public override void Setup(string name)
    {
       /* print("캐릭터클래스거쳐감");
        states.Add((int)Enum_CharacterState.Idle, new State<Character>((Character entity) =>
        {
            Debug.Log("가만히 있는다");
            //   animator.SetBool("Idle", true);
        },
        (Character entity) =>
        {
            print("Update에서 가만히 있는중");
        },
        (Character entity) =>
        {
            print("FixedUpdate에서 가만히 있는중");
        },
        (Character entity) =>
        {

        }));

        base.Setup(name);*/
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
