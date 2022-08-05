using System.Collections;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public class VenomJet : MonoBehaviour, IJet
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private int _damageValue = 1;
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _body;

        private Collider2D _collider;
        private DamageInfo _attackInfo;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damageValue, _attackInfo);
            }
        }
    }
}