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
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Monster"))
        {          
            MonsterState monsterState = other.GetComponent<MonsterState>();

            DamageFactory.instance.MonsterDamage(monsterState, skillDamage, characterStat, addforce);
        }
    }  

}
  
       