using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightningTrap : MonoBehaviour
{
    [SerializeField] private float activationDelay; //time before trap sets
    [SerializeField] private float activateTime; //time the trap stays active for
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public int AD = 20; //damage dealt

    private bool _triggered; //if the trap has been triggered
    private bool _activated; //if the trap has been set off

    private Damageable _playerDamageable; //player's health component

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!_triggered)
            {
                //if hasn't been triggered, the trap triggers
                StartCoroutine(Activate());
            }

            _playerDamageable = collision.GetComponent<Damageable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerDamageable = null;
    }

    private IEnumerator Activate()
    {
        //activates trap
        _triggered = true;
        //changes the color to red
        _spriteRenderer.color = Color.red;

        //waits for activation
        yield return new WaitForSeconds(activationDelay);
        //changes the color back white
        _spriteRenderer.color = Color.white;
        _activated = true;
        _animator.SetBool(AnimationStrings.activated, true);

        //wait for deactivation
        yield return new WaitForSeconds(activateTime);
        _activated = false;
        _triggered = false;
        _animator.SetBool(AnimationStrings.activated, false);
    }


    private void Update()
    {
        if (_activated && !(_playerDamageable is null))
            _playerDamageable.Hit(AD, Vector2.zero);
    }
}