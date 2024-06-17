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
    private GameObject _player; //player
    
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
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>(); //finds the player
    }

    //if dashes into enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if can hit
        Damageable damageable = collision.GetComponent<Damageable>(); //can use interface (IDamageable)
        //get bonus dmg striking from behind
        float bonus = _playerController.BehindBuff / 100f + 1;
        
        if (damageable != null && playerAnim.GetBool(AnimationStrings.dashing))
        {
            if (Math.Sign(_player.transform.localScale.x) == Math.Sign(collision.transform.localScale.x) && bonus > 0)
            {
                //hit extra
                damageable.Hit(Convert.ToInt32(AD * bonus), _knockBack);
            }
            else
            {
                //hit
                damageable.Hit(AD, _knockBack);
            }
        }
    }
    
    //if starts from enemy. Invincible takes care of multiple damage.
    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if can hit
        Damageable damageable = collision.GetComponent<Damageable>(); //can use interface (IDamageable)
        //get bonus dmg striking from behind
        float bonus = _playerController.BehindBuff / 100 + 1;

        if (damageable != null && playerAnim.GetBool(AnimationStrings.dashing))
        {
            if (Math.Sign(_player.transform.localScale.x) == Math.Sign(collision.transform.localScale.x) && bonus > 0)
            {
                //hit extra
                damageable.Hit(Convert.ToInt32(AD * bonus), _knockBack);
            }
            else
            {
                //hit
                damageable.Hit(AD, _knockBack);
            }
            
            //if killed, drop loot
            if (!damageable.IsAlive)
            {
                //randomly gain darkness according to multiplier
                int gain = Random.Range(0, damageable.Multiplier);
                if (gain > 0)
                {
                    _playerController.AddDarkness(gain);
                }
                
                //heal if upgrade
                _playerController.KillHeal();
            }
        }
        
    }
}
