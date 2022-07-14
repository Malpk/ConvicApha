using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rocket : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [Range(0,1f)]
        [SerializeField] private float _delayExplosin;
        [Min(1)]
        [SerializeField] private float _speedMovement;
        [SerializeField] private FireWave _wave;

        private Rigidbody2D _rigidBody;
        private DamageInfo _attackInfo;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
        }
        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }
        public void SetTarget(Vector3 target)
        {
            StartCoroutine(Destroy(target));
            Destroy(gameObject, 4);
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
                var explosion = Instantiate(_wave.gameObject, transform.position, transform.rotation).GetComponent<FireWave>();
                explosion.SetAttack(_attackInfo);
                explosion.Explosion();
                enabled = false;
            }
#if UNITY_EDITOR
            else
                Debug.LogWarning("_wave = null");
#endif
            Destroy(gameObject);
        }


    }
}