using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectBurst : MonoBehaviour
{
/*    [SerializeField]
    CharacterState characterState;*/
    [SerializeField]
    CharacterStatus characterStat;
    [SerializeField]
    Transform characterTransform;

    [SerializeField]
    public MonsterState monsterState;

    [SerializeField]
    float addforce = 30f;

    [SerializeField]
    int skillDamage;

    
    private void Awake()
    {     
       /* characterState = gameObject.transform.root.GetComponent<CharacterState>();
        characterTransform = gameObject.transform.root.GetComponent<Transform>();*/
    }

    private void OnEnable()
    {
        characterStat = PlayerController.instance._playerStat;
        skillDamage = characterStat.SkillDamage;
        //print(skillDamage);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            CharacterState characterState = other.GetComponent<CharacterState>();
            DamageFactory.instance.CharacterDamage(characterState, skillDamage, monsterState, addforce);
        }

        if (other.gameObject.CompareTag("Monster"))
        {
            MonsterStatus monsterState = other.GetComponent<MonsterStatus>();

            DamageFactory.instance.MonsterDamage(monsterState, skillDamage, characterStat, addforce);
        }
    }  

}
  
       