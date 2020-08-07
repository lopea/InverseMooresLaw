using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Light _light;

    private float t;
    
    [SerializeField]
    private BulletParticle onDeathParticle;

    public Vector2 direction;

    public float SlowSpeed, FastSpeed;
    
    public Color MainColor = Color.cyan;

    public Color SlowColor, FastColor;


    
    void Awake()
    {
        
      _light = GetComponentInChildren<Light>();
    }

    void Kill()
    {
        Destroy(gameObject);
        var particle = Instantiate(onDeathParticle, transform.position, transform.rotation);
        particle.SlowColor = SlowColor;
        particle.FastColor = FastColor;
        Destroy(particle.gameObject, 1);
    }

    
    
    void Update()
    {
        var color =  (ComputerStateManager.CurrentState == ComputerState._8bit) ? SlowColor : FastColor;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        _light.color  = color;
        
        var speed = (ComputerStateManager.CurrentState == ComputerState._8bit) ? SlowSpeed : FastSpeed;
        if(direction != Vector2.zero)
        {
            var newDirection = Player.playerTransform.position - transform.position;
            transform.Translate(((Vector2)newDirection.normalized * 0.5f + direction) * speed * Time.deltaTime, Space.World);
        }  

        if(t > 5)
        {
            Kill();
        }
         t+= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Kill();
    }
}
