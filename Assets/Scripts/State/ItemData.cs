using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public WarriorWeaponItems warriorWeapons;

    // ���� ������ ����
    public class WarriorWeaponItems
    {
        public ItemWeaponData[] warriorWeaponItems;
    }

    void LoadAllSavedData()
    {
        if (GameManager.Data.CheckData("WarriorWeaponData.json"))
        {
            warriorWeapons = JsonUtility.FromJson<WarriorWeaponItems>(GameManager.Data.LoadData("WarriorWeaponData.json"));

            foreach (ItemWeaponData it in warriorWeapons.warriorWeaponItems)
            {
                Debug.Log(it.level);
            }

        }
    }

}
