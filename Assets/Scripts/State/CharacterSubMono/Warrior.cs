using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WarriorSkill
{
    AssaultBlade = 9,
    DivingSlash,
    DubleSlash,
    SwordStorm,
    WheelWind,
    WarriorAnger,
    Default,
}
public class Warrior : CharacterState
{
    public override void StateAdd()
    {
        base.StateAdd();
        ChangeState((int)Enum_CharacterState.Idle);

    }

/*    public override void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defenes, bool firstLevel)
    {
        base.stats.LevelStatUP(maxEXP, maxHP, maxMP, attack, defenes, firstLevel);
    }*/

    protected override void _Init()
    {
        _board._playerStat.Attack = 700;
        attackCombo = 2;      
        base._Init();
    }

    protected override void _Excute()
    {

    }

    protected override void _Clear()
    {

    }

}
