using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;

    //field for player controller
    private PlayerController _playerController;
    
    //field for all data
    private int[] _intStats;
    private float[] _floatStats;
    private int[] _upStats;

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>(); //finds the player
        EndRoom();
        SaveStats(); //save all needed stats

        StartCoroutine(LoadScene(id)); //make sure the second player component is only after scene loads
    }
    
    //load scene
    private IEnumerator LoadScene(int id)
    {
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
        
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        
        //reload player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>(); //finds the player
        LoadStats(); //save all needed stats
    }

    //save the stats locally
    private void SaveStats()
    {
        var data = _playerController.SaveStats();
        _intStats = data.intStats;
        _floatStats = data.floatStats;
        _upStats = data.upStats;

    }

    //load the stats to the player
    private void LoadStats()
    {
        _playerController.LoadStats(_intStats, _floatStats, _upStats);
    }
    
    //action to player
    private void EndRoom()
    {
        //heal if upgrade
        _playerController.RoomHeal();
    }
}
