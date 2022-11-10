using UnityEngine;
using PlayerComponent;

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
        [SerializeField] private Animator _jetAnimator;
        [SerializeField] private SpriteRenderer _body;

        private Collider2D _collider;
        private DamageInfo _attackInfo;

        private bool _isActivate;

        public float ForceJet => _force;

        public bool IsActive => _isActivate;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            SetMode(false);
        }

        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }

        public void SetMode(bool mode)
        {
            _jetAnimator.SetBool("Mode", mode);
        }
        private void SetActivateStateAnimationEvent()
        {
            SetState(true);
        }
        private void SetDeactivateStateAnimatuinEvent()
        {
            SetState(false);
        }

        private void SetState(bool mode)
        {
            _isActivate = mode;
            _collider.enabled = mode;
            _body.enabled = mode;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitDamage(collision);
            if (collision.TryGetComponent(out Rigidbody2D body))
                body.AddForce((Vector2)collision.transform.up * (-_force), ForceMode2D.Impulse);
        }

        private void HitDamage(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, _attackInfo);
            }
        }
    }
}