using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float _elapsedTime;
    private bool _start = false;
    private bool _paused = false;

    public bool TimerStarted
    {
        get { return _start; }
        set { _start = value; }
    }

    public float ElapsedTime
    {
        get { return _elapsedTime; }
        set { _elapsedTime = value; }
    }


    void Update()
    {
        if (_start)
        {
            if (!_paused)
            {
                _elapsedTime += Time.deltaTime;
            }
            TimeSpan display = TimeSpan.FromSeconds(_elapsedTime);
            timerText.text = display.ToString(@"mm\:ss\:ff");
        }
    }

    public void ResetTimer()
    {
        _elapsedTime = 0;
        _start = false;
        timerText.text = "";
    }

    public IEnumerator Pause(float time)
    {
        _paused = true;
        yield return new WaitForSeconds(time);
        _paused = false;
    }
}