using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BossTouchingDirections))]
public class Demon : MonoBehaviour, IEnemy
{
    public float walkSpeed = 3f; //speed that the octo moves at
    public DetectionZone attackZone; //zone of detection for attack
    public DetectionZone foundZone; //zone of detection to chase player
    public DetectionZone tpFind; //finds where to tp to
    public bool hasTarget = false;
    public bool foundTarget = false;

    private Rigidbody2D _rb; //rigidbody component of the 
    private BossTouchingDirections _touchingDirections; //using touchigndirections to check walls.
    private SpriteRenderer _spriteRenderer;
    private Animator _animator; //animator of the 
    private Damageable _damageable; //damageable obj of 
    private BossHealthBar _healthBar; //healthbar of 

    private bool _hasLOS; //checks if the player is in line of sight
    private Vector3 _playerPos; //the target player position
    private bool _firstDeath;

    public bool CanTP;





    //checks if octo has locked on
    public bool HasTarget
    {
        get { return hasTarget; }
        set
        {
            hasTarget = value;
            _animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }


    //checks if octo has found a target
    public bool FoundTarget
    {
        get { return foundTarget; }
        set
        {
            foundTarget = value;
            _animator.SetBool(AnimationStrings.foundTarget, value);
        }
    }


    public bool CanMove
    {
        get { return _animator.GetBool(AnimationStrings.canMove); }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<BossTouchingDirections>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _healthBar = GetComponentInChildren<BossHealthBar>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //setups damageable
        _damageable.InvincibleTime = 2f;
        _damageable.Multiplier = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_damageable.IsAlive && !_firstDeath)
        {
            _firstDeath = !_firstDeath;
            SceneController.instance.NextScene(-1);
        }

        //checks if either a target is locked for attack or if is in range of chase
        HasTarget = attackZone.detectedColliders.Count > 0;
        FoundTarget = foundZone.detectedColliders.Count > 0;

        _playerPos = foundZone.playerPos;
    }

    private void FixedUpdate()
    {


        //makes the octo move
        if (CanMove)
        {
            // TODO: check if anything above, if not and detected in LOS, jump up.
            if (foundTarget)
            {
                ChaseFound();
            }
            else
            {
                _animator.SetTrigger(AnimationStrings.teleport);
            }


            if (!_touchingDirections.IsOnWall) //if walking normally
            {
                _rb.velocity = new Vector2(walkSpeed, _rb.velocity.y);
            }
            else //if is on a wall, stop but keep the y momentum
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }

            if (_rb.velocity.y > 0 && _playerPos.y + 2f < _rb.position.y) //if already over the wall, stop y momentum
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.2f);
            }
        }
        else
        {
            //if cannot move, make him stop with a damp.
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 0.05f), _rb.velocity.y);
            
            
            if (CanTP)
            {
                CanTP = false;
                if (tpFind.playerPos.y < -11)
                {
                    transform.position = new Vector2(transform.position.x, -14);
                }
                else if (tpFind.playerPos.y < 4)
                {
                    transform.position = new Vector2(transform.position.x, -1);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x, 14);
                }
            }
        }
    }

    //sets the correct direction for chasing
    private void ChaseFound()
    {
        //goes to the direction of the player
        if ((_playerPos.x > _rb.position.x && transform.localScale.x > 0) ||
            (_playerPos.x < _rb.position.x && transform.localScale.x < 0))
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        walkSpeed *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
    }

    //applies vigilant debuff onto enemy
    public IEnumerator ApplyGraviton(int seconds)
    {
        walkSpeed -= 0.5f * walkSpeed;
        //Wait for x seconds
        yield return new WaitForSeconds(seconds);
        walkSpeed *= 2;
    }


    //when hit.
    public void OnHit(int damage, Vector2 knockback)
    {
        //apply the healthbar update
        _healthBar.UpdateHp(_damageable.Health, _damageable.MaxHealth);
        StartCoroutine(Invincible(2));
    }
    
    //applies vigilant debuff onto enemy
    private IEnumerator Invincible(int seconds)
    {
        _spriteRenderer.material.color = new Color(1, 1, 1, 0.1f);
        yield return new WaitForSeconds(seconds);
        _spriteRenderer.material.color = new Color(1, 1, 1, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set hp bar
        _healthBar.UpdateHp(_damageable.Health, _damageable.MaxHealth);
    }
    
}