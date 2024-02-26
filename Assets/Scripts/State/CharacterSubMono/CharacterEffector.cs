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
    private List<Transform> effectTransform;

    private int instanceEffect;

    private bool isSkillInstance;

    public int InstanceEffect { get { return instanceEffect; } set { instanceEffect = value; } }

    public bool IsSkillInstance { get { return isSkillInstance; } set { isSkillInstance = value; } }

    /*[SerializeField]

    [SerializeField]
    private Transform[] effectsPosition;*/
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

    public void EffectDurationInstance(int transformIndex)
    {            
        Instantiate(effectDuration[instanceEffect], effectTransform[transformIndex].position, effectTransform[transformIndex].rotation);
    }



    public void EffectBurstStop()
    {
        for (int i = 0; i < effetcBurst.Length; i++)
        {
            effetcBurst[i].SetActive(false);
        }
    }
}
