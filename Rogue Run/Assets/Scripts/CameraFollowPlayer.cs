using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        // Get the Cinemachine Virtual Camera component
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Find the player object by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the player object is found
        if (player != null)
        {
            // Assign the player object to the Follow field of the Cinemachine Virtual Camera
            _virtualCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found. Ensure the player has the correct tag.");
        }
    }
}
