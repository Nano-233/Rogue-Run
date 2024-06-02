using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashRefill : MonoBehaviour
{
    
    public UnityEvent<int, GameObject> addDash; //damage and knockback
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if colliding with a player。
        if (collision.CompareTag("Player"))
        {
            //add 1 dash
            addDash.Invoke(1, gameObject);
        }
    }
}
