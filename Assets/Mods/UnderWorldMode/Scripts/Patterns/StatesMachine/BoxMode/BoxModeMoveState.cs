using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class BoxModeMoveState : BasePatternState
    {
        private readonly int count;
        private readonly float speed;
        private readonly Vector2 maxOffset;
        private readonly Vector3[] _directions = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up,
            Vector3.down, Vector3.one, -Vector3.one,
            new Vector3(1,-1), new Vector3(-1,1)
        };
        private readonly Transform boxHolder;

        private float _duration;

        private float _curretCount = 0;
        private float _progress = 0f;
        private Vector2 _target = Vector2.zero;
        private Vector2 _startPosition;

        public BoxModeMoveState(Transform boxHolder, Vector2 maxOffset, int countMove, float speed)
        {
            count = countMove;
            this.speed = speed;
            this.boxHolder = boxHolder;
            this.maxOffset = maxOffset;
        }

        public override bool IsComplite => _curretCount >= count;

        public override void Start()
        {
            _curretCount = 0;
            ResetProgress();
        }
        public override bool Update()
        {
            _progress = Mathf.Clamp01(_progress + Time.deltaTime / _duration);
            boxHolder.position = Vector2.Lerp(_startPosition, _target, _progress);
            if (_progress >= 1)
            {
                _curretCount++;
                ResetProgress();
            }
            return _curretCount < count;
        }

        private void ResetProgress()
        {
            _progress = 0f;
            _target = GetTargetPosition(maxOffset);
            _duration = CalculateDuration(_target, speed);
            _startPosition = boxHolder.position;
        }
        private float CalculateDuration(Vector3 target, float speed)
        {
            var distance = Vector3.Distance(target, boxHolder.position);
            return Mathf.Abs(distance / speed);
        }
        private Vector3 GetTargetPosition(Vector2 maxOffset)
        {
            _target = GetDirection(_target);
            return Vector3.right * _target.x * maxOffset.x +
                Vector3.up * _target.y * maxOffset.y;
        }
        private Vector3 GetDirection(Vector3 lostDirection)
        {
            var listDirection = FiltreDirection(_directions, lostDirection);
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
    }
}