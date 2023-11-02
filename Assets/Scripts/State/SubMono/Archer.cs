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
        print("��������");
    }
    IEnumerator Skill1(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("��������");
    }
    IEnumerator Skill2(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("��������");
    }
    IEnumerator Skill3(Character entity)
    {
        yield return new WaitForSeconds(2f);
        print("��������");
    }

    protected override void _Init()
    {
        print("ĳ����Ŭ�������İ�");



        //ĳ���� idle���� �տ��� �Ķ���͸� ������ �ǰ� ���ٽ��� �̿��� Enter,FixedStay,Stay,Exit������ Action<T> �븮�� ����
        //���� �Ƚᵵ �Ǵ� �κ��� ��������� ȣ���ص���
        states.Add((int)Enum_CharacterState.Idle, new State/*<Character>*/((/*Character entity*/) =>
        {
            Debug.Log("������ �ִ´�");

            StartCoroutine("Skill0", this);
        },
        (/*Character entity*/) =>
        {
            print("FixedUpdate���� ������ �ִ���");
        },
        (/*Character entity*/) =>
        {

            print("Update���� ������ �ִ���");
        },
        (/*Character entity*/) =>
        {
            Debug.Log("�ൿ ����");
        }));



        states.Add((int)Enum_CharacterState.Move, new State/*<Character>*/((/*Character entity*/) =>
        {
            //   Debug.Log("�����̱� ����");
            //entity.gameObject.transform.LookAt(new Vector3(entity.InputVec.x, 0, entity.InputVec.z));         
            /*Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
                new Vector3(entity.transform.position.x, 0, entity.transform.position.z);*/
            //  Debug.Log(Vector3.Distance(entity.transform.position, entity.InputVec));
            // Debug.Log($"hit ������ : { entity.InputVec}, dir ������ : {dir.normalized}");
        },
        (/*Character entity*/) =>
        {
            Vector3 dir = InputVec - transform.position;
            /* Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
                 new Vector3(entity.transform.position.x, 0, entity.transform.position.z);*/

            transform.rotation = Quaternion.LookRotation(dir);
            //Debug.Log("�����̴���");
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
