using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace Underworld
{
    [RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
    public class Trident : MonoBehaviour, IPause
    {
        [SerializeField] private Marker _marker;
        [Header("Effect Setting")]
        [SerializeField] private Animator _effect;
        [SerializeField] private SpriteRenderer _effectSpiteBody;

        private bool _isPause;
        private float _warningTime;
        private float _speedMovement;
        private Coroutine _corotine;
        private Rigidbody2D _rigidBody;
        private SpriteRenderer _sprite;

        private int _curretDirection = 0;

        private int[] _direction = new int[] { -1, 1 };
        private int[] _angls = new int[] { 0, 180 };

        public bool IsActive => _corotine != null;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
            Show(false); 
        }
        public void Intilizate(TridentSetting setting)
        {
            _warningTime = setting.WarningTime;
            _speedMovement = setting.SpeedMovement;
        }
        #region Movement
        public bool Activate()
        {
            if (_corotine != null)
                return false;
            //IntilizateTrident(angle, marker);
            _corotine = StartCoroutine(Move());
            Show(true);
            return true;
        }
        public void SetDistance(float distacne)
        {
            var index = Random.Range(0, _angls.Length);
            _curretDirection = _direction[index];
            transform.localRotation = Quaternion.Euler(Vector3.forward * _angls[index]);
            transform.localPosition += Vector3.up * _curretDirection *  distacne;
        }
        private IEnumerator Move()
        {
            //_marker.ActiveMarker();
            yield return new WaitWhile(() => _marker.IsActive);
            Vector2 startPosition = transform.localPosition;
            var target = new Vector2(startPosition.x, -startPosition.y);
            while (Vector2.Distance(transform.localPosition, target) > 0.05f)
            {
                _rigidBody.MovePosition(transform.TransformPoint(transform.localPosition +
                    Vector3.up * _speedMovement * Time.fixedDeltaTime));
                yield return null;
                yield return new WaitWhile(() => _isPause);
            }
            Show(false);
            _corotine = null;
        }
        #endregion
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                target.Dead();
            }
        }
        private void Show(bool mode)
        {
            _effect.enabled = mode;
            _sprite.enabled = mode;
            _effectSpiteBody.enabled = mode;
        }
        public void Pause()
        {
            _isPause = true;
        }

        public void UnPause()
        {
            _isPause = false;
        }
    }
}