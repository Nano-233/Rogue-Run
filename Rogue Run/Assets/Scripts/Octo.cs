using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Octo : MonoBehaviour
{

    public float walkSpeed = 3f; //speed that the octo moves at

    private Rigidbody2D _rb; //rigidbody component of the octo.
    private TouchingDirections _touchingDirections; //using touchigndirections to check walls.
    
    public enum WalkableDirection {Right, Left} //direction the octo is moving in

    private WalkableDirection _walkDirection; //direction of walk
    private Vector2 _walkDirectionVector = Vector2.left; //vector of walk

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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        if (_touchingDirections.IsOnWall && _touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        //makes the octo move
        _rb.velocity = new Vector2(walkSpeed * _walkDirectionVector.x, _rb.velocity.y);
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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
