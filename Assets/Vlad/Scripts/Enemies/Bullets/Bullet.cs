using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : DeviceWithNegativeEffect
{
    [SerializeField]
    protected float _speed;

    private Rigidbody2D _rigidbody;
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _speed * transform.up;
        Destroy(gameObject, 5f);
    }

    protected override void ActivateDevice(EffectsHandler effectsHandler)
    {
        base.ActivateDevice(effectsHandler);
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Destroy(gameObject);
    }
}
