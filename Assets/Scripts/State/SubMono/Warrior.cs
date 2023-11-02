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
        print("마포 대교는 무너졌냐?");
    }

    protected override void _Init()
    {
        print($"{rigid.velocity}");
        print("캐릭터클래스거쳐감");



        //캐릭터 idle상태 앞에는 파라미터를 넣으면 되고 람다식을 이용해 Enter,FixedStay,Stay,Exit순으로 Action<T> 대리자 생성
        //굳이 안써도 되는 부분은 빈공간으로 호출해도됌
        states.Add((int)Enum_CharacterState.Idle, new State(() =>
        {
            Debug.Log("가만히 있는다");
        },
        () =>
        {
            print("FixedUpdate에서 가만히 있는중");
        },
        () =>
        {

            print("Update에서 가만히 있는중");
        },
        () =>
        {
            Debug.Log("행동 시작");
        }));



        states.Add((int)Enum_CharacterState.Move, new State(() =>
        {
            Debug.Log("움직이기 시작");
            //entity.gameObject.transform.LookAt(new Vector3(entity.InputVec.x, 0, entity.InputVec.z));

            //*Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -

            //  Debug.Log(Vector3.Distance(entity.transform.position, entity.InputVec));
            // Debug.Log($"hit 포지션 : { entity.InputVec}, dir 포지션 : {dir.normalized}");
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
