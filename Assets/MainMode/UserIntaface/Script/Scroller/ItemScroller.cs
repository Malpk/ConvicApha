using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class ItemScroller : MonoBehaviour
    {
        [Header("ScrollSetting")]
        [SerializeField] private float _moveOffset;
        [SerializeField] private float _speedOffset;
        [SerializeField] private float _maxXOffset;
        [Header("Reference")]
        [SerializeField] private Transform _scrollPointHolder;
        [SerializeField] private ScrollItem[] _scrollItems;


        private int _rightIndex;
        private int _leftIndex;
        private ScrollPoint[] _points;
        private Coroutine _curretCommand;

        private void Awake()
        {
            _points = _scrollPointHolder.GetComponentsInChildren<ScrollPoint>();
            _leftIndex = (_points.Length - 1) / 2;
            for (int i = 0; i < _points.Length; i++)
            {
                _leftIndex = GetPreviusIndex(_leftIndex);
                _points[i].SetItem(_scrollItems[_leftIndex]); 
            }
            _rightIndex = GetPreviusIndex(_leftIndex);
        }

        public void MoveRight()
        {
            if (_curretCommand == null)
            {
                _rightIndex = GetPreviusIndex(_rightIndex);
                _leftIndex = GetNextIndex(_leftIndex);
                _curretCommand = StartCoroutine(Move(1, _leftIndex));
            }
        }

        public void MoveLeft()
        {
            if (_curretCommand == null)
            {
                _rightIndex = GetNextIndex(_rightIndex);
                _leftIndex = GetPreviusIndex(_leftIndex);
                _curretCommand = StartCoroutine(Move(-1, _rightIndex));
            }
        }

        public ScrollItem GetSelectItem()
        {
            var point = _points[0];
            for (int i = 0; i < _points.Length; i++)
            {
                if (Mathf.Abs(point.Position.x) > Mathf.Abs(_points[i].Position.x))
                    point = _points[i];
            }
            return point.Item;
        }

        private IEnumerator Move(int direction, int index)
        {
            SetHidePoint(direction, index);
            var points = SetOffset(_moveOffset * direction);
            while (points.Count > 0)
            {
                var list = new List<ScrollPoint>();
                foreach (var point in points)
                {
                    if (point.UpdatePosition(_speedOffset))
                        list.Add(point);
                }
                points.Clear();
                points = list;
                yield return null;
            }
            _curretCommand = null;
        }
        private int GetNextIndex(int curretIndex)
        {
            curretIndex++;
            if (curretIndex >= _scrollItems.Length)
                curretIndex = 0;
            return curretIndex;
        }
        private int GetPreviusIndex(int curretIndex)
        {
            curretIndex--;
            if (curretIndex < 0)
                curretIndex = _scrollItems.Length - 1;
            return curretIndex;
        }
        private List<ScrollPoint> SetOffset(float offset)
        {
            var points = new List<ScrollPoint>();
            foreach (var point in _points)
            {
                point.SetTarget(Vector3.right * offset);
                points.Add(point);
            }
            return points;
        }

        private void SetHidePoint(float direction, int index)
        {
            foreach (var point in _points)
            {
                if (Mathf.Abs(point.Position.x) >= _maxXOffset)
                {
                    point.SetPosition(Vector3.right * -direction * _maxXOffset);
                    point.SetItem(_scrollItems[index]);
                    return;
                }
            }
        }

    }
}
