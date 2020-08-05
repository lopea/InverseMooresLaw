using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraZoom : MonoBehaviour
{
    private Camera _camera;
    private bool _zoomIn;
    private float _timer = 0;

    [SerializeField]
    private float startZoom = 10;
    
    [SerializeField]
    private float endZoom = 30;

    [SerializeField]
    private float speed = 2;

    
    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        _timer += Time.deltaTime * speed;
        if(_timer > 1)
        {
            enabled = false;
        }
        _camera.orthographicSize = startZoom + (1- Mathf.Pow(1-_timer,3))*(endZoom - startZoom);
        
    }


}
