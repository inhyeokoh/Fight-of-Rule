using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_ArcherSkill
{
    ArcherSkill0 = 7,
    ArcherSkill1,
    ArcherSkill2,
    ArcherSkill3,
}

public class Archer : Character
{     
  
    public override void LevelUp()
    {
        
    }


    IEnumerator Skill0(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("움직였다");
    }
    IEnumerator Skill1(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("움직였다");
    }
    IEnumerator Skill2(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("움직였다");
    }
    IEnumerator Skill3(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("움직였다");
    }

    protected override void _Init()
    {
        print("캐릭터클래스거쳐감");



        //캐릭터 idle상태 앞에는 파라미터를 넣으면 되고 람다식을 이용해 Enter,FixedStay,Stay,Exit순으로 Action<T> 대리자 생성
        //굳이 안써도 되는 부분은 빈공간으로 호출해도됌
        states.Add((int)Enum_CharacterState.Idle, new State/*<Character>*/((/*Character entity*/) =>
        {
            Debug.Log("가만히 있는다");

            StartCoroutine("Skill0", this);
        },
        (/*Character entity*/) =>
        {
            print("FixedUpdate에서 가만히 있는중");
        },
        (/*Character entity*/) =>
        {

            print("Update에서 가만히 있는중");
        },
        (/*Character entity*/) =>
        {
            Debug.Log("행동 시작");
        }));



        states.Add((int)Enum_CharacterState.Move, new State/*<Character>*/((/*Character entity*/) =>
        {
            //   Debug.Log("움직이기 시작");
            //entity.gameObject.transform.LookAt(new Vector3(entity.InputVec.x, 0, entity.InputVec.z));         
            /*Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
                new Vector3(entity.transform.position.x, 0, entity.transform.position.z);*/
            //  Debug.Log(Vector3.Distance(entity.transform.position, entity.InputVec));
            // Debug.Log($"hit 포지션 : { entity.InputVec}, dir 포지션 : {dir.normalized}");
        },
        (/*Character entity*/) =>
        {
            Vector3 dir = InputVec - transform.position;
            /* Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
                 new Vector3(entity.transform.position.x, 0, entity.transform.position.z);*/

            transform.rotation = Quaternion.LookRotation(dir);
            //Debug.Log("움직이는중");
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


            /*  if(entity.InputVec.x == 0f && entity.InputVec.z == 0f)
              {
                  entity.ChangeState(Enum_WarriorState.Idle);
              }*/
        },
        (/*Character entity*/) =>
        {

        },
        (/*Character entity*/) =>
        {
        }));

        /*tates.Add((int)Enum_CharacterState.Attack, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_CharacterState.Avoid, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_CharacterState.Hit, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_CharacterState.Fall, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_CharacterState.Die, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_ArcherSkill.ArcherSkill0, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
        
        states.Add((int)Enum_ArcherSkill.ArcherSkill1, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_ArcherSkill.ArcherSkill2, new State<Character>((Character entity) =>
        {

        },
       (Character entity) =>
       {

       },
       (Character entity) =>
       {
       },
       (Character entity) =>
       {
       }));
       
        states.Add((int)Enum_ArcherSkill.ArcherSkill3, new State<Character>((Character entity) =>
        {

        },
        (Character entity) =>
        {

        },
        (Character entity) =>
        {
        },
        (Character entity) =>
        {
        }));

        
*/
    }

    protected override void _Excute()
    {
  
    }

    protected override void _Clear()
    {
      
    }
}
