using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{ 
 
    public override void LevelUp()
    {

    }
    IEnumerator Skill()
    {
        yield return new WaitForSeconds(2f);
        print("���� �뱳�� ��������?");
    }

    protected override void _Init()
    {
        print($"{rigid.velocity}");
        print("ĳ����Ŭ�������İ�");



        //ĳ���� idle���� �տ��� �Ķ���͸� ������ �ǰ� ���ٽ��� �̿��� Enter,FixedStay,Stay,Exit������ Action<T> �븮�� ����
        //���� �Ƚᵵ �Ǵ� �κ��� ��������� ȣ���ص���
        states.Add((int)Enum_CharacterState.Idle, new State(() =>
        {
            Debug.Log("������ �ִ´�");
        },
        () =>
        {
            print("FixedUpdate���� ������ �ִ���");
        },
        () =>
        {

            print("Update���� ������ �ִ���");
        },
        () =>
        {
            Debug.Log("�ൿ ����");
        }));



        states.Add((int)Enum_CharacterState.Move, new State(() =>
        {
            Debug.Log("�����̱� ����");
            //entity.gameObject.transform.LookAt(new Vector3(entity.InputVec.x, 0, entity.InputVec.z));

            //*Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -

            //  Debug.Log(Vector3.Distance(entity.transform.position, entity.InputVec));
            // Debug.Log($"hit ������ : { entity.InputVec}, dir ������ : {dir.normalized}");
        },
        () =>
        {
            Vector3 dir = InputVec - transform.position;
            //* Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
            transform.rotation = Quaternion.LookRotation(dir);
            // Vector3 nextVec = entity.InputVec; //* entity.Speed * Time.fixedDeltaTime;
            // entity.Rigid.velocity = nextVec * entity.Speed; //MovePosition(entity.Rigid.position + nextVec);

            if (Vector3.Distance(transform.position, InputVec) >= 0.2f)
            {
                rigid.velocity = dir.normalized * Speed;
                //  Debug.Log(dir);
            }
            else
            {
                rigid.velocity = Vector3.zero;
                ChangeState((int)Enum_CharacterState.Idle);
            }


            if (InputVec.x == 0f && InputVec.z == 0f)
            {
                ChangeState((int)Enum_CharacterState.Idle);
            }

        },
        () =>
        {

        },
        () =>
        {
        }));

        states.Add((int)Enum_CharacterState.Attack, new State(() =>
        {

        },
       () =>
       {

       },
       () =>
       {
       },
       () =>
       {
       }));

        states.Add((int)Enum_CharacterState.Avoid, new State(() =>
        {

        },
       () =>
       {

       },
       () =>
       {
       },
       () =>
       {
       }));

        states.Add((int)Enum_CharacterState.Hit, new State(() =>
        {

        },
       () =>
       {

       },
       () =>
       {
       },
       () =>
       {
       }));

        states.Add((int)Enum_CharacterState.Fall, new State(() =>
        {

        },
       () =>
       {

       },
       () =>
       {
       },
       () =>
       {
       }));

        states.Add((int)Enum_CharacterState.Die, new State(() =>
        {

        },
       () =>
       {

       },
       () =>
       {
       },
       () =>
       {
       }));
        base._Init();

    }

    protected override void _Excute()
    {
       
    }

    protected override void _Clear()
    {
        
    }
}
