using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

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

    public Image inv;
    public Sprite invOn, invOff;

    public void Invincibility()
    {
        KirbyController.invincibile = !KirbyController.invincibile;
        inv.sprite = KirbyController.invincibile ? invOn : invOff; 
    }    

    void Start() {
        if(inv != null)
            inv.sprite = KirbyController.invincibile ? invOn : invOff; 
    }
}
