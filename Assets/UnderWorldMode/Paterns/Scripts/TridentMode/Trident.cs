using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace Underworld
{
    [RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
    public class Trident : MonoBehaviour
    {
        [Header("Effect Setting")]
        [SerializeField] private Animator _effect;
        [SerializeField] private SpriteRenderer _effectSpiteBody;

        private float _warningTime;
        private float _speedMovement;
        private Coroutine _corotine;
        private Rigidbody2D _rigidBody;
        private SpriteRenderer _sprite;
        private Vector3 _centerPosition;

        public bool IsActive => _corotine != null;

        private void Awake()
        {
            _centerPosition = transform.position;
            _sprite = GetComponent<SpriteRenderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
            SetActiveTrident(false); 
        }
        public void Constructor(float speedMovement,float warningTime)
        {
            _warningTime = warningTime;
            _speedMovement = speedMovement;
        }
        public bool StartMove(float moveDistance, int angle, Marker marker)
        {
            if (_corotine != null)
                return false;
            marker.ActiveMarker(_centerPosition, angle, _warningTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            transform.position = _centerPosition - transform.up * moveDistance / 2;
            _corotine = StartCoroutine(Move(moveDistance, marker));
            SetActiveTrident(true);
            return true;
        }
        private void SetActiveTrident(bool mode)
        {
            _effect.enabled = mode;
            _sprite.enabled = mode;
            _effectSpiteBody.enabled = mode;
        }
        private IEnumerator Move(float moveDistance, Marker marker)
        {
            yield return new WaitWhile(() => marker.IsActive);
            var startPosition = transform.position;
            while (moveDistance > Vector2.Distance(startPosition, transform.position))
            {
                _rigidBody.MovePosition(_rigidBody.position + (Vector2)transform.up
                   * _speedMovement * Time.fixedDeltaTime);
                yield return null;
            }
            SetActiveTrident(false);
            _corotine = null;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerLegacy>(out PlayerLegacy player))
            {
            }
        }
    }
}