using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads Level_0
        SceneManager.LoadScene("Level_0"); // Loads Level_0
    }
    
    public void QuitGame()
    {
        print("Quit!!!");
        Application.Quit();
    }
}
