using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeviceWithNegativeEffect : Device
{
    [SerializeField]
    protected DestructiveEffect[] _directionalEffects;
    protected override void ActivateDevice(EffectsHandler effectsHandler)
    {
        foreach(DestructiveEffect effect in _directionalEffects)
        {
            effectsHandler.AddEffect(effect);
        }
    }
}
