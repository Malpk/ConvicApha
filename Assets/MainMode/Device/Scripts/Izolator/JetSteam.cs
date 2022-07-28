using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class JetSteam : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] private float _force = 5;
        [SerializeField] private MovementEffect _effect;

        private DamageInfo _attackInfo;

        public float ForceJet => _force;

        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage,_attackInfo);
            }
            if (collision.collider.TryGetComponent(out RobotMan man))
                man.AddEffects(_effect, _attackInfo.TimeEffect);
            if (collision.rigidbody)
                collision.rigidbody.AddForce((Vector2)collision.transform.up * (-_force), ForceMode2D.Impulse);
        }
    }
}