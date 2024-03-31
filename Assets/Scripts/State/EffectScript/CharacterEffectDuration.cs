using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectDuration : MonoBehaviour
{
    [SerializeField]
    CharacterStatus characterStat;

    [SerializeField]
    private float activeTime;
 
    [SerializeField]
    private float durationTime;

    private float addforce = 0;

    [SerializeField]
    private bool Isoverlap;

   [SerializeField]
    private float scale;

    [SerializeField]
    List<Collider> enemys;

    [SerializeField]
    bool damageOn;
    [SerializeField]
    bool delayTime;

    private int skillDamage;


    [SerializeField]
    bool firstDamageOn;
    private void Awake()
    {
        //characterState = PlayerController.instance._playerState;
        enemys = new List<Collider>();
        print(GetComponent<BoxCollider>().center);
    }

    private void OnEnable()
    {
        delayTime = false;
        damageOn = true;
        characterStat = PlayerController.instance._playerStat;
        skillDamage = characterStat.SkillDamage;
        Invoke("SetActvieObject", activeTime);
    }

   
    private void Update()
    {
        if (damageOn && enemys.Count > 0)
        {
            {

                for (int i = 0; i < enemys.Count; i++)
                {
                    MonsterStatus monsterState = enemys[i].GetComponent<MonsterStatus>();
                    DamageFactory.instance.MonsterDamage(monsterState, skillDamage, characterStat, addforce);
                }

            }

            damageOn = false;
        }
        else
        {
            if (!delayTime)
            {
                print("작동됌");
                delayTime = true;
                StartCoroutine("DurationDamage", durationTime);
            }
        }
    }   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            enemys.Add(other);
            MonsterStatus monsterState = other.GetComponent<MonsterStatus>();
            DamageFactory.instance.MonsterDamage(monsterState, skillDamage, characterStat, addforce);
            damageOn = false;
            print(other.gameObject);
        }

        else
        {
            if (!delayTime)
            {
                delayTime = true;
                StartCoroutine("DurationDamage", durationTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            enemys.Remove(other);
            print(other.gameObject);
        }

    }

    private void OnDisable()
    {
        enemys.Clear();
    }

    private void SetActvieObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DurationDamage(float durationTime)
    {
        durationTime = this.durationTime;

        while (durationTime >= 0)
        {
            durationTime -= Time.deltaTime;
            yield return null;
        }

        delayTime = false;
        damageOn = true;
    }
}
  
       