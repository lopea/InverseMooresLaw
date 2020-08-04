using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ReverseSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
}

public class ReverseRecorder : MonoBehaviour
{
    private List<ReverseSnapshot> _snapshots;
    private ReverseSnapshot _last;

    private float _timer = 0;
    public bool isRecording {get; private set;}
    

    public void StartRecording()
    {
        if(isRecording)
            return;

        _snapshots = new List<ReverseSnapshot>();
        isRecording = true;
        AddSnapshot();
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    void AddSnapshot()
    {
        var newSnapshot = new ReverseSnapshot{position = transform.position, rotation = transform.rotation};
        _last = newSnapshot;
        _snapshots.Add(newSnapshot);


    }
    void Update()
    {
        if(isRecording)
        {
            _timer += Time.deltaTime;

            if(_timer > 0.1f)
            {
                AddSnapshot();
                _timer = 0;
            }


        }
    }

    public Vector3 GetPosition(float position)
    {
        if(isRecording)
        {
            Debug.LogWarning("Cannot get position while RewindRecorder is still recording!");
            return transform.position;
        }
        if(_snapshots == null || _snapshots.Count == 0)
        {
            Debug.LogWarning("RewindRecorder has no data recorded! Please start recording before geting a position!");
            return  transform.position;
        }
        if(position == 1)
            return _snapshots[_snapshots.Count - 1].position;
        
        if(position == 0)
            return _snapshots[0].position;

        int lowerIndex = (int)((_snapshots.Count - 1) * position);
        float t = ((_snapshots.Count - 1) * position) - lowerIndex;
        print("lower " + lowerIndex + " T " + t);
        return Vector3.Lerp(_snapshots[lowerIndex].position, _snapshots[lowerIndex + 1].position, t);
        
        
    }
    public Quaternion GetRotation(float position)
    {
        if(isRecording)
        {
            Debug.LogWarning("Cannot get rotation while RewindRecorder is still recording!");
            return transform.rotation;
        }
        if(_snapshots == null || _snapshots.Count == 0)
        {
            Debug.LogWarning("RewindRecorder has no data recorded! Please start recording before geting a rotation!");
            return  transform.rotation;
        }
        if(position == 1)
            return _snapshots[_snapshots.Count - 1].rotation;
        
        if(position == 0)
            return _snapshots[0].rotation;

        int lowerIndex = (int)((_snapshots.Count - 1) * position);
        float t = ((_snapshots.Count - 1) * position) - lowerIndex;

        return Quaternion.Lerp(_snapshots[lowerIndex].rotation, _snapshots[lowerIndex + 1].rotation, t);
    }
}
