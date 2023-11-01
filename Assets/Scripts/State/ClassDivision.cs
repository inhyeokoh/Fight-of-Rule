using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//for character info
/*public class Character : SubMono<PlayerControler>
{
    int _hp;
    int _mp;
    int _sp;
    //...
    //중략
    //...

    *//*public void ChangeState(int stateId)
    {
        //생략
    }*//*
}*/



/*public class PlayerControler : MonoBehaviour
{
    //이 방식이 아니면
    AnimationController _animCon;
    Character _character;
    Effector _effector;
    EventHandler _eHanlder;

    //이 방식으로 드래그&드롭 으로 받아와도 됨. 다만 인덱스별로 어떤 컴포넌트가 들어갈지는 따로 정해햐암
    //public List<SubMono<PlayerControler>> _subMonos = new List<SubMono<PlayerControler>>();

    void Start()
    {
        _animCon = GetComponent<AnimationController>();
        _character = GetComponent<Character>();
        _effector = GetComponent<Effector>();
        _eHanlder = GetComponent<EventHandler>();

        _animCon.Mount(this);
        //_character.Mount(this);
        _effector.Mount(this);
        _eHanlder.Mount(this);
    }

    public void EventDistributor(int stateId)
    {
        _effector.InstEffect(stateId);
    }
    public void StateDistributor(int stateId)
    {
       // _animCon.ChangeAnimation(stateId);
      //  _character.ChangeState(stateId);
    }
}*/