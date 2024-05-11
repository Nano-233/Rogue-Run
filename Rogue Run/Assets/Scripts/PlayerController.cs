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
    private Animator _animator; //animator of the player sprite

    private bool _isMoving = false; //is the player moving
    public bool isFacingRight = true; //which way the player is facing

    //checks if the player is moving
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            //sync with the animator
            _animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }

    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        } private set
        {
            //if facing is wrong
            if (isFacingRight != value)
            {
                //flip the sprite
                transform.localScale *= new Vector2(-1, 1);
            }

            isFacingRight = value;
        }
    }

    private void Awake()
    {
        //gets the rigidbody.
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>(); //gets the animator
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
        //sets the facing direction
        SetFacingDirection(_moveInput);
    }

    //sets the sprite direction
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    
}
