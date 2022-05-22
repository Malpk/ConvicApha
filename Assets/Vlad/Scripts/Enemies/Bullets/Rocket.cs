using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rocket : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [Min(1)]
        [SerializeField] private float _timeExplosin;
        [Min(1)]
        [SerializeField] private float _speedMovement;
        [SerializeField] private GameObject _wave;

        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
        }
        private void Start()
        {
            StartCoroutine(Destroy());
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
                Destroy(gameObject);
            }
        }
        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(_timeExplosin);
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