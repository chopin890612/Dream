using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);//Prologue
        GameManager.instance.Loading(true);
    }
    public void StartLevel()
    {
        SceneManager.LoadScene(2);//Levels
        GameManager.instance.Loading(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);//Menu
        GameManager.instance.Loading(true);
    }
}
