using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxMode : GameMode,IModeForSwitch
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
        [SerializeField] private Vector3 _maxOffset;

        private Vector3[] _moveDirection = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up,
            Vector3.down, Vector3.one, -Vector3.one,
            new Vector3(1,-1), new Vector3(-1,1)
        };
        private Point[,] _map;
        private BoxCollider2D _collider;

        public bool IsAttackMode => throw new System.NotImplementedException();

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }
        public void Constructor(SwitchMode swictMode)
        {
            _map = swictMode.builder.Map;
            TurnOnPoints(_map);
            startMode = StartCoroutine(Scale(transform.position, _collider.size));
        }
        private IEnumerator Scale(Vector3 previsiousPosition, Vector3 previsiousSize)
        {
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
            startMode = null;
        }
        private IEnumerator MoveSqurt()
        {
            yield return new WaitForSeconds(_delay);
            Vector3 direction = Vector3.zero;
            for (int i = 0; i < _countMove; i++)
            {
                direction = ChooseDirection(direction);
                var target = direction.x * _maxOffset.x * Vector3.right + Vector3.up * direction.y * _maxOffset.y;
                var duration = CalculateDuration(target, _speedMovement);
                yield return StartCoroutine(Move(target, duration));
            }
            yield return new WaitWhile(() => TurnOffPoints(_map).IsActive);
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