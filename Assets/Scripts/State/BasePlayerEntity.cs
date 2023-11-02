/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerEntity : SubMono<PlayerController>
{
    //플레이어 닉네임
    private string ninkName;
    //플레이어 고유 번호
    private static int ID;

    private int saveHp = 5;
    private int saveMp = 5;
    private int saveExp = 5;
    private int saveDamage = 5;
    private int saveDefense = 5;
    private int saveSpeed = 5;
    private int saveLevel = 5;


    protected int SaveHp { get { return saveHp; } }
    protected int SaveMp { get { return saveMp; } }
    protected int SaveExp { get { return saveExp; } }
    protected int SaveDamage { get { return saveDamage; } }
    protected int SaveDefense { get { return saveDefense; } }
    protected int SaveSpeed { get { return saveSpeed; } }
    protected int SaveLevel { get { return saveLevel; } }

    public virtual void Setup(string name)
    {      
        ninkName = name;
        ++ID;
    }



    public abstract void FixedUpdated();

    /// <summary>
    /// GameController에 있는 모든 클래스들 업데이트로 구동
    /// </summary>
    public abstract void Updated();

    /// <summary>
    /// 일단 테스트로 그 직업 텍스트 출력
    /// </summary>
    public void Print(string text)
    {
        print($"{name}는 : {text}  ");
    }

}
*/