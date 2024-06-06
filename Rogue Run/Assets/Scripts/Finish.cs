using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{

    [SerializeField] public int NextSceneId = -1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            SceneController.instance.NextScene(NextSceneId);
            other.gameObject.SetActive(true);
        }
    }
}
