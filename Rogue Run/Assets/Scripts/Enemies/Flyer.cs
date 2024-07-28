using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Flyer : MonoBehaviour, IEnemy
{
    public Transform target; //player

    public float speed = 20f;
    public float nextWaypointDistance = 3f;

    private float _timeSinceSafe = 0f;
    private float _safeCooldown = 2f;
    private bool _isSafe = false;

    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;

    private Seeker _seeker;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    private EnemyHealthBar _healthBar;

    //checks if octo has found a target
    public bool IsSafe
    {
        get { return _isSafe; }
        set
        {
            _isSafe = value;
            _animator.SetBool(AnimationStrings.isSafe, value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _healthBar = GetComponentInChildren<EnemyHealthBar>();
        //set hp bar
        _healthBar.UpdateHp(_damageable.Health, _damageable.MaxHealth);

        //setups damageable
        _damageable.InvincibleTime = 0.1f;
        _damageable.Multiplier = 100;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone())
            //initial enemy pos, target pos, function calculating path in bg
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p) // check if any errors in calculating path
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeSinceSafe > _safeCooldown)
        {
            //can be hit
            IsSafe = !IsSafe;
            _timeSinceSafe = 0;
            speed = IsSafe ? speed / 2 : speed * 2;
        }

        _timeSinceSafe += Time.deltaTime; //add the time increment.

        if (_path == null)
            return;

        if (_currentWaypoint >=
            _path.vectorPath
                .Count) //check if we are at a greater waypoint than the path has -> means we have reached the end
        {
            _reachedEndOfPath = true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        _rb.AddForce(force);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance) //if we are close to the next waypoint, proceed on to the next
        {
            _currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    //applies vigilant debuff onto enemy
    public IEnumerator ApplyGraviton(int seconds)
    {
        speed -= 0.5f * speed;
        //Wait for x seconds
        yield return new WaitForSeconds(seconds);
        speed *= 2;
    }


    //when hit.
    public void OnHit(int damage, Vector2 knockback)
    {
        //add the knockback
        _rb.velocity = new Vector2(knockback.x, _rb.velocity.y * 0.2f + knockback.y);
        //apply the healthbar update
        _healthBar.UpdateHp(_damageable.Health, _damageable.MaxHealth);
    }
}