using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMode;

public interface BaseInteface 
{
    public delegate void Commands(GameState state);
    public event Commands CommandsAction;
}
