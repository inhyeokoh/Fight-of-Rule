using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인게임 전 키보드 입력 받음
public class UI_KeyInput : MonoBehaviour
{
    public void OnEscape()
    {
        GameManager.UI.Escape();
    }
}
