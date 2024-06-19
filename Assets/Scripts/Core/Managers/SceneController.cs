//#define SERVER
#define CLIENT_TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance = null;
    int curSceneIdx;

    public enum Enum_Scenes
    {
        Title,
        Select,
        Create,
        StatePattern,
        Loading    
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        curSceneIdx = SceneManager.GetActiveScene().buildIndex;

#if SERVER
        GameManager.UI.SetGamePopups(UIManager.Enum_PopupSetJunction.Title);
        if (scene.name == "StatePattern")
        {
            GameManager.UI.SetGamePopups(UIManager.Enum_PopupSetJunction.StatePattern);
            GameManager.UI.ConnectPlayerInput();
            GameManager.Inven.ConnectInven();
        }
#elif CLIENT_TEST
#endif
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 이전에 있던 씬으로 이동
    public void LoadPreviousScene()
    {
        if (curSceneIdx == (int)Enum_Scenes.Title)
        {
            ExitGame();
        }
        else if (curSceneIdx <= (int)Enum_Scenes.Create)
        {
            SceneManager.LoadScene(--curSceneIdx);
        }
        else if (curSceneIdx == (int)Enum_Scenes.StatePattern)
        {
            SceneManager.LoadScene((int)Enum_Scenes.Title);
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
