using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    
    public int AD = 100;
    private Vector2 _knockBack = Vector2.zero;  //no knockback from the player
    
    [SerializeField]
    public Animator playerAnim;

    
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
        }
        
    }
}
