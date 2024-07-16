using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ExploderAttack : MonoBehaviour
{
    public int AD = 50;
    public Vector2 knockBack = Vector2.zero;
    private Damageable _selfDamageable;

    private void Awake()
    {
        //gets the exploder script
        _selfDamageable = GetComponentInParent<Damageable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if can hit
        Damageable damageable = collision.GetComponent<Damageable>(); //can use interface (IDamageable)

        if (damageable != null)
        {
            //starts the fuse
            StartCoroutine(StartFuse(damageable));
        }
    }

    //starts the exploding fus
    private IEnumerator StartFuse(Damageable damageable)
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(1.7f);

        //explodes
        Vector2 deliveredKnockBack =
            damageable.transform.position.x > transform.parent.position.x
                ? knockBack
                : new Vector2(-knockBack.x, knockBack.y);


        if (transform.parent.GetComponent<Damageable>().IsAlive)
        {
            //hit if still alive
            damageable.Hit(AD, deliveredKnockBack);

            //destroys
            Destroy(transform.parent.gameObject);
        }
    }
}