using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UI_Loading : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // �񵿱�� �ҷ���
        op.allowSceneActivation = false; // �ε� ��ġ�� �ڵ����� �Ѿ�� �ʵ��� + �� �ܿ��� ���ҽ����� ����� �ε� �ǵ���

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null; // �ݺ����� ���������� ����Ƽ ������ ������� �Ѱܾ� �� �������� �ö�

            if (op.progress < 0.2f)
            {
                progressBar.fillAmount = op.progress;
            }
            else // ����ũ �ε�
            {
                timer += Time.unscaledDeltaTime / 5f;
                progressBar.fillAmount = Mathf.Lerp(0.2f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

