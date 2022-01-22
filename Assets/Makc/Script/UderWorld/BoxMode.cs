using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxMode : GameMode
    {
        [Header("Game Setting")]
        [SerializeField] private int _countMove;
        [SerializeField] private float _scaleDuration;
        [SerializeField] private float _delay;
        [SerializeField] private float _speedMovement;

        [Header("Perfab Setting")]
        [SerializeField] private Vector2 _minSize;
        [SerializeField] private Vector3 _maxOffset;
        [SerializeField] private TrisMode _trisMode;

        private Vector3[] _moveDirection = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up,
            Vector3.down, Vector3.one, -Vector3.one,
            new Vector3(1,-1), new Vector3(-1,1)
        };
        private bool _status = true;
        private BoxCollider2D _collider;

        public override bool statusWork => _status;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            StartCoroutine(Scale());
        }
        private IEnumerator Move(Vector3 target, float duration)
        {
            float progress = 0f;
            var startPosition = transform.position;
            while (progress < 1f)
            {
                transform.position = Vector2.Lerp(startPosition, target, progress);
                progress += Time.deltaTime / duration;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
        private IEnumerator Scale()
        {
            yield return new WaitWhile(() => _trisMode.state != TernState.Fire);
            float progress = 0f;
            var baseSize = _collider.size;
            var lostPosition = transform.position;
            var target = GetOffset(_maxOffset);
            while (progress < 1f)
            {
                progress += Time.deltaTime / _scaleDuration;
                _collider.size = Vector2.Lerp(baseSize, _minSize, progress);
                transform.position = Vector2.Lerp(lostPosition, target, progress);
                yield return null;
            }
            yield return new WaitForSeconds(_delay);
            yield return StartCoroutine(MoveSqurt());
        }
        private IEnumerator MoveSqurt()
        {
            Vector3 direction = Vector3.zero;
            for (int i = 0; i < _countMove; i++)
            {
                direction = ChooseDirection(direction);
                var target = direction.x * _maxOffset.x * Vector3.right + Vector3.up * direction.y * _maxOffset.y;
                var duration = CalculateDuration(target, _speedMovement);
                yield return StartCoroutine(Move(target, duration));
            }
            _status = false;
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
    }
}