using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffector : SubMono<PlayerController>
{
    //¡ÔΩ√ ¿Ã∆Â∆Æ
    [SerializeField]
    private GameObject[] effetcBurst;

    //¡ˆº” ¿Ã∆Â∆Æ
    [SerializeField]
    private GameObject[] effectDuration;
 
    [SerializeField]
    private List<Transform> effectTransform;

    //«ˆ¿Á Ω∫≈≥ ¿Ã∆Â∆Æ ¿Œµ¶Ω∫
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
        print(index);
    }

    public void EffectBurstOn(int index)
    {
        effetcBurst[index].SetActive(true);
        print(index);
    }
    public void EffectBurstOff(int index)
    {
        effetcBurst[index].SetActive(false);
    }

    public void EffectDurationInstance(int transformIndex)
    {            
        Instantiate(effectDuration[transformIndex], effectTransform[transformIndex].position, effectTransform[transformIndex].rotation);
    }



    public void EffectBurstStop()
    {
        for (int i = 0; i < effetcBurst.Length; i++)
        {
            effetcBurst[i].SetActive(false);
        }
    }
}
