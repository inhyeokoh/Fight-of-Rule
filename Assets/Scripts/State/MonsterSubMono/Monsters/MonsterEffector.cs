using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffector : SubMono<MonsterController>
{
    [SerializeField]
    private GameObject[] effetcBurst;
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

    public void EffectBurstStop()
    {
        for(int i = 0; i < effetcBurst.Length; i++)
        {
            effetcBurst[i].SetActive(false);
        }
    }
}
