using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : DeviceWithNegativeEffect
{
    [SerializeField]
    private float _forceValue = 10;
    protected override void ActivateDevice(EffectsHandler effectsHandler)
    {
        base.ActivateDevice(effectsHandler);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        EffectsHandler effectsHandler = collision.attachedRigidbody?.GetComponent<EffectsHandler>();
        if (effectsHandler)
        {
            Rigidbody2D rigidbody = effectsHandler.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(transform.up * _forceValue);
        }
    }
}
