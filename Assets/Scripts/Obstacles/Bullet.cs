using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    
    [SerializeField]
    private ParticleSystem onDeathParticle;

    public Vector2 direction;

    
    
    void Awake()
    {
      Destroy(gameObject, 5);  
    }
    void Update()
    {
        if(direction != Vector2.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }  
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        var particle = Instantiate(onDeathParticle, transform.position, transform.rotation);
        Destroy(particle.gameObject, 1);
    }
}
