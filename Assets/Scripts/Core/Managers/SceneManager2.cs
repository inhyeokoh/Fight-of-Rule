using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager2 : SubClass<GameManager>
{
    int curIdx = 0;
    int maxIdx;

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

    public void GetNextScene(int numToSkip = 1)
    {
        curIdx = SceneManager.GetActiveScene().buildIndex;
        if (curIdx < maxIdx - numToSkip)
        {
            SceneManager.LoadScene(curIdx + numToSkip);
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
