using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class JetSteam : MonoBehaviour, IJet
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] private float _force = 5;
        [SerializeField] private MovementEffect _effect;
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _body;

        private Collider2D _collider;
        private DamageInfo _attackInfo;

        public float ForceJet => _force;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }

        public void SetMode(bool mode)
        {
            _body.enabled = mode;
            _collider.enabled = mode;
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