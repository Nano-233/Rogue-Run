using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu: MonoBehaviour
{

    public void Home()
    {
        Time.timeScale = 1;
        SceneController.instance.PauseTimer(1f);
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneController.instance.SavePref();
        }
        SceneController.instance.ResetPool();
        SceneController.instance.NextScene(0);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().Restart();
    }
}