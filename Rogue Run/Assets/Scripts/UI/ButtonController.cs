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
    public GameObject continueButton;
    public GameObject deleteButton;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Seed"))
        {
            continueButton.SetActive(true);
            deleteButton.SetActive(true);
        }
    }

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

    public void ContinuePressed()
    {
        SceneController.instance.Continue(2);
    }

    public void TutorialButtonPressed()
    {
        SceneController.instance.NextScene(1);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        continueButton.SetActive(false);
        deleteButton.SetActive(false);
    }
}