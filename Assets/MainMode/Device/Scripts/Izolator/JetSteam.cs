using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class JetSteam : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] private float _force = 5;
        [Min(1)]
        [SerializeField] private float _effectTime = 1f;

        public float ForceJet => _force;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage);
            }
            if (collision.collider.TryGetComponent<IEffect>(out IEffect screen))
            {
                screen.SetEffect(EffectType.Fire, _effectTime);
            }
            if (collision.rigidbody)
                collision.rigidbody.AddForce((Vector2)collision.transform.up * (-_force), ForceMode2D.Impulse);
        }
    }
}