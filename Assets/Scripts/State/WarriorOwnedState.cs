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
        int speed;
        
        public override void Enter(Warrior entity)
        {

            Debug.Log("움직이기 시작");
            entity.animator.SetBool("Move", true);
        }
        public override void FixedStay(Warrior entity)
        {

            speed = entity.Speed;
            Debug.Log("움직이는중");
            Vector3 nextVec = entity.InputVec; //* entity.Speed * Time.fixedDeltaTime;
            entity.Rigid.velocity = nextVec * entity.Speed; //MovePosition(entity.Rigid.position + nextVec);


            if(entity.InputVec.x == 0f && entity.InputVec.z == 0f)
            {
                entity.ChangeState(Enum_WarriorState.Idle);
            }
        }

        public override void Stay(Warrior entity)
        {
            
        }

        public override void Exit(Warrior entity)
        {
            Debug.Log("가만히 있는다");
        }

    }

    public class SkillAction : State<Warrior>
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

    public class Avoid : State<Warrior>
    {
        private float avoid = 1f;
        public override void Enter(Warrior entity)
        {
            Debug.Log("피한다");          
        }

        public override void Exit(Warrior entity)
        {
            
            Debug.Log($"구르기 끝  {avoid}");
        }

        public override void FixedStay(Warrior entity)
        {
            if (avoid > 0)
            {
                avoid -= Time.deltaTime;  
                Debug.Log("구르는중");
            }
            else if (avoid <= 0)
            {
                entity.ChangeState(entity.WarriorState = Enum_WarriorState.Idle);
                avoid = 1f;
            }
        }

        public override void Stay(Warrior entity)
        {
           
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
