using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    public override void Setup(string name)
    {
       /* print("ĳ����Ŭ�������İ�");
        states.Add((int)Enum_CharacterState.Idle, new State<Character>((Character entity) =>
        {
            Debug.Log("������ �ִ´�");
            //   animator.SetBool("Idle", true);
        },
        (Character entity) =>
        {
            print("Update���� ������ �ִ���");
        },
        (Character entity) =>
        {
            print("FixedUpdate���� ������ �ִ���");
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
