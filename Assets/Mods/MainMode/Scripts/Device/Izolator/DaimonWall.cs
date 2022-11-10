using UnityEngine;

namespace MainMode
{
    public class DaimonWall : MonoBehaviour, IDamage, IJet
    {
        [Min(1)]
        [SerializeField] private int _healhtWall = 1;
        [Header("Reference")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _wallColider;
        [SerializeField] private SpriteRenderer _wallSprite;

        private int _curretHealht;
        private bool _isActivate;

        public bool IsActive => _isActivate;

        private void Awake()
        {
            _curretHealht = _healhtWall;
            _animator = GetComponent<Animator>();
        }
        public void Activate()
        {
            _curretHealht = _healhtWall;
            _animator.SetBool("Mode", false);
        }

        public void Explosion()
        {
            Activate();
        }
        private void SetActivateState()
        {
            _isActivate = true;
            _wallSprite.enabled = true;
            _wallColider.enabled = true;
        }
        private void SetDeactivateState()
        {
            _isActivate = false;
            _wallSprite.enabled = false;
            _wallColider.enabled = false;
        }
        public void TakeDamage(int damage, DamageInfo type)
        {
            _curretHealht -= damage;
            if (_curretHealht <= 0)
                Explosion();
        }
        public void SetMode(bool mode)
        {
            _curretHealht = mode ? _healhtWall : 0;
            _animator.SetBool("Mode", mode);
        }

        public void SetAttack(DamageInfo info)
        {
        }
    }
}
