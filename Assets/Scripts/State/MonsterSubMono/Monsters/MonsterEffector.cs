using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffector : SubMono<MonsterController>
{
    [SerializeField]
    private GameObject[] effetcBurst;

    [SerializeField]
    private GameObject[] effectDuration;

    [SerializeField]
    private List<Transform> effectTransform;

    private int instanceEffect;

    public int InstanceEffect { get { return instanceEffect; } set { instanceEffect = value; } }
    protected override void _Clear()
    {
       
    }

    protected override void _Excute()
    {
       
    }

    protected override void _Init()
    {
       
    }

    //바로 나가는 이펙트들 이벤트로 체크
    public void EffectBurstOn(int index)
    {
        effetcBurst[index].SetActive(true);
    }

    //일정 시간에 이펙트를 지우기 위한 메서드
    public void EffectBurstOff(int index)
    {
        effetcBurst[index].SetActive(false);
    }


    // 현재 지속되야하는 이펙트일때
    public void EffectDurationInstance(int transformIndex)
    {
        GameObject clone = effectDuration[instanceEffect];
        //clone.GetComponent<ObjectMoveDestroy>().monsterState = _board._monsterState;
        print(effectTransform[transformIndex].rotation);
        Instantiate(clone, effectTransform[transformIndex].position, effectTransform[transformIndex].rotation);
        
    }

    // 몬스터가 도중에 맞으면 스킬을 지워야하는 메서드
    public void EffectBurstStop()
    {
        for(int i = 0; i < effetcBurst.Length; i++)
        {
            effetcBurst[i].SetActive(false);
        }
    }
}
