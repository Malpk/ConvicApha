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

        private int _offset = 0;
        private ScrollPoint[] _points;
        private Coroutine _curretCommand;

        public System.Action<string,string> OnSelectItem;

        private void Awake()
        {
            _points = _scrollPointHolder.GetComponentsInChildren<ScrollPoint>();
            for (int i = 0; i < _points.Length; i++)
            {
                if (_maxXOffset > Mathf.Abs(_points[i].Position.x))
                {
                    _offset++;
                    _points[i].SetItem(_scrollItems[i]);
                }
            }
        }
        private void OnEnable()
        {
            foreach (var point in _points)
            {
                point.OnSelectItem += (string text, string name) => OnSelectItem?.Invoke(text, name);
            }
        }
        private void OnDisable()
        {
            foreach (var point in _points)
            {
                point.OnSelectItem -= (string text, string name) => OnSelectItem?.Invoke(text, name);
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
            return point.Content;
        }
        public void MoveRight()
        {
            if (_curretCommand == null)
            {
                var item = _scrollItems[_scrollItems.Length-1];
                _curretCommand = GeneralMove(1, item);
                OffsetRight(_scrollItems);
            }
        }
        public void MoveLeft()
        {
            if (_curretCommand == null)
            {
                var item = _scrollItems[_offset < _scrollItems.Length ? _offset : 0];
                _curretCommand = GeneralMove(-1, item);
                OffsetLeft(_scrollItems);
            }
        }
        private void OffsetLeft(ScrollItem[] scrollItems)
        {
            var end = scrollItems.Length - 1;
            var curret = scrollItems[end];
            for (int i = end-1; i >= 0; i--)
            {
                var temp = scrollItems[i];
                scrollItems[i] = curret;
                curret = temp;
            }
            scrollItems[end] = curret;
        }
        private void OffsetRight(ScrollItem[] scrollItems)
        {
            var curret = scrollItems[0];
            for (int i = 1; i < scrollItems.Length; i++)
            {
                var temp = scrollItems[i];
                scrollItems[i] = curret;
                curret = temp;
            }
            scrollItems[0] = curret;
        }
        private Coroutine GeneralMove(int direction, ScrollItem hideItem)
        {
            SetHidePoint(direction, hideItem);
            return StartCoroutine(Move(direction));
        }
        private IEnumerator Move(int direction)
        {
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

        private void SetHidePoint(float direction, ScrollItem item)
        {
            var _offset = 5f;
            foreach (var point in _points)
            {
                if (Mathf.Abs(point.Position.x) >= _maxXOffset - _offset)
                {
                    point.SetPosition(Vector3.right * -direction * _maxXOffset);
                    point.SetItem(item);
                    return;
                }
            }
        }

    }
}
