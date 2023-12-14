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

    // ���ο� ���� �Ʒ� ������ ���� ȣ��
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*        if (SceneManager.GetActiveScene().name == "InGame")
        {
            inGame = true;
        }*/
        GameManager.UI.SetPopups();    // �˾��� ���� ���� TODO: inGame�� ���ڷ� �޵���
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �ΰ��� �������� Ű���� �Է� ����
    public void OnEscape()
    {
        GameManager.UI.Escape();
    }
}
