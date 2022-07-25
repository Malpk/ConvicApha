using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Rocket : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [Range(0,1f)]
        [SerializeField] private float _delayExplosin;
        [Min(1)]
        [SerializeField] private float _speedMovement;
        [SerializeField] private FireWave _wave;
        [SerializeField] private SpriteRenderer _spriteBody;

        private bool _isActive = true;
        private float _timeDestoy = 4f;
        private Transform _parent;
        private Collider2D _collider;
        private DamageInfo _attackInfo;
        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _parent = transform.parent;
            _collider = GetComponent<Collider2D>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
        }
        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
            _wave.SetAttack(info);
        }
        public void SetTarget(Vector3 target)
        {
            StartCoroutine(Destroy(target));
            Hide(_timeDestoy);
        }
        public IEnumerator Hide(float timeHide = 0)
        {
            yield return new WaitForSeconds(timeHide);
            SetMode(false);
        }
        public void SetMode(bool mode)
        {
            _isActive = mode;
            _collider.enabled = mode;
            _spriteBody.enabled = mode;
            _rigidBody.simulated = mode;
            transform.parent = !mode ? _parent : null;
        }
        private void FixedUpdate()
        {
            _rigidBody.MovePosition(_rigidBody.position + (Vector2)(transform.up * _speedMovement * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, _attackInfo);
                InstateWave();
            }
        }
        private IEnumerator Destroy(Vector3 target)
        {
            yield return new WaitForSeconds(_delayExplosin);
            yield return new WaitWhile(()=> 
            {
                return Vector3.Distance(transform.position, target) > 0.2f;
            });
            InstateWave();

        }
        private void InstateWave()
        {
            if (_wave != null)
            {
                SetMode(false);
                _wave.Explosion();
            }
#if UNITY_EDITOR
            else
                Debug.LogWarning("_wave = null");
#endif
        }
    }
}