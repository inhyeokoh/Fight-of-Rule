using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float hp;
    public float mp;
    public Player pm;
    public float playerSpeed;

    public static PlayerStats Inst { get; private set; }
    private void Awake()
    {
        if (null == Inst)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        playerSpeed = 10;

    }
    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        mp = 100;
        pm.moveSpeed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);
        Debug.Log(mp);
    }

    public bool CheckHp => hp <= 100 && hp > 0;
    public bool CheckMp => mp <= 100 && mp > 0;

    public void HpUp(float hpValue)
    {
        if (CheckHp)
            hp += hpValue;
        else
            Debug.Log("full Hp");
    }

    public void HpDown(float hpValue)
    {
        if (CheckHp)
        {
            hp -= hpValue;
        }
        else
            Dead();
    }

    public void Dead()
    {

    }
    public void MpUp(float mpValue)
    {
        if (CheckMp)
            mp += mpValue;
        else
            Debug.Log("full Mp");
    }

    public void SpeedUp(float speedPercent)
    {
        float speed = (100 + speedPercent) * 0.01f;
        playerSpeed *= speed;

        Invoke(nameof(ResetSpeed), 4);
    }

    public void ResetSpeed()
    {
        playerSpeed = 10;
    }
}
