using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    //speed of player
    public float movementSpeed = 5f;
    public float jumpImpulse = 6f;
    
    private Vector2 _moveInput; //gets vector from player input.
    private Rigidbody2D _rb; //Rigidbody2D of player.
    private Animator _animator; //animator of the player sprite

    private bool _isMoving = false; //is the player moving
    public bool isFacingRight = true; //which way the player is facing

    private TouchingDirections _touchingDirections; //Used for ground checking

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
            _animator.SetBool(AnimationStrings.isMoving, value);
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
        _touchingDirections = GetComponent<TouchingDirections>();
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
        
        //sets the y velocity of the animator to check for rising or falling
        _animator.SetFloat(AnimationStrings.yVelocity, _rb.velocity.y);
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

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO: check if alive
        if (context.started && _touchingDirections.IsGrounded)
        {
            _animator.SetTrigger(AnimationStrings.jump);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
        }
        if (context.canceled && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
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
