using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : InGameItemEquipment
{
    //상태패턴을 이용한 장비세팅
    public override void Setting()
    {

        // 현재 플레이어에게 이 장비의 능력치를 전달하기 위한 컴포넌트
        ap = gameObject.GetComponent<InGameItemEquipment>();



        item.Level = 20;

        state = new State(() =>
        {
            player.SumAttack += item.Attack;
        }, () => { print(item.Attack); }, () => { },
        () =>
        {
            player.SumAttack -= item.Attack;
        });
    }

    //현재 정진님이 원하시는 것
    //데이터에 있는 아이템들은 다 묶고 자식 데이터도 하나 만들어서 상속하게 만들고 하자
}
