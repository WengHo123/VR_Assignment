using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartNewGame()
    {
        Debug.Log("Start New Game");
        SceneManager.LoadScene(4); // Replace "GameScene" with your actual game scene name
    }

    public void ShowCredits()
    {
        Debug.Log("Credits");
        // Implement your credits logic here
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}

