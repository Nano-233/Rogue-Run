using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] private float speed; //speed of camera
    private float _currentPosX;
    private Vector3 _velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(_currentPosX, transform.position.y, transform.position.z),
            ref _velocity, speed);
    }

    //Moves to new room.
    public void MoveToNewRoom(Transform newRoom)
    {
        _currentPosX = newRoom.position.x;
    }
    

}
