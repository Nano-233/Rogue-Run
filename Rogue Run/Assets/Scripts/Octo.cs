using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Octo : MonoBehaviour
{

    public float walkSpeed = 3f; //speed that the octo moves at
    public DetectionZone attackZone; //zone of detection for attack
    public DetectionZone foundZone; //zone of detection to chase player
    public bool hasTarget = false;
    public bool foundTarget = false;

    private Rigidbody2D _rb; //rigidbody component of the octo.
    private TouchingDirections _touchingDirections; //using touchigndirections to check walls.
    private Animator _animator; //animator of the octo
    
    public enum WalkableDirection {Right, Left} //direction the octo is moving in

    private WalkableDirection _walkDirection; //direction of walk
    private Vector2 _walkDirectionVector = Vector2.left; //vector of walk MIGHT CHANGE TO BE SERIALIZED

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            //if the direction needs to be flipped.
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y); //changes localscale if wrong

                //sets walking vector
                if (value == WalkableDirection.Right)
                {
                    _walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    _walkDirectionVector = Vector2.left;
                }   
            }
            _walkDirection = value;
        }
    }

    //checks if octo has locked on
    public bool HasTarget
    {
        get
        {
            return hasTarget;
        }
        set
        {
            hasTarget = value;
            _animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    
    //checks if octo has found a target
    public bool FoundTarget
    {
        get
        {
            return foundTarget;
        }
        set
        {
            foundTarget = value;
            _animator.SetBool(AnimationStrings.foundTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return _animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //checks if either a target is locked for attack or if is in range of chase
        HasTarget = attackZone.detectedColliders.Count > 0;
        foundTarget = foundZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        //if is on the floor and collided with a wall, turn around
        if (_touchingDirections.IsOnWall && _touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        
        //makes the octo move
        if (CanMove)
        {
            _rb.velocity = new Vector2(walkSpeed * _walkDirectionVector.x, _rb.velocity.y);
        }
        else
        {
            //if cannot move, make him stop with a damp.
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 0.05f), _rb.velocity.y);
        }
        
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    
    
}
