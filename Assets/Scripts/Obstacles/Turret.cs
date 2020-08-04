using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool _active = false;
    private float _timer = 0;

    [SerializeField]
    private float fireRate = 1;

    [SerializeField]
    private Bullet bullet;


    float GetAngle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if(_active)
        {
            transform.rotation = Quaternion.Euler(0,0,GetAngle( (Player.playerTransform.position - transform.position)));
            
            _timer += Time.deltaTime;

            if(_timer >= fireRate)
            {
                var bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
                bulletInstance.direction = (Player.playerTransform.position - transform.position).normalized;
                _timer = 0;
            }
        }
    }
    void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, (Player.playerTransform.position - transform.position).normalized);

        if(hit.collider != null && hit.collider.tag == "Player")
        {
            _active = true;
            print ("Hit!");
        }
        else
        {
            _active = false;
        }
    }


}
