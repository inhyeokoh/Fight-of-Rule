using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : CharacterState
{


/*    public override void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defenes, bool firstLevel)
    {

    }*/

    public override void StateAdd()
    {

        base.StateAdd();
    }
    protected override void _Init()
    {
        attackCombo = 0;
        base._Init();
    }

    protected override void _Excute()
    {

    }

    protected override void _Clear()
    {

    }
}
