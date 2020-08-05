using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [SerializeField]
    private float maxVelocity = 10;

    [SerializeField]
    private float gravity = 10;

    [SerializeField]
    private float jumpHeight = 20;

    
    public bool IsDead{get; private set;}
    public Vector2 Velocity => _rigidbody.velocity;

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
                    _slideAmount = _rigidbody.velocity.normalized.x * 2;
                }
            }
        }
        else
        {
            _isGrounded = false;
        }


        
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
            }
            
        }
        else
        {
            float movement = Input.GetAxis("Horizontal") * maxVelocity;

            if(_isGrounded)
            {
                //ground movement
                _rigidbody.velocity = new Vector2(movement, _rigidbody.velocity.y);



                //jumping
                if(Input.GetButtonDown("Jump"))
                {
                    _rigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);


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
        }
        else if(other.gameObject.tag == "Danger")
        {
            IsDead = true;
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
