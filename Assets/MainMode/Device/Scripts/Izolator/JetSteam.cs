using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class JetSteam : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] private float _force = 5;
        [Min(1)]
        [SerializeField] private float _effectTime = 1f;
        
        private AttackInfo _attackInfo;

        public float ForceJet => _force;

        public void SetAttack(AttackInfo info)
        {
            _attackInfo = info;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage,_attackInfo);
            }
            if (collision.collider.TryGetComponent<IEffect>(out IEffect screen))
            {
                screen.SetEffect(_attackInfo.Effect, _effectTime);
            }
            if (collision.rigidbody)
                collision.rigidbody.AddForce((Vector2)collision.transform.up * (-_force), ForceMode2D.Impulse);
        }
    }
}