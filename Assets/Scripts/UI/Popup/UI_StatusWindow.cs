using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_StatusWindow : UI_Entity
{
    CharacterStatus player;
    TMP_Text[] status;

    enum Enum_UI_StatusWindow
    {
        Interact,
        Panel,
        Stat,
        Player,
        Close,
    }
    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_StatusWindow);
    }

    private void OnEnable()
    {
        if (PlayerController.instance == null)
        {
            return;
        }
        else
        {
            if (player == null)
            {
                player = PlayerController.instance._playerStat;
            }
        }
    }

    private void Update()
    {
        SetPlayerStat();
    }

    protected override void Init()
    {
        base.Init();

        status = _entities[(int)Enum_UI_StatusWindow.Stat].GetComponentsInChildren<TMP_Text>();

        _entities[(int)Enum_UI_StatusWindow.Close].ClickAction = (PointerEventData data) =>
        {
            gameObject.SetActive(false);
        };
       
    }

    private void SetPlayerStat()
    {
        status[0].text = $"Level: {player.Level}";
        status[1].text = @$"HP: {player.SumMaxHP} /
           / {player.HP}";           
        status[2].text = @$"MP: {player.SumMaxMP} /
           / {player.MP}";
        status[3].text = $"Attack : {player.SumAttack}";
        status[4].text = $"Defense : {player.SumDefense}";
        status[5].text = $"Speed : {player.SumSpeed}";
        status[6].text = $"AttackSpeed : {player.SumAttackSpeed}";
        status[7].text = $"MaxExp : {player.MaxEXP}";
        status[8].text = $"Exp : {player.EXP}";
    }
}

