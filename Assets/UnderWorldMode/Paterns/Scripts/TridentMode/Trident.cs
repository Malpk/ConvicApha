using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace Underworld
{
    [RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
    public class Trident : MonoBehaviour, IPause
    {
        [Header("Effect Setting")]
        [SerializeField] private TridentConfig _config;
        [SerializeField] private Animator _effect;
        [SerializeField] private SpriteRenderer _effectSpiteBody;

        private bool _isPause;
        private Coroutine _corotine;
        private Rigidbody2D _rigidBody;
        private SpriteRenderer _sprite;
        private Vector3 _startPostion;

        public bool IsActive => _corotine != null;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
            DisplayTrident(false);
            _startPostion = transform.position;
        }
        public void SetConfig(TridentConfig config)
        {
            _config = config;
        }
        public void SetDistances(float distance)
        {
            transform.position = _startPostion - transform.up * distance;
        }
        #region Movement
        public bool Activate()
        {
            if (_corotine == null)
            {
                DisplayTrident(true);
                _corotine = StartCoroutine(Move());
                return true;
            }
            return false;
        }
        private IEnumerator Move()
        {
            Vector2 startPosition = transform.position;
            while (Vector2.Distance(transform.position, startPosition) < _config.DistanceFly)
            {
                _rigidBody.MovePosition(transform.position +
                   transform.up * _config.SpeedMovement * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
                yield return new WaitWhile(() => _isPause);
            }
            DisplayTrident(false);
            _corotine = null;
        }
        #endregion
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsActive)
                return;
            if (collision.TryGetComponent(out IDamage target))
            {
                target.Explosion();
            }
        }
        private void DisplayTrident(bool mode)
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