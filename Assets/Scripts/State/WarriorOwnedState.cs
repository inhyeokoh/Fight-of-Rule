using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarriorOwnedState
{
    public class Idle : State<Warrior>
    {
        public override void Enter(Warrior entity)
        {
            Debug.Log("가만히 있는다");
            entity.animator.SetBool("Move",false);
        }
        public override void FixedStay(Warrior entity)
        {
            Debug.Log("가만히 있는중 FixedStay");
        }

        public override void Stay(Warrior entity)
        {
            Debug.Log("가만히 있는중 Stay");
        }

        public override void Exit(Warrior entity)
        {
            Debug.Log("행동 시작");
        }

    }

    public class Move : State<Warrior>
    {
      
        public override void Enter(Warrior entity)
        {

         //   Debug.Log("움직이기 시작");
            //entity.gameObject.transform.LookAt(new Vector3(entity.InputVec.x, 0, entity.InputVec.z));
            entity.animator.SetBool("Move", true);
            /*Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
                new Vector3(entity.transform.position.x, 0, entity.transform.position.z);*/
          //  Debug.Log(Vector3.Distance(entity.transform.position, entity.InputVec));
           // Debug.Log($"hit 포지션 : { entity.InputVec}, dir 포지션 : {dir.normalized}");
        }
        public override void FixedStay(Warrior entity)
        {

            Vector3 dir = entity.InputVec - entity.transform.position;
            /* Vector3 dir = new Vector3(entity.InputVec.x, 0, entity.InputVec.z) -
                 new Vector3(entity.transform.position.x, 0, entity.transform.position.z);*/
                  
            entity.transform.rotation = Quaternion.LookRotation(dir);
            //Debug.Log("움직이는중");
            // Vector3 nextVec = entity.InputVec; //* entity.Speed * Time.fixedDeltaTime;
            // entity.Rigid.velocity = nextVec * entity.Speed; //MovePosition(entity.Rigid.position + nextVec);

             if (Vector3.Distance(entity.transform.position, entity.InputVec) >= 0.2f)
             {
                entity.Rigid.velocity = dir.normalized * entity.Speed;
                Debug.Log(entity.Rigid.velocity.magnitude);
            }
             else
             {
                entity.Rigid.velocity = Vector3.zero;
                entity.ChangeState(Enum_WarriorState.Idle);
             }


          /*  if(entity.InputVec.x == 0f && entity.InputVec.z == 0f)
            {
                entity.ChangeState(Enum_WarriorState.Idle);
            }*/
        }

        public override void Stay(Warrior entity)
        {
            
        }

        public override void Exit(Warrior entity)
        {
            Debug.Log("가만히 있는다");
        }

    }

    public class Attack : State<Warrior>
    {
        public override void Enter(Warrior entity)
        {
          
        }

        public override void Exit(Warrior entity)
        {
           
        }

        public override void FixedStay(Warrior entity)
        {
           
        }

        public override void Stay(Warrior entity)
        {
           
        }
    }

    public class SkillActionQ : State<Warrior>
    {
        public event Action<Warrior> InputQ = null;
        
        public override void Enter(Warrior entity)
        {
            InputQ?.Invoke(entity);
        }

        public override void Exit(Warrior entity)
        {
            
        }

        public override void FixedStay(Warrior entity)
        {
           
        }

        public override void Stay(Warrior entity)
        {
           
        }   
    }
    public class SkillActionW : State<Warrior>
    {
        public event Action<Warrior> InputW = null;

        public override void Enter(Warrior entity)
        {
            InputW?.Invoke(entity);
        }

        public override void Exit(Warrior entity)
        {

        }

        public override void FixedStay(Warrior entity)
        {

        }

        public override void Stay(Warrior entity)
        {

        }
    }
    public class SkillActionE : State<Warrior>
    {

        public event Action<Warrior> InputE = null;

        public override void Enter(Warrior entity)
        {
            InputE?.Invoke(entity);
        }

        public override void Exit(Warrior entity)
        {

        }

        public override void FixedStay(Warrior entity)
        {

        }

        public override void Stay(Warrior entity)
        {

        }
    }
    public class SkillActionR : State<Warrior>
    {

        public event Action<Warrior> InputR = null;

        public override void Enter(Warrior entity)
        {
            InputR?.Invoke(entity);
        }

        public override void Exit(Warrior entity)
        {

        }

        public override void FixedStay(Warrior entity)
        {

        }

        public override void Stay(Warrior entity)
        {

        }
    }

    public class Avoid : State<Warrior>
    {
        private float avoid;
        int speed;
        Vector3 dir;
        public override void Enter(Warrior entity)
        {
            avoid = 0.3f;
            entity.Speed = 50;
            dir = entity.InputVec - entity.transform.position;
            entity.transform.rotation = Quaternion.LookRotation(dir);
            entity.animator.SetBool("Move",false);
            entity.animator.SetBool("Avoid", true);          
                    
        }

        public override void Exit(Warrior entity)
        {
            entity.Speed = 10;
            
        }

        public override void FixedStay(Warrior entity)
        {
           
        }

        public override void Stay(Warrior entity)
        {
            if (avoid > 0)
            {               
                entity.Rigid.velocity = dir.normalized * entity.Speed;
                avoid -= Time.deltaTime;
                
            }
            else if (avoid <= 0)
            {
                entity.Rigid.velocity = dir.normalized * 0;
                entity.ChangeState(entity.WarriorState = Enum_WarriorState.Idle);            
            }
        }
    }

    public class Hit : State<Warrior>
    {
        public override void Enter(Warrior entity)
        {
            
        }

        public override void Exit(Warrior entity)
        {
            
        }

        public override void FixedStay(Warrior entity)
        {
            
        }

        public override void Stay(Warrior entity)
        {
            
        }
    }
    public class Fall : State<Warrior>
    {
        public override void Enter(Warrior entity)
        {
          
        }

        public override void Exit(Warrior entity)
        {
           
        }

        public override void FixedStay(Warrior entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Stay(Warrior entity)
        {
          
        }
    }

    public class Die : State<Warrior>
    {
        public override void Enter(Warrior entity)
        {
           
        }

        public override void Exit(Warrior entity)
        {
            
        }

        public override void FixedStay(Warrior entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Stay(Warrior entity)
        {
            
        }
    }
}
