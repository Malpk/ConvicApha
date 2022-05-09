using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTile : Device
{
    [SerializeField]
    private Gun _parentGun;

    private void Start()
    {
        transform.parent = null;
    }
    protected override void ActivateDevice(EffectsHandler effectsHandler)
    {
        _parentGun.ActivateGun(effectsHandler);
    }
}
