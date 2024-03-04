using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneManager2 : SubClass<GameManager>
{
    int curIdx = 0;
    int maxIdx;
    GameObject uiManage;

    protected override void _Clear()
    {
    }

    protected override void _Excute()
    {
    }

    protected override void _Init()
    {
        maxIdx = SceneManager.sceneCountInBuildSettings;
        curIdx = SceneManager.GetActiveScene().buildIndex;

        uiManage = GameObject.Find("UI_Manage");
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

    // 이전에 있던 씬으로 이동
    public void GetLocatedScene()
    {
        if (uiManage.GetComponent<UI_Manage>().preSceneNum == 0)
        {
            ExitGame();
        }
        SceneManager.LoadScene(uiManage.GetComponent<UI_Manage>().preSceneNum);
    }

    public void GetNextScene(int numToSkip = 1)
    {
        curIdx = SceneManager.GetActiveScene().buildIndex;
        if (curIdx < maxIdx - numToSkip)
        {
            SceneManager.LoadScene(curIdx + numToSkip);
        }
    }
    public void LoadSceneByAsync()
    {       
        // StartCoroutine()
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
