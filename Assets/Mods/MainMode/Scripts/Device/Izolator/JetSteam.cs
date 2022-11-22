using UnityEngine;

namespace MainMode
{
    public class JetSteam : JetPoint
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] private float _force = 5;

        private DamageInfo _attackInfo;

        public float ForceJet => _force;

        public override void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitDamage(collision);
            if (collision.TryGetComponent(out Rigidbody2D body))
                body.AddForce((Vector2)collision.transform.up * (-_force), ForceMode2D.Impulse);
        }

        private void HitDamage(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                target.TakeDamage(_damage, _attackInfo);
            }
        }
    }
}