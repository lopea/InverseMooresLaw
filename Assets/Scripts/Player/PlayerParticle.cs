using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    private ParticleSystem _system;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _system = GetComponentInChildren<ParticleSystem>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        var e = _system.emission;
        e.rateOverTimeMultiplier = _player.Velocity.magnitude;
    }
}
