using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    [SerializeField] private TMP_Text seed;
    private int _seed;

    public Timer timer;
    public TMP_Text[] permText;
    public TMP_Text[] tempText;
    public GameObject displayPanel;

    public static SceneController instance;

    //field for fade animation
    public Animator fadeAnimator;

    //field for player
    private PlayerController _playerController;
    private GameObject _player;

    //field for all data
    private int[] _intStats;
    private float[] _floatStats;
    private int[] _permUpStats = new int[6];
    private int[] _tempUpStats = new int[12];

    //scene numbers
    private int _lastBoss = 21;
    private int _firstBoss = 11;
    private int _secondBoss = 21;

    //pool of rooms to draw from
    private List<int> _chapter1 = new List<int> { 3, 5, 7, 9 };
    private List<int> _chapter2 = new List<int> { 13, 15, 17, 19 };

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
    
    private void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            UpdateUpgradeDisplay();
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
    
    public void Continue(int id)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>(); //finds the player
        EndRoom();
        SaveStats(); //save all needed stats
        LoadPref();
        //fade out
        fadeAnimator.SetTrigger("FadeOut");
        StartCoroutine(LoadScene(id)); //make sure the second player component is only after scene loads
    }

    //resets pool of levels
    public void ResetPool()
    {
        _chapter1 = new List<int> { 3, 5, 7, 9 };
        _chapter2 = new List<int> { 13, 15, 17, 19 };
    }


    //load scene
    private IEnumerator LoadScene(int id)
    {
        //checks current scene and saves
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        //if entering final room, stop timer
        if (currentIndex == _lastBoss)
        {
            timer.TimerStarted = false;
        }

        //puts player into stasis
        _playerController.Stasis();
        //Wait for 1 seconds for the animation to finish
        yield return new WaitForSeconds(0.8f);

        AsyncOperation asyncLoadLevel;
        // Start loading the scene

        if (id == -1) //next scene id
        {
            asyncLoadLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1,
                LoadSceneMode.Single);
        }
        else if (id == -2) //pool next level in 1-
        {
            asyncLoadLevel = SceneManager.LoadSceneAsync(GetNextRoom(1), LoadSceneMode.Single);
        }
        else if (id == -3) //pool next level in 2-
        {
            asyncLoadLevel = SceneManager.LoadSceneAsync(GetNextRoom(2), LoadSceneMode.Single);
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
        yield return new WaitForEndOfFrame();
        
        //reload player
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>(); //finds the player
        
        //if restarted
        if (id == 0)
        {
            timer.ResetTimer();
            seed.text = "";
        }
        else
        {
            LoadStats(); //save all needed stats
        }
        StartRoom();
        
        
        //start speedrun timer if starts
        if (currentIndex == 0 && id != 1 && !timer.TimerStarted)
        {
            timer.TimerStarted = true;
            seed.text = "Seed: " + _seed;
        }
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
    

    //gets the next room number by randomizing
    private int GetNextRoom(int index)
    {
        //pool from chapter 1
        if (index == 1)
        {
            //load boss fight
            if (_chapter1.Count == 0)
            {
                return _firstBoss;
            }

            //gets index, return scene number, remove from pool
            int sceneNum = Random.Range(0, _chapter1.Count);
            int scene = _chapter1[sceneNum];
            _chapter1.RemoveAt(sceneNum);
            return scene;
        }

        //pool from chapter 2
        if (index == 2)
        {
            //load boss fight
            if (_chapter2.Count == 0)
            {
                return _secondBoss;
            }

            //gets index, return scene number, remove from pool
            int sceneNum = Random.Range(0, _chapter2.Count);
            int scene = _chapter2[sceneNum];
            _chapter2.RemoveAt(sceneNum);
            return scene;
        }

        return 0;
    }

    public void SetSeed(int seed)
    {
        Random.InitState(seed);
        _seed = seed;
    }
    
    public void UpdateUpgradeDisplay()
    {
        if (!displayPanel.activeSelf)
        {
            if (SceneManager.GetActiveScene().buildIndex % 2 != 0 && SceneManager.GetActiveScene().buildIndex > 1)
            {
                displayPanel.SetActive(true);
                //sets the correct text
                for (int i = 0; i < permText.Length; i++)
                {
                    permText[i].text = permText[i].text.Remove(permText[i].text.Length - 1);
                    permText[i].text += _permUpStats[i] / UpgradeInts.permArr[i];
                }

                for (int i = 0; i < _tempUpStats.Length; i++)
                {
                    tempText[i].text = tempText[i].text.Remove(tempText[i].text.Length - 1);
                    tempText[i].text += _tempUpStats[i] / UpgradeInts.tempArr12[i];
                }

            }
        }
        else
        {
            displayPanel.SetActive(false);
        }
        
    }

    public void LoadPref()
    {
        for (int i = 0; i < _permUpStats.Length; i++)
        {
            _permUpStats[i] = PlayerPrefs.GetInt("perm_" + i);
        }
        for (int i = 0; i < _tempUpStats.Length; i++)
        {
            _tempUpStats[i] = PlayerPrefs.GetInt("temp_" + i);
        }
        timer.ElapsedTime = PlayerPrefs.GetFloat("Timer");
        SetSeed(PlayerPrefs.GetInt("Seed"));
        _intStats[4] = PlayerPrefs.GetInt("Money");
        _playerController.DarknessCount = PlayerPrefs.GetInt("Money");

    }

    public void SavePref()
    {
        for (int i = 0; i < _permUpStats.Length; i++)
        {
            PlayerPrefs.SetInt("perm_" + i, _permUpStats[i]);
        }
        for (int i = 0; i < _tempUpStats.Length; i++)
        {
            PlayerPrefs.SetInt("temp_" + i, _tempUpStats[i]);
        }
        PlayerPrefs.SetFloat("Timer", timer.ElapsedTime);
        PlayerPrefs.SetInt("Seed", _seed);
        PlayerPrefs.SetInt("Money", _playerController.DarknessCount);
    }

    public void PauseTimer(float time)
    {
        StartCoroutine(timer.Pause(time));
    }
}