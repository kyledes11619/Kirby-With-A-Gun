using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = pauseMenu.activeSelf ? 1f : 0f;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}