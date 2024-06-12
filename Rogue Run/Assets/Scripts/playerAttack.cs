using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    
    public int _AD = 100; //attack damage
    private Vector2 _knockBack = Vector2.zero;  //no knockback from the player
    private PlayerController _playerController; //player controller component
    
    [SerializeField]
    public Animator playerAnim;

    public int AD
    {
        get
        {
            return _AD;
        }
        set
        {
            _AD = value;
        }
    }

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>(); //finds the player
    }

    //if dashes into enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if can hit
        Damageable damageable = collision.GetComponent<Damageable>(); //can use interface (IDamageable)

        if (damageable != null && playerAnim.GetBool(AnimationStrings.dashing))
        {
            //hit
            damageable.Hit(AD, _knockBack);
        }
    }
    
    //if starts from enemy. Invincible takes care of multiple damage.
    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if can hit
        Damageable damageable = collision.GetComponent<Damageable>(); //can use interface (IDamageable)

        if (damageable != null && playerAnim.GetBool(AnimationStrings.dashing))
        {
            //hit
            damageable.Hit(AD, _knockBack);
            //if killed, drop loot
            if (!damageable.IsAlive)
            {
                //randomly gain darkness according to multiplier
                int gain = Convert.ToInt32(Random.Range(0f, 1f)) * damageable.Multiplier;
                _playerController.AddDarkness(gain);
                CharacterEvents.CharacterDropped.Invoke(gameObject, gain);
            }
        }
        
    }
}
