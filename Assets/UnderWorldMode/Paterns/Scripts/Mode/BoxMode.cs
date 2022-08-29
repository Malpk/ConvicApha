using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxMode : TotalMapMode
    {
        [Header("Movement Setting")] [Min(1)]
        [SerializeField] private int _countMove;
        [Min(0)]
        [SerializeField] private float _speedMovement;
        [Header("Time Setting")] [Min(1)]
        [SerializeField] private float _scaleDuration;
        [Min(0)]
        [SerializeField] private float _delay;
        [Header("Scale Setting")]
        [SerializeField] private Vector2 _minSize;
        [SerializeField] private Vector2 _maxOffset;

        private Vector3[] _moveDirection = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up,
            Vector3.down, Vector3.one, -Vector3.one,
            new Vector3(1,-1), new Vector3(-1,1)
        };

        private bool _isActive = false;
        private Coroutine _corotine;
        private BoxCollider2D _collider;
        private Vector2 _defoutSize;

        public bool IsActive => _isActive;

        protected void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.enabled = false;
            _defoutSize = _collider.size;
            _maxOffset -= Vector2.one * _minSize / 2;
        }

        public override bool Activate()
        {
#if(UNITY_EDITOR)
            if (_isActive)
                throw new System.Exception("patern is already play");
#endif
            if (_corotine == null)
            {
                _isActive = true;
                _collider.enabled = true;
                State = ModeState.Play;
                _corotine = StartCoroutine(Scale(transform.position, _defoutSize));
                return true;
            }
            return false;
        }
        private IEnumerator Scale(Vector3 previsiousPosition, Vector2 previsiousSize)
        {
            yield return new WaitWhile(() => !IsReady);
            float progress = 0f;
            var target = GetOffset(_maxOffset);
            while (progress < 1f)
            {
                progress += Time.deltaTime / _scaleDuration;
                _collider.size = Vector2.Lerp(previsiousSize, _minSize, progress);
                transform.position = Vector2.Lerp(previsiousPosition, target, progress);
                yield return null;
            }
            yield return StartCoroutine(MoveSqurt());
        }
        private IEnumerator MoveSqurt()
        {
            yield return new WaitForSeconds(_delay);
            Vector3 direction = Vector3.zero;
            for (int i = 0; i < _countMove; i++)
            {
                if (State == ModeState.Pause)
                {
                    yield return new WaitWhile(() => State == ModeState.Pause);
                    yield return new WaitForSeconds(0.1f);
                }
                direction = ChooseDirection(direction);
                var target = Vector3.right * direction.x * _maxOffset.x +
                    Vector3.up * direction.y * _maxOffset.y;
                var duration = CalculateDuration(target, _speedMovement);
                yield return StartCoroutine(Move(target, duration));
            }
            yield return WaitHideMap();
            _collider.enabled = false;
            State = ModeState.Stop;
            _isActive = false;
        }
        private IEnumerator Move(Vector3 target, float duration)
        {
            float progress = 0f;
            var startPosition = transform.position;
            while (progress < 1f && State != ModeState.Stop)
            {
                transform.position = Vector2.Lerp(startPosition, target, progress);
                progress += Time.deltaTime / duration;
                yield return null;
                yield return new WaitWhile(() => State == ModeState.Pause);
            }
            yield return new WaitForSeconds(0.1f);
        }
        private Vector3 GetOffset(Vector3 offset)
        {
            var x = GetOffset(offset.x);
            var y = GetOffset(offset.y);
            return new Vector3(x, y, 0);
        }
        private Vector3 ChooseDirection(Vector3 lostDirection)
        {
            var listDirection = FiltreDirection(_moveDirection, lostDirection);
            var index = Random.Range(0, listDirection.Count); 
            return listDirection[index];
        }
        private List<Vector3> FiltreDirection(Vector3[] direction, Vector3 lostDirection)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < direction.Length; i++)
            {
                if (direction[i] != lostDirection)
                    list.Add(direction[i]);
            }
            return list;
        }
        private float GetOffset(float offset)
        {
            return Random.Range(-offset, offset);
        }
        private float CalculateDuration(Vector3 target, float speed)
        {
            var distance = Vector3.Distance(target, transform.position);
            return Mathf.Abs(distance / speed);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isActive)
                return;
            if (collision.TryGetComponent(out Term term))
            {
                if (term.IsActive)
                {
                    term.Deactivate(false);
                    term.HideItem();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_isActive)
                return;
            if (collision.TryGetComponent(out Term term) && _collider.enabled)
            {
                if (!term.IsActive)
                {
                    term.ShowItem();
                    term.Activate(FireState.Stay);
                }
            }
        }
    }
}