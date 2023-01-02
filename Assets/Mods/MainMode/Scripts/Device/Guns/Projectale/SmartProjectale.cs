using UnityEngine;

namespace MainMode
{
    public class SmartProjectale : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _following;
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] private float _timeDestriy = 1f;
        [Min(1)]
        [SerializeField] private float _speedMovement = 1;
        [SerializeField] private DamageInfo _damageInfo;
        [Header("Reference")]
        [SerializeField] private Player _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidBody;

        private float _progress = 0f;

        public bool IsActivate { get; private set; }

        private void Start()
        {
            if (_playOnStart)
                Activate(_target);
        }

        private void FixedUpdate()
        {
            if(_following)
                Rotate();
            var move = (Vector2)transform.up * _speedMovement * Time.fixedDeltaTime;
            _rigidBody.MovePosition(_rigidBody.position + move);
            _progress += Time.deltaTime / _timeDestriy;
            if (_progress >= 1f)
            {
                Explosion();
            }
        }

        public void SetAttack(DamageInfo damageinfo)
        {
            _damageInfo = damageinfo;
        }
        public void Activate(Player target)
        {
            _target = target;
            _progress = 0f;
            gameObject.SetActive(true);
            enabled = true;
            IsActivate = true;
        }
        private void Rotate()
        {
            var localPosition = (Vector3)_target.Position - transform.position;
            var angle = Vector3.Angle(Vector3.up, localPosition) * (localPosition.x > 0 ? -1 : 1);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        private void Explosion()
        {
            if (_animator)
                _animator.SetTrigger("Explosion");
            else
                DestroyObject();
        }
        private void DestroyObject()
        {
            enabled = false;
            IsActivate = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                _animator.SetTrigger("Explosion");
                player.TakeDamage(_damage, _damageInfo);
            }
        }
    }
}