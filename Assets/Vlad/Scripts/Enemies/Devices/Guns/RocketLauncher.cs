using System.Collections;
using UnityEngine;

namespace BaseMode
{
    public class RocketLauncher : KI
    {
        [SerializeField] private float _aimTime = 1f;
        [SerializeField] private float _speedRotation;

        [SerializeField] private GameObject _bullet;

        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rotateBody;
        [SerializeField] private Transform _signalHolder;
        [SerializeField] private Transform _spawnProjectelePosition;
        [SerializeField] private FireWave _wave;


        private Vector3 _lostTargetPosition;
        private ShootPoint _shootPoint;
        private SignalTile[] _signals;

        private Coroutine _coroutine;


        private void Awake()
        {
            _signals = _signalHolder.GetComponentsInChildren<SignalTile>();
            _shootPoint = GetComponentInChildren<ShootPoint>();
        }
        private void OnEnable()
        {
            if (_shootPoint != null)
                _shootPoint.FireAction += Fire;
            foreach (var signal in _signals)
            {
                signal.SingnalAction += SetTarget;
            }
        }
        private void OnDisable()
        {
            if (_shootPoint != null)
                _shootPoint.FireAction -= Fire;
            foreach (var signal in _signals)
            {
                signal.SingnalAction -= SetTarget;
            }
        }

        public void SetTarget(Collider2D target)
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(Rotate(target.transform));
        }
        private IEnumerator Rotate(Transform target)
        {
            float progress = 0f;
            while (progress < 1f)
            {
                var localPOsition = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
                var direction = localPOsition.y > 0 ? 1 : -1;
                _rotateBody.MoveRotation(Quaternion.Lerp(_rotateBody.transform.rotation,
                    Quaternion.Euler(Vector3.forward * (-90 + Vector2.Angle(Vector2.right, localPOsition) *
                          direction)), Time.deltaTime * _speedRotation));
                progress += Time.deltaTime / _aimTime;
                yield return null;
            }
            _lostTargetPosition = _rotateBody.transform.position + _rotateBody.transform.up * Vector3.Distance(transform.position, target.position);
            Shoot();
            if (_shootPoint == null)
                Fire();
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(ReturnState());
            _coroutine = null;
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_rotateBody.rotation) > 0.01f)
            {
                _rotateBody.MoveRotation(Mathf.MoveTowards(_rotateBody.rotation, 0, _speedRotation));
                yield return null;
            }
        }

        private void Shoot()
        {
            _animator.SetTrigger("Shoot");
        }
        private void Fire()
        {
            if (_wave != null)
                _wave.Explosion();
#if UNITY_EDITOR
            else
                Debug.LogWarning("_wave = null");
#endif
            var rocet =  Instantiate(_bullet, _spawnProjectelePosition.position, Quaternion.Euler(Vector3.forward * _rotateBody.rotation));
            rocet.GetComponent<Rocket>().SetTarget(_lostTargetPosition);
        }
    }
}