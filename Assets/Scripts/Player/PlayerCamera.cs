using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
public class PlayerCamera : MonoBehaviour
{
    
    ReverseRecorder _recorder;

    float _recorderT = 1;


    Camera _camera;

    [SerializeField]
    Player player;

    
    [SerializeField]
    float maxYDifference = 10;

    [SerializeField]
    float minYValue = -.2f;

    [SerializeField]
    float maxYValue = 100;

    [SerializeField]
    float minXValue = -.2f;

    [SerializeField]
    float maxXValue = 100;
    
    float yOffset;

    GlitchShaderScript _glitch;


    void Awake()
    {
        _recorderT = 1;
        _camera = GetComponent<Camera>();
        _glitch = GetComponent<GlitchShaderScript>();
        _recorder = GetComponent<ReverseRecorder>();
        if(_recorder == null)
        {
            _recorder = gameObject.AddComponent<ReverseRecorder>();
        }

        _recorder.StartRecording();
        yOffset = Mathf.Abs(player.transform.position.y - transform.position.y);
    }

    void Update()
    {
        _glitch.enabled = player.IsDead;

        if(player.IsDead)
        {
            if(_recorder.isRecording)
                _recorder.StopRecording();

            transform.position = _recorder.GetPosition(_recorderT);
            transform.rotation = _recorder.GetRotation(_recorderT);

            _recorderT  -= Time.deltaTime /2f;

        }
        else
        {
            var pos = transform.position;
            var velocity = player.Velocity;
            
            if(!_recorder.isRecording)
            {
                _recorder.StartRecording();
                _recorderT = 1;
            }

            if((transform.position.y < minYValue && velocity.y < 0) &&
               (transform.position.y > maxYValue && velocity.y > 0))
               velocity.y =0;
            
            if((transform.position.x < minXValue && velocity.x < 0) &&   
               (transform.position.x > maxXValue && velocity.x > 0))
               velocity.x =0;

            pos += (Vector3)velocity * Time.deltaTime;  

            _glitch.enabled =  ComputerStateManager.CurrentState == ComputerState._8bit && Mathf.PerlinNoise(Time.time, Time.time) > 0.5f;

            transform.position = pos;

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
    }
    
    
}
