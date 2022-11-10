using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosion 
{
    public bool ReadyExplosion { get; }
    public void Explosion();
}
