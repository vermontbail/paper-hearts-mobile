using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ToNewScene : MonoBehaviour
{
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if(sceneName == GameManager.GAMEPLAY_SCENE)
        {
            GameManager.gameplayStarting = true;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    public void LoadLevel1()
    {
        PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, 1);
        LoadScene("Gameplay");
    }
}
