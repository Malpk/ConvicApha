using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{
    public bool IsComplite { get; private set; } = false;

    public abstract void Start();

    public abstract void Update();

    //public bool GetNextState(out BaseState nextState)
    //{
    //    return 
    //}
}
