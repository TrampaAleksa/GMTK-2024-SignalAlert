using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int _currentId = 0;
    private void Awake()
    {
        _currentId = SceneManager.GetActiveScene().buildIndex;
    }
    public void PlayGame() => SceneManager.LoadScene(1);
    public void NextLevel() => SceneManager.LoadScene(_currentId + 1);
    public void ReloadLevel() => SceneManager.LoadScene(_currentId);

    public void QuitGame()
    {
        print("Quit!!!");
        Application.Quit();
    }
}
