using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool Invincibility = false;

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_Scene");
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public void ButtonOn()
    {
        Invincibility = true;
        Debug.Log("Invincibility: On");
    }

    public void ButtonOff()
    {
        Invincibility = false;
        Debug.Log("Invincibility: Off");
    }
    
}
