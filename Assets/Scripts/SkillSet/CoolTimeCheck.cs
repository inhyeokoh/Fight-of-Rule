using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeCheck : MonoBehaviour
{
    private float coolTime;
    private float maxCoolTime;

    
    public Text skillCoolTimeText;
    public Image skillFillAmount;

    public float CoolTime { get { return coolTime; }}
    public float MaxCoolTime { get { return maxCoolTime; } set { maxCoolTime = value; } }


    public void SkillUse()
    {
        coolTime = maxCoolTime;
        print(coolTime); 
        skillFillAmount.fillAmount = 1;
        StartCoroutine(CoolTimeStart());
    }
    public void CoolTimeStop()
    {
        StopCoroutine(CoolTimeStart());
        coolTime = 0;
        maxCoolTime = 0;
        skillFillAmount.fillAmount = 0;
        skillCoolTimeText.text = " ";
    }


    IEnumerator CoolTimeStart()
    {
        while (coolTime > 0)
        {
           
            coolTime -= Time.deltaTime;              
            skillFillAmount.fillAmount = coolTime / maxCoolTime;
            skillCoolTimeText.text = $"{(int)coolTime}";
            yield return null;
        }

        coolTime = 0;
        skillFillAmount.fillAmount = 0;
        skillCoolTimeText.text = " ";
    }
}
