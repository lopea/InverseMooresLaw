using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{

    private float _offset;

    [SerializeField]
    private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        _offset = Random.Range(-10, 10);   
        //GetComponent<Renderer>().material.SetFloat("_Offset", _offset);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, Time.deltaTime * speed * _offset);
        
    }
}
