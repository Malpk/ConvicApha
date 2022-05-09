using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Device : MonoBehaviour
{
    [SerializeField] protected DeviceEnum deviceType;
    public DeviceEnum DeviceType => deviceType;

    protected abstract void ActivateDevice(EffectsHandler effectsHandler);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        EffectsHandler effectsHandler = collision.attachedRigidbody?.GetComponent<EffectsHandler>();
        if (effectsHandler)
        {
            ActivateDevice(effectsHandler);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        EffectsHandler effectsHandler = collision.rigidbody?.GetComponent<EffectsHandler>();
        if (effectsHandler)
        {
            ActivateDevice(effectsHandler);
        }
    }
}
