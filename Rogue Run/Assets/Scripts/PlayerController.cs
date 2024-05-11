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

    private int _maxDash = 1; //does the player have a dash
    private int _dashCount = 1;
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

    //checks if the player can move
    public bool CanMove
    {
        get
        {
            return _animator.GetBool(AnimationStrings.canMove);
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
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        
        //sets the y velocity of the animator to check for rising or falling
        _animator.SetFloat(AnimationStrings.yVelocity, _rb.velocity.y);
        _dashCount = _touchingDirections.IsGrounded ? _maxDash : _dashCount;
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
        if (context.started && _touchingDirections.IsGrounded && CanMove)
        {
            _animator.SetTrigger(AnimationStrings.jumpTrigger);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
        }
        if (context.canceled && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && _dashCount > 0)
        {
            _animator.SetTrigger(AnimationStrings.dashTrigger);
            _dashCount--;
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

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove) //checks if can move
            {
                if (IsMoving && !_touchingDirections.IsOnWall) //if not on a wall and moving
                {
                    return movementSpeed;
                }
                else //if not moving or against wall
                {
                    return 0;
                }
            }
            else //if cannot move
            {
                return 0;
            }
        }
    }
    
}
