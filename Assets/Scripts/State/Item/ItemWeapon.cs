using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : InGameItemEquipment
{
    //���������� �̿��� �����
    public override void Setting()
    {

        // ���� �÷��̾�� �� ����� �ɷ�ġ�� �����ϱ� ���� ������Ʈ
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

    //���� �������� ���Ͻô� ��
    //�����Ϳ� �ִ� �����۵��� �� ���� �ڽ� �����͵� �ϳ� ���� ����ϰ� ����� ����
}
