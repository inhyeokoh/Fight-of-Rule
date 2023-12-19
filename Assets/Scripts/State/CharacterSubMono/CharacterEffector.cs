using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffector : SubMono<PlayerController>
{
    [SerializeField]
    private GameObject[] effetcBurst;
    [SerializeField]
    private GameObject[] effectDuration;
    [SerializeField]
    private Transform effectMiddle;

    [SerializeField]
    private Transform[] effectsPosition;
    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {

    }

    public void EffectDurationOn(int index)
    {
        effectDuration[index].SetActive(true);
    }

    public void EffectBurstOn(int index)
    {
        effetcBurst[index].SetActive(true);
    }
    public void EffectBurstOff(int index)
    {
        effetcBurst[index].SetActive(false);
    }

    public void EffectDurationInstance(int effectIndex)
    {      
        GameObject clone = effectDuration[effectIndex];
       /* print(positionIndex);
        Instantiate(effectDuration[effectIndex], effectsPosition[positionIndex].position, effectsPosition[positionIndex].rotation);*/
    }

   

    public void EffectBurstStop()
    {
        for (int i = 0; i < effetcBurst.Length; i++)
        {
            effetcBurst[i].SetActive(false);
        }
    }
}
