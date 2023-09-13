using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarriorOwnedState
{
    public class Idle : State<Warrior>
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

    public class Move : State<Warrior>
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
