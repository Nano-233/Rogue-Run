using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    //field for fade animation
    public Animator fadeAnimator;

    //field for player
    private PlayerController _playerController;
    private GameObject _player;

    //field for all data
    private int[] _intStats;
    private float[] _floatStats;
    private int[] _permUpStats;
    private int[] _tempUpStats;

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

    public void ReloadScene()
    {
        NextScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene(int id)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>(); //finds the player
        EndRoom();
        SaveStats(); //save all needed stats
        //fade out
        fadeAnimator.SetTrigger("FadeOut");
        StartCoroutine(LoadScene(id)); //make sure the second player component is only after scene loads
    }


    //load scene
    private IEnumerator LoadScene(int id)
    {
        //puts player into stasis
        _playerController.Stasis();
        //Wait for 1 seconds for the animation to finish
        yield return new WaitForSeconds(0.8f);

        AsyncOperation asyncLoadLevel;
        // Start loading the scene
        if (id == -1)
        {
            asyncLoadLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1,
                LoadSceneMode.Single);
        }
        else
        {
            asyncLoadLevel = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
        }

        fadeAnimator.SetTrigger("FadeIn");

        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();

        //reload player
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>(); //finds the player
        LoadStats(); //save all needed stats
        StartRoom();
    }

    //save the stats locally
    private void SaveStats()
    {
        var data = _playerController.SaveStats();
        _intStats = data.intStats;
        _floatStats = data.floatStats;
        _permUpStats = data.permUpStats;
        _tempUpStats = data.tempUpStats;
    }

    //load the stats to the player
    private void LoadStats()
    {
        _playerController.LoadStats(_intStats, _floatStats, _permUpStats, _tempUpStats);
    }

    //action to player when ending scene
    private void EndRoom()
    {
        //heal if upgrade
        _playerController.RoomHeal();
    }

    //action to player when new scene
    private void StartRoom()
    {
        //activate vanguard
        _playerController.Vanguard();
    }
}