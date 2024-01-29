using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEventHandler : SubMono<MonsterController>
{
    //Event를 통해 callback함수 대부분 애니메이션 이벤트에다 넣기 위해 만든 클래스
    public void EffectBurstOnEvent(int index)
    {
        _board.DistributeEffectBurstOn(index);
    }

    public void EffectBurstOffEvent(int index)
    {
        _board.DistributeEffectBurstOff(index);
    }

    public void OnChangeStateEvent(int stateId)
    {
        _board.DistributeState(stateId);
    }

    public void EffectDurationInstanceEvent(int index)
    {
        _board.DistributeEffectInstace(index);
    }

    public void OnRootMotion()
    {
        _board.DistributeRootMotion();
    }

    protected override void _Init()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Clear()
    {

    }
}
