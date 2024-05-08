using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room Camera
    [SerializeField] private float speed; //speed of camera
    private float _currentPosX;
    private Vector3 _velocity = Vector3.zero;
    
    //Player camera
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        //room camera
        // transform.position = Vector3.SmoothDamp(transform.position,
        //     new Vector3(_currentPosX, transform.position.y, transform.position.z),
        //     ref _velocity, speed);
        
        //follow player
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }

    //Moves to new room.
    public void MoveToNewRoom(Transform newRoom)
    {
        _currentPosX = newRoom.position.x;
    }
    

}
