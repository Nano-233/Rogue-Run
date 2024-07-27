using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int checkpointNo;

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if colliding with a player。
        if (collision.CompareTag("Player"))
        {
            //updates checkpoint
            collision.gameObject.GetComponent<PlayerController>().SetTutorialSpawn(checkpointNo);
        }
    }
}