using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Manage : MonoBehaviour
{
    // bool inGame;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 새로운 씬에 아래 내용을 새로 호출
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*        if (SceneManager.GetActiveScene().name == "InGame")
        {
            inGame = true;
        }*/
        GameManager.UI.SetPopups();    // 팝업들 전부 세팅 TODO: inGame을 인자로 받도록
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 인게임 전까지의 키보드 입력 받음
    public void OnEscape()
    {
        GameManager.UI.Escape();
    }
}
