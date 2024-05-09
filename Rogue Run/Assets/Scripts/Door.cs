using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner confiner;
    [SerializeField] private Collider2D nextRoom;
    [SerializeField] private Collider2D previousRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if colliding with a player。
        if (collision.CompareTag("Player"))
        {
            //moves camera to room depending on position coming from.
            if (collision.transform.position.x < transform.position.x)
            {
                confiner.m_BoundingShape2D = nextRoom;
            }
            else
            {
                confiner.m_BoundingShape2D = previousRoom;
            }
        }
    }
}
