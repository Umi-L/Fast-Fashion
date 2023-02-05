using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public void ReturnToMainMenu()
    {
        //unity load main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void RestartLevel()
    {
        //unity load current level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadTutorial()
    {
        //unity load main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }
    
    public void NextLevel()
    {
        //if last level
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1)
        {
            ReturnToMainMenu();
            return;
        }
        
        //unity load next level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
