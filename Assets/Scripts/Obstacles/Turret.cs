using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool _active = false;
    private float _timer = 0;

    [SerializeField]
    private float fireRate = 0.01f;

    [SerializeField]
    private float slowFireRate = 2;

    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private float slowSpeed = 10, fastSpeed = 100;

    [SerializeField]
    float maxDistance = 100;


    [SerializeField]
    private Color fastColor = Color.cyan, slowColor = Color.red;

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
                bulletInstance.SlowSpeed = slowSpeed;
                bulletInstance.FastSpeed = fastSpeed;
                bulletInstance.FastColor = fastColor;
                bulletInstance.SlowColor = slowColor;
                bulletInstance.direction = (Player.playerTransform.position - transform.position).normalized;
                _timer = 0;
            }
        }

        GetComponent<Renderer>().material.color = (ComputerStateManager.CurrentState == ComputerState._8bit) ? slowColor : fastColor;
    }
    void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, (Player.playerTransform.position - transform.position).normalized,maxDistance);

        if(hit.collider != null && hit.collider.tag == "Player")
        {
            _active = true;
        }
        else
        {
            _active = false;
        }
    }


}
