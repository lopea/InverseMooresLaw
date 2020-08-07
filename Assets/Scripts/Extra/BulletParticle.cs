using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystemRenderer))]
public class BulletParticle : MonoBehaviour
{
    ParticleSystemRenderer _renderer;

    public Color SlowColor, FastColor;
    
    void Awake()
    {
        _renderer = GetComponent<ParticleSystemRenderer>();
    }

    void Update()
    {
        _renderer.material.SetColor("_EmissionColor", (ComputerStateManager.CurrentState == ComputerState._8bit) ? SlowColor : FastColor);
    }
}
