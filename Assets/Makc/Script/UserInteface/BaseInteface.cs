using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseInteface 
{
    public delegate void Commands(TypeGameEvent state);
    public event Commands CommandsAction;
}
