using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;

    public void loadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void exitGame()
    {
        Debug.Log("Game was closed");
        Application.Quit();
    }
}
