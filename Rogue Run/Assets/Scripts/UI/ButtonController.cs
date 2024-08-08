using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ButtonController : MonoBehaviour
{
    public void PlayButtonPressed()
    {
        if (GetComponentInChildren<TMP_InputField>().text != "")
        {
            SceneController.instance.SetSeed(Int32.Parse(GetComponentInChildren<TMP_InputField>().text));
        }
        else
        {
            //randomizes the seed
            SceneController.instance.SetSeed(Random.Range(0, 99999));
        }

        SceneController.instance.NextScene(2);
    }

    public void TutorialButtonPressed()
    {
        SceneController.instance.NextScene(1);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}