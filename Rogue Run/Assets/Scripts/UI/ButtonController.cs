using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void PlayButtonPressed()
    {
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
