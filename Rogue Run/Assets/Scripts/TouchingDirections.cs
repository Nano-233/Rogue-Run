using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    
    private CapsuleCollider2D _touchingCol;
    private Animator _animator;

    private RaycastHit2D[] _groundHits = new RaycastHit2D[5];

    
    [SerializeField]
    public bool isGrounded = true;

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        private set
        {
            isGrounded = value;
            _animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    private void Awake()
    {
        _touchingCol = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    } 
    

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = _touchingCol.Cast(Vector2.down, castFilter, _groundHits, groundDistance) > 0;
    }
}
