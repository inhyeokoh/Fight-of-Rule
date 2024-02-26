using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Manage : MonoBehaviour
{
    // bool inGame;
    public int preSceneNum;
    public int curSceneNum;

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
        curSceneNum = SceneManager.GetActiveScene().buildIndex;
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
    public void OnInven()
    {
        GameManager.UI.OpenOrClose(GameManager.UI.Inventory);
    }
}
