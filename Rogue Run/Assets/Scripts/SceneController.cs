using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;

    [SerializeField] private Transform _player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextScene(int id)
    {
        //if increment, add 1, else jump to specified id
        if (id == -1)
        {
            id = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadSceneAsync(id);
        }
        else
        {
            SceneManager.LoadSceneAsync(id);
        }
        
        //sets positions
        switch(id)
        {
            case 0:
                _player.position = new Vector3(-40, 4, 0);
                break;
            case 1:
                _player.position = new Vector3(-100, 4.5f, 0);
                break;
            case 2:
                _player.position = new Vector3(-60, 5.5f, 0);
                break;
            case 3:
                _player.position = new Vector3(-40, 4f, 0);
                break;
        }
    }
}
