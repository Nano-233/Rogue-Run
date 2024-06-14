using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    //speeds
    public float movementSpeed = 8f; //speed of player.
    public float jumpImpulse = 10f; //velocity of jump
    private float _dashSpeed = 35f; //current dash speed if should be kept
    
    //inputs
    private Vector2 _moveInput; //gets vector from player input.
    
    //components
    private Rigidbody2D _rb; //Rigidbody2D of player.
    private Animator _animator; //animator of the player sprite
    private Damageable _damageable; //damageable component
    private TrailRenderer _trail; //trail
    private TouchingDirections _touchingDirections; //Used for ground checking
    private PlayerAttack _playerAttack; //player's attack info

    //movement variables
    private int _maxDash = 1; //Number of dashes that can be replenished to
    private int _dashCount = 1; //player's current dash count
    private bool _isMoving = false; //is the player moving
    private float _lastDash = -999f; //time since last dash
    private float _dashCD = 0.3f; //cooldown of dash
    private Vector2 _dashDir; //direction of dash
    public bool isFacingRight = true; //which way the player is facing
    private bool _canDash = true;
    
    //currency
    private int _darknessCount = 0; //Count of currency
    
    //upgrades
    private int _dashUp = 0; //decreased dash cd, in %
    private int _behindUp = 0; //Increased damage to enemies from behind, in %
    private int _decFirstUp = 0; //Decreased first damage taken per room, %
    private int _roomHealUp = 0; //Damage healed per room, flat
    private int _darkUp = 0; //Increased darkness gained, %
    private int _killHealUp = 0; //Damaged healed per kill, flat

    public bool CanDash
    {
        get
        {
            return _canDash;
        }
        set
        {
            _canDash = value;
        }
    }

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
    
    //checks if the player is alive
    public bool IsAlive
    {
        get
        {
            return _animator.GetBool(AnimationStrings.isAlive);
        }
    }
    
    //checks if the player is spawning
    public bool IsSpawning
    {
        get
        {
            return _animator.GetBool(AnimationStrings.isSpawning);
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

    //gets darkness count
    public int DarknessCount
    {
        get
        {
            return _darknessCount;
        }
    }

    private void Awake()
    {
        //gets the rigidbody.
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>(); //gets the animator
        _touchingDirections = GetComponent<TouchingDirections>(); //wall detection
        _damageable = GetComponent<Damageable>(); //damageable component
        _trail = GetComponent<TrailRenderer>();
        _playerAttack = GetComponentInChildren<PlayerAttack>();

        //TODO GET STATS FROM STAT MANAGER
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

        if (!_damageable.IsAlive)
        {
            SceneController.instance.NextScene(0);
        }
        
        float inputX = Input.GetAxisRaw("Horizontal");//horizontal input
        float inputY = Input.GetAxisRaw("Vertical");//vertical input
        
        
        //finds the direction the dash is going to
        _dashDir = new Vector2(inputX, inputY);
        
        //if the player is not moving, set the direction the player is facing as the dash direction.
        if (_dashDir == Vector2.zero)
        {
            _dashDir = new Vector2(transform.localScale.x, 0);
        }

        //if not being hit
        if (!_damageable.IsHit && !IsSpawning)
        {
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
                    _trail.emitting = false; //stop the trail
                    //keeps a portion of the upwards momentum only
                    _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y * 0.35f);
                    //resets the value of stop dash.
                    _animator.SetBool(AnimationStrings.stopDash, false);
                }
                else //change the velocity normally
                {
                    //changes the velocity
                    _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
                }
            }
        }
        else //if got hit
        {
            _trail.emitting = false; //stop the trail
            _animator.SetBool(AnimationStrings.dashing, false);
            _animator.SetBool(AnimationStrings.stopDash, true);
        }
        
        
        //sets the y velocity of the animator to check for rising or falling
        _animator.SetFloat(AnimationStrings.yVelocity, _rb.velocity.y);
        
        //replenish dash if is on the ground and not already dashing
        if (_touchingDirections.IsGrounded && !Dashing)
        {
            _dashCount = _maxDash;
        }

        // if (_rb.position.y < -9.2f)
        // {
        //     _damageable.Hit(100, Vector2.zero);
        //     _rb.velocity = Vector2.zero;
        // }
    }

    //player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        //gets the vector of input
        _moveInput = context.ReadValue<Vector2>();

        if (IsAlive && !IsSpawning) //only move if the player is living and active
        {
            //checks if the player is moving
            IsMoving = _moveInput.x != 0;
            //sets the facing direction
            SetFacingDirection(_moveInput);
        }
        else
        {
            _isMoving = false;
        }
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
        //if has a dash, not being hit and not spawning
        if (context.started && _dashCount > 0 && !_damageable.IsHit && !IsSpawning && CanDash) 
        {
            if (Time.time - _lastDash < _dashCD) //if dash cd not reached yet, cannot dash.
            {
                return;
            }

            _trail.emitting = true;
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

    //calculates the current x move speed
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

    //when the player is hit.
    public void OnHit(int damage, Vector2 knockback)
    {
        //add the knockback
        _rb.velocity = new Vector2(knockback.x, _rb.velocity.y * 0.2f + knockback.y); 
    }
    
    //change dash count
    public void AddDash(int count, GameObject obj)
    {
        //if no overflow
        if (_dashCount < _maxDash)
        {
            //add dash, disable
            _dashCount += count;
            StartCoroutine(DashUsed(obj));
        }
    }

    //disables the refill for a bit
    IEnumerator DashUsed(GameObject obj)
    {
        obj.SetActive(false);
        //Wait for 4 seconds
        yield return new WaitForSeconds(4);
        obj.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //die when touch smth bad, 12 is groundhurt
        if (other.gameObject.layer == 12)
        {
            _damageable.Health = 0;
        }

    }
    
    //saves stats whenever new scene
    public Tuple<int[], float[]> SaveStats()
    {
        int[] intStats = new[] { _maxDash, _damageable.MaxHealth, _playerAttack.AD, _damageable.Health, _darknessCount};
        float[] floatStats = new[] { _dashCD, movementSpeed };
        return Tuple.Create(intStats, floatStats);
    }

    //loads stats whenever new scene
    public void LoadStats(Tuple<int[], float[]> tuple)
    {
        //loads into arrays
        int[] intStats = tuple.Item1;
        float[] floatStats = tuple.Item2;
        
        //saves int info
        _maxDash = intStats[0];
        _damageable.MaxHealth = intStats[1];
        _playerAttack.AD = intStats[2];
        //checks if needs respawning
        if (intStats[3] > 0) //if still alive, keep hp
        {
            _damageable.Health = intStats[3];
        }

        _darknessCount = intStats[4];
        
        //saves float info
        _dashCD = floatStats[0];
        movementSpeed = floatStats[1];
    }

    //adds darkness
    public void AddDarkness(int amount)
    {
        _darknessCount += amount;
    }
    
    //upgrades stats
    public void PermUpgrade(int upgrade)
    {
        switch (upgrade)
        {
            case 0: //reduce dash cd
                _dashUp += 10;
                _dashCD -= 0.03f;
                break;
            case 1: //increase backstab dmg
                _behindUp += 5;
                break;
            case 2: //first damage reduced
                _decFirstUp += 5;
                break;
            case 3: //heal per room
                _roomHealUp += 5;
                break;
            case 4: //increased darkness
                _darkUp += 10;
                break;
            case 5: //kill to heal
                _killHealUp += 2;
                break;
        }
    }
    
    //Check levels of upgrades
    public int GetPermUpgrade(int upgrade)
    {
        switch (upgrade)
        {
            case 0: //reduce dash cd
                return _dashUp;
            case 1: //increase backstab dmg
                return _behindUp;
            case 2: //first damage reduced
                return _decFirstUp;
            case 3: //heal per room
                return _roomHealUp;
            case 4: //increased darkness
                return _darkUp;
            case 5: //kill to heal
                return _killHealUp;
        }
        return 0;
    }
}
