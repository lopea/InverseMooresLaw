using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
public class PlayerCamera : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    GameObject player;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        
    }

    void Update()
    {
        var pos = transform.position;
        pos.x = player.transform.position.x;
        transform.position = pos;
    }
    private void OnDrawGizmos()
    {
        
    }
}
