using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
    private string ninkName;



    public virtual void Setup(string name)
    {
        ninkName = name;
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
        print($"{name}는 : {text}");
    }

}
