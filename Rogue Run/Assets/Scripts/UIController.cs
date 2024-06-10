using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TMP_Text healthText; //text of hp
    public Image healthBar; //green bar of hp
    private Damageable _damageable; //player's damageable component

    private void Awake()
    {
        //gets the player component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //gets damageable component
        _damageable = player.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = _damageable.Health + "/" + _damageable.MaxHealth; //sets the correct text
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = _damageable.Health / 100f; //sets green hp bar
        healthText.text = _damageable.Health + "/" + _damageable.MaxHealth; //sets text
    }
}
