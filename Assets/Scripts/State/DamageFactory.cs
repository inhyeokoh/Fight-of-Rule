using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFactory : MonoBehaviour
{
    public static DamageFactory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HitCheck(GameObject characterObject)
    {
        if (characterObject.CompareTag("Monster"))
        {
            
        }
    }
    
    public void CharacterDamage(CharacterState character, int skillDamage, MonsterState monster, float addforce)
    {

    }

    public void MonsterDamage(MonsterStatus monster, int skillDamage, CharacterStatus character, float addforce)
    {
        //print("데미지 팩토리 실행됌");
        monster.DeadCheck(skillDamage, character, addforce);
    }
}
