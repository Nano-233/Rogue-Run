using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; //damage and knockback
    
    [SerializeField]
    private int _maxHealth = 100; //max hp
    [SerializeField]
    private int _health = 100; //current hp

    private bool _isInvincible; //checks if character invincible
    private float _timeSinceHit = 0; //time since hit
    public float _invincibilityTime = 0.5f; //invincible after hit

    private bool _isAlive = true;

    private Animator _animator;

    public bool IsHit
    {
        get
        {
            return _animator.GetBool(AnimationStrings.isHit);
        }
        private set
        {
            //sometimes dies in the middle of setting.
            if (IsAlive)
            {
                _animator.SetBool(AnimationStrings.isHit, value);
            }
        }
    }

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }


    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            _animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if is invincible, check to remove the status
        if (_isInvincible)
        {
            if (_timeSinceHit > _invincibilityTime)
            {
                //can be hit
                _isInvincible = false;
                _timeSinceHit = 0;
            }

            _timeSinceHit += Time.deltaTime; //add the time increment.
        }
    }
    
    //checks if the player is hit
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !_isInvincible)
        {
            Health -= damage;
            _isInvincible = true;

            //notify other components damageable was hit and handle knockback.
            IsHit = true;
            damageableHit.Invoke(damage, knockback);
            
            return true;
        }

        return false;
    }

}
