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
        [Header("Scene Setting")]
        [SerializeField] private Vector2 _minSize;
        [SerializeField] private Vector3 _maxOffset;

        private Vector3[] _moveDirection = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up,
            Vector3.down, Vector3.one, -Vector3.one,
            new Vector3(1,-1), new Vector3(-1,1)
        };
        private bool _status = true;
        private Vector3 _lostDireciton = Vector3.zero;
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
        protected override void ModeUpdate()
        {
        }
        private IEnumerator Scale()
        {
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
            for (int i = 0; i < _countMove; i++)
            {
                var listDirection = GetDirection(_moveDirection);
                var index = Random.Range(0, listDirection.Count);
                var direction = listDirection[index];
                _lostDireciton = direction;
                var target = direction.x * _maxOffset.x * Vector3.right + Vector3.up * direction.y * _maxOffset.y;
                var duration = Mathf.Abs(Vector3.Distance(target,transform.position) / (_speedMovement));
                float progress = 0f;
                var lostPostion = transform.position;
                while (progress < 1f)
                {
                    transform.position = Vector2.Lerp(lostPostion, target, progress);
                    progress += Time.deltaTime / duration;
                    yield return null;
                }
                yield return new WaitForSeconds(0.1f);
            }
            _status = false;
        }
        private List<Vector3> GetDirection(Vector3 [] direction)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < direction.Length; i++)
            {
                if (direction[i] != _lostDireciton)
                    list.Add(direction[i]);
            }
            return list;
        }
        private Vector3 GetOffset(Vector3 offset)
        {
            var x = GetOffset(offset.x);
            var y = GetOffset(offset.y);
            return new Vector3(x, y, 0);
        }
        private float GetOffset(float offset)
        {
            return Random.Range(-offset, offset);
        }
    }
}