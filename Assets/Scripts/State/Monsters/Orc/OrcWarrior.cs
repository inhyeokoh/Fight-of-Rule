using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWarrior : OrcBase, IBaseMonstersPart
{

    StateMachine stateMachine;
    public State[] state;
    void Start()
    {
        MonsterController.instance.monsters.Add(this);
        Destroy(gameObject, 2f);
    }


    private void OnDestroy()
    {
        MonsterController.instance.monsters.Remove(this);
    }

    public override void SetUp()
    {
        MonsterController.instance.monsters.Add(this);
        Destroy(gameObject, 2f);
        base.SetUp();


        stateMachine = new StateMachine();


        //stateMachine.Setup(this,);
    }
    public void FixedUpdated()
    {
       // print("움직이는중");
    }

    public void Updated()
    {
      //  print("움직움직이는중");
    }
}
