using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KillType
{
    Bullet,
    Fall,
    Spike,
    Lazer,
    Saw
}

public delegate void OnKillFunc(KillType type, int count);
[RequireComponent(typeof(Rigidbody2D), typeof(ReverseRecorder))]
public class Player : MonoBehaviour
{
    private float _deadTimer = 1;
    private ReverseRecorder _recorder;
    private bool _isGrounded = false;
    
    private Vector3 _startPos;

    private bool _allowWallJump = false;

    private float _wallJumpTimer = 0;
    private Vector2 _wallNormal; 
    private float _slideAmount = 0;
    private LayerMask _heightMask;
    private Rigidbody2D _rigidbody;

    private KillType _lastKill;
    [SerializeField]
    private float maxVelocity = 10;

    [SerializeField]
    private float gravity = 10;

    [SerializeField]
    private float jumpHeight = 20;

    [SerializeField]
    private float yOnDeath = -20;

    
    public bool IsDead{get; private set;}
    public bool IsWallJumping => _allowWallJump;
    public bool IsJumping{get; private set;}

    public bool IsGrounded =>_isGrounded;

    public Vector2 Velocity => _rigidbody.velocity;

    public bool FollowY{get; private set;}

    public int DeathCount{get; private set;}

    public event OnKillFunc AfterRewindEvent;
    
    private static Player _current;

    

    private static Player current
    {
        get
        {
            if(_current == null)
            {
                _current = FindObjectOfType<Player>();
            }

            return _current;
        }
    }

    public static Transform playerTransform => current.transform;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _heightMask = ~(LayerMask.GetMask("Player") | LayerMask.GetMask("Danger"));
        _startPos = transform.position;
        _recorder = GetComponent<ReverseRecorder>();
        _recorder.StartRecording();
        DeathCount = 0;
    }

    void FixedUpdate()
    {
        if(IsDead)
         return;
        var colliders = Physics2D.OverlapBoxAll(transform.position - transform.up * 2.5f 
        , new Vector2(4f, .2f),0 ,_heightMask);
        
        
        if(colliders.Length != 0)
        {
            foreach(var collider in colliders)
            {
                if(!_isGrounded)
                {
                    _isGrounded = true;
                    IsJumping = false;
                    FollowY = false;
                    _slideAmount = _rigidbody.velocity.normalized.x * 2;
                }
            }
        }
        else
        {
            _isGrounded = false;
        }

        var hit = Physics2D.Raycast(transform.position, Vector2.down,500, _heightMask);
        if(hit.distance > 20)
        {
            FollowY = true;
        }
        
    }


    KillType GetKillType(string name)
    {
        //call me yandere-dev from now on 'cuz holy shit this is bad.
        if(name.Contains("bullet"))
        {
            return KillType.Bullet;
        }
        else if(name.Contains("Saw"))
        {
            return KillType.Saw;
        }
        else if(name.Contains("Spike"))
        {
            return KillType.Spike;
        }
        else if(name.Contains("Lazer"))
        {
            return KillType.Lazer;
        }
        return KillType.Fall;
    }
    void Update()
    {
        
        if(IsDead)
        {
            transform.position = _recorder.GetPosition(_deadTimer);
            transform.rotation = _recorder.GetRotation(_deadTimer);
            _deadTimer -= Time.deltaTime /2;
            
            if(_deadTimer <= 0)
            {
                IsDead = false;
                _deadTimer = 1;
                _recorder.StartRecording();
                AfterRewindEvent?.Invoke(_lastKill, DeathCount);
            }
            
        }
        else
        {
            float movement = Input.GetAxis("Horizontal") * maxVelocity;
            if(Input.GetKeyDown(KeyCode.Q))
            {
                ComputerStateManager.CurrentState = (ComputerState)(((int)ComputerStateManager.CurrentState + 1) % 2); 
            }
            if(_isGrounded)
            {

                //ground movement
                _rigidbody.velocity = new Vector2(movement, _rigidbody.velocity.y);

                //jumping
                if(Input.GetButtonDown("Jump"))
                {
                    _rigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                    IsJumping = true;

                }

            }

            if(_allowWallJump)
            {
                if(Input.GetButtonDown("Jump"))
                {
                    _rigidbody.AddForce((Vector2.up + _wallNormal).normalized * jumpHeight, ForceMode2D.Impulse);
                    _allowWallJump = false;
                    _rigidbody.gravityScale *= 10;
                    _wallNormal = Vector2.zero;


                }

                _wallJumpTimer += Time.deltaTime;

                if(_wallJumpTimer > 0.5f)
                {
                    _allowWallJump = false;
                    _rigidbody.gravityScale *= 10;
                    _wallNormal = Vector2.zero;
                } 

            }
            if(transform.position.y < yOnDeath)
            {
                IsDead = true;
                _lastKill = KillType.Fall;
                _recorder.StopRecording();
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(IsDead)
            return;
        if(other.gameObject.tag == "Wall")
        {
            _allowWallJump = true;
            _wallNormal = transform.position - other.transform.position;
            _wallNormal.y = 0;
            _wallNormal = _wallNormal.normalized;
            print(_wallNormal);
            _wallJumpTimer = 0;
            _rigidbody.gravityScale *= 0.1f;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            FollowY = true;
        }
        else if(other.gameObject.tag == "Danger")
        {
            IsDead = true;
            DeathCount ++;
            _lastKill = GetKillType(other.transform.name);
            _recorder.StopRecording();
        }
        else if(other.gameObject.tag == "Floor" && Vector2.Dot(((Vector2)transform.position - other.GetContact(0).point).normalized, Vector2.up) < 0)
        {
           _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(IsDead)
            return;
        if(other.gameObject.tag == "Wall" && _allowWallJump)
        {
            _allowWallJump = false;
            _rigidbody.gravityScale *= 10;
            _wallNormal = Vector2.zero;
                
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position - transform.up * 2.5f, new Vector3(4.5f, .2f,1));

        Gizmos.color = Color.green;
        if(_wallNormal != Vector2.zero)
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_wallNormal * 5);
        }
    }
}
