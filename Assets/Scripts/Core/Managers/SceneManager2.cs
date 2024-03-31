#define TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneManager2 : SubClass<GameManager>
{
    int curIdx = 0;
    int maxIdx;
    UI_Manage uiManage;

    protected override void _Clear()
    {
    }

    protected override void _Excute()
    {
    }

    protected override void _Init()
    {
#if TEST
        maxIdx = SceneManager.sceneCountInBuildSettings;
        curIdx = SceneManager.GetActiveScene().buildIndex;
#else
        maxIdx = SceneManager.sceneCountInBuildSettings;
        curIdx = SceneManager.GetActiveScene().buildIndex;

        uiManage = GameObject.Find("UI_Manage").GetComponent<UI_Manage>();
#endif

    }

    public void GetPreviousScene(int numToSkip = 1)
    {
        curIdx = SceneManager.GetActiveScene().buildIndex;
        if (curIdx - numToSkip + 1 > 0)
        {
            SceneManager.LoadScene(--curIdx);
        }
        else
        {
            ExitGame();
        }
    }

    // ������ �ִ� ������ �̵�
    public void GetLocatedScene()
    {
        if (uiManage.scenes.Count > 0)
        {
            if (SceneManager.GetActiveScene().name == "Loading") // �ε��� �ѹ� �� ���������� �̵�
            {
                uiManage.scenes.Pop();
                SceneManager.LoadScene(uiManage.scenes.Pop());
            }
            uiManage.scenes.Pop(); //����� pop�ؼ� ������
            SceneManager.LoadScene(uiManage.scenes.Pop()); //���������� �̵�
        }
        else
        {
            ExitGame();
        }
    }

    public void GetNextScene(int numToSkip = 1)
    {
        curIdx = SceneManager.GetActiveScene().buildIndex;
        if (curIdx < maxIdx - numToSkip)
        {
            SceneManager.LoadScene(curIdx + numToSkip);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }    
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
