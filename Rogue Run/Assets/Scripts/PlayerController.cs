using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //walking speed of player
    public float movementSpeed = 5f;
    
    private Vector2 _moveInput; //gets vector from player input.
    private Rigidbody2D _rb; //Rigidbody2D of player.
    
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        //gets the rigidbody.
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //changes the velocity
        _rb.velocity = new Vector2(_moveInput.x * movementSpeed, _rb.velocity.y);
    }

    //player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        //gets the vector of input
        _moveInput = context.ReadValue<Vector2>();
        //checks if the player is moving
        IsMoving = _moveInput != Vector2.zero;
    }
    
    
}
