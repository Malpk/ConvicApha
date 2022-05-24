using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rocket : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [Range(0,1f)]
        [SerializeField] private float _delayExplosin;
        [Min(1)]
        [SerializeField] private float _speedMovement;
        [SerializeField] private GameObject _wave;

        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
        }
        public void SetTarget(Vector3 target)
        {
            StartCoroutine(Destroy(target));
        }
        private void Update()
        {
            _rigidBody.MovePosition(_rigidBody.position + (Vector2)(transform.up * _speedMovement * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage);
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
                Instantiate(_wave, transform.position, transform.rotation).GetComponent<FireWave>().Explosion();
#if UNITY_EDITOR
            else
                Debug.LogWarning("_wave = null");
#endif
            Destroy(gameObject);
        }
    }
}