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
    public float movementSpeed = 8f;
    public float jumpImpulse = 15f; //velocity of jump
    private float _dashSpeed = 30f; //current dash speed if should be kept
    
    private Vector2 _moveInput; //gets vector from player input.
    private Rigidbody2D _rb; //Rigidbody2D of player.
    private Animator _animator; //animator of the player sprite

    private int _maxDash = 1; //Number of dashes that can be replenished to
    private int _dashCount = 1; //player's current dash count
    private bool _isMoving = false; //is the player moving
    private float _lastDash = -999f; //time since last dash
    private float _dashCD = 0.3f; //cooldown of dash
    private Vector2 _dashDir; //direction of dash
    
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
    
    //checks if the player is dashing
    public bool Dashing
    {
        get
        {
            return _animator.GetBool(AnimationStrings.dashing);
        }
    }
    
    //checks if the player should stop dashing
    public bool StopDash
    {
        get
        {
            return _animator.GetBool(AnimationStrings.stopDash);
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
        
        float inputX = Input.GetAxisRaw("Horizontal");//horizontal input
        float inputY = Input.GetAxisRaw("Vertical");//vertical input
        
        
        //finds the direction the dash is going to
        _dashDir = new Vector2(inputX, inputY);
        
        //if the player is not moving, set the direction the player is facing as the dash direction.
        if (_dashDir == Vector2.zero)
        {
            _dashDir = new Vector2(transform.localScale.x, 0);
        }

        //if dashing, dash in the direction the player is facing
        if (Dashing) 
        {
            if (Math.Abs(inputY) > 0)
            {
                if (Math.Abs(inputX) + Math.Abs(inputY) > 1)
                {
                    _rb.velocity = _dashDir.normalized * _dashSpeed / 1.2f;
                }
                else
                { 
                    _rb.velocity = _dashDir.normalized * _dashSpeed / 1.5f;
                }
            }
            else
            {
                _rb.velocity = _dashDir.normalized * _dashSpeed;
            }
        }
        else
        {
            if (StopDash) //if the dash should be stopped
            {
                //keeps a portion of the upwards momentum only
                _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y * 0.2f);
                //resets the value of stop dash.
                _animator.SetBool(AnimationStrings.stopDash, false);
            }
            else //change the velocity normally
            {
                //changes the velocity
                _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
            }
        }
        
        //sets the y velocity of the animator to check for rising or falling
        _animator.SetFloat(AnimationStrings.yVelocity, _rb.velocity.y);
        
        //replenish dash if is on the ground
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

    //when player dashes
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && _dashCount > 0) //if has a dash
        {
            if (Time.time - _lastDash < _dashCD) //if dash cd not reached yet, cannot dash.
            {
                return;
            }
            _lastDash = Time.time; //reset the cd
            _animator.SetTrigger(AnimationStrings.dashTrigger);
            _dashCount--; //reduce a dash count
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
