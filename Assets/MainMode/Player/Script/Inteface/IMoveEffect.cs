using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveEffect 
{
    public void StopMove(float timeStop, EffectType effect = EffectType.None);
    public void ChangeSpeed(float duration, float value = 1);
}

