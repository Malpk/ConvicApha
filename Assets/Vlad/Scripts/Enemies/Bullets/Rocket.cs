using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField]
    private DestructiveEffect[] _effectsOnArea;
    [SerializeField]
    private float _radiusArea;
    [SerializeField]
    private SpriteRenderer _spriteArea;
    protected override void Start()
    {
        _spriteArea.enabled = false;
        base.Start();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        ViewArea();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radiusArea);
        foreach (Collider2D collider in colliders)
        {
            EffectsHandler effectsHandler = collider.attachedRigidbody?.GetComponent<EffectsHandler>();
            if (effectsHandler)
            {
                foreach (DestructiveEffect effect in _effectsOnArea)
                {
                    effectsHandler.AddEffect(effect);
                }
            }
        }
    }

    private void ViewArea()
    {
        _spriteArea.enabled = true;
        _spriteArea.transform.parent = null;
        Destroy(_spriteArea.gameObject, 0.2f);
    }

    private void OnValidate()
    {
        _spriteArea.transform.localScale = Vector2.one * _radiusArea * 2;
    }
}
