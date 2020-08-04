using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComputerState
{
    
    _8bit = -1,
    _x86 = 0,
    _quantium = 1
}

public class ComputerStateManager : MonoBehaviour
{
    private ComputerState _state;

    private static ComputerStateManager _manager;

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

    public static ComputerState CurrentState {get => manager._state; set => manager._state = value;}
    

}
