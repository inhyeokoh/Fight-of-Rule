using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager2 : SubMono<GameManager>
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
        maxIdx = SceneManager.sceneCount;
    }


    public void GetPreviousScene()
    {
        curIdx = SceneManager.GetActiveScene().buildIndex;
        if (curIdx > 0)
        {
            SceneManager.LoadScene(--curIdx);
        }
    }

    public void GetNextScene()
    {
        curIdx = SceneManager.GetActiveScene().buildIndex;
        if (curIdx < maxIdx)
        {
            SceneManager.LoadScene(++curIdx);
        }
    }

}
