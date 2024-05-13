using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public int AD = 10;
    public Vector2 knockBack = Vector2.zero;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if can hit
        Damageable damageable = collision.GetComponent<Damageable>(); //can use interface (IDamageable)

        if (damageable != null)
        {
            //hit
            damageable.Hit(AD, knockBack);
        }
        
        
    }
}
