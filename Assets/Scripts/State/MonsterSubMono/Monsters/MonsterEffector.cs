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

    public void EffectBurstOn(int index)
    {
        effetcBurst[index].SetActive(true);
    }
    public void EffectBurstOff(int index)
    {
        effetcBurst[index].SetActive(false);
    }

    public void EffectDurationInstance(int transformIndex)
    {
        GameObject clone = effectDuration[instanceEffect];
        //clone.GetComponent<ObjectMoveDestroy>().monsterState = _board._monsterState;
        print(effectTransform[transformIndex].rotation);
        Instantiate(clone, effectTransform[transformIndex].position, effectTransform[transformIndex].rotation);
        
    }
    public void EffectBurstStop()
    {
        for(int i = 0; i < effetcBurst.Length; i++)
        {
            effetcBurst[i].SetActive(false);
        }
    }
}
