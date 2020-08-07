using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComputerState
{
    
    _8bit = 1,
    _x86 = 0,
}

public class ComputerStateManager : MonoBehaviour
{
    private ComputerState _state;
    [SerializeField]
    private float cooldown;

    [SerializeField]
    private float length;

    

    private static ComputerStateManager _manager;

    public static bool Enabled{get=>manager.enabled;}
    private static ComputerStateManager manager
    {
        get 
        {
            if(_manager == null)
            {
              _manager = FindObjectOfType<ComputerStateManager>();   
            }
            return _manager;
        }
    }
    
    public static float CoolDownTimer{get; private set;}
    public static float Timer{get; private set;}
    public static ComputerState CurrentState {get => (Enabled) ? manager._state: ComputerState._x86; 
    set
    {
        if( (IsOld && CoolDownTimer != 0) || !Enabled)
        {
            return;
        }
        manager._state = value;
        if(IsOld && Timer != manager.length)
        {
            Timer = manager.length;
        }
            
    }}
    
    public static  bool IsOld{get => CurrentState == ComputerState._8bit;}

    void Awake()
    {
        CoolDownTimer = 0;
        Timer = 0;
    }
    void Update()
    {
        if(IsOld)
        {
            Timer -= Time.deltaTime;
            if(Timer < 0)
            {
                CurrentState = ComputerState._x86;
            }
        }
    }

}
