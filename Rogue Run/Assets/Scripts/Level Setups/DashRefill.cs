using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashRefill : MonoBehaviour
{
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if colliding with a player。
        if (collision.CompareTag("Player"))
        {
            if (_source && collision.gameObject.GetComponent<PlayerController>().AddDash(1, gameObject))
            {
                AudioSource.PlayClipAtPoint(_source.clip, gameObject.transform.position, _source.volume);
            }
            //add 1 dash
            
        }
    }
}