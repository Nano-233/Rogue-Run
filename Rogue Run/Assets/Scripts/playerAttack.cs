using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    
    public int AD = 10;
    private Vector2 _knockBack = Vector2.zero;  
    [SerializeField]
    public Animator playerAnim;

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
