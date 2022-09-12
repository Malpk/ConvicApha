using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.GameInteface
{
    public class ScrolMenuBase : MonoBehaviour
    {
        [SerializeField] private float _timeOffset;
        [SerializeField] private float _offset;
        [SerializeField] private float _maxXOffsetRadius;
        [SerializeField] private RectTransform _scrolPointHolder;

        private int _leftItem;
        private int _rightItem;
        private ScrollItem[] _items;

        private List<ScrollPoint> _points = new List<ScrollPoint>();
        private List<ScrollPoint> _deactiveScroll = new List<ScrollPoint>();

        public bool IsReady { get; private set; }
        public event System.Action IntilizateAction;

        private void Awake()
        {
            _points.AddRange(_scrolPointHolder.GetComponentsInChildren<ScrollPoint>());
            if (IntilizateAction != null)
                IntilizateAction();
            SetHidePoint();
        }
        public void Intializate(ScrollItem[] items)
        {
            var count = items.Length > _points.Count ? _points.Count : items.Length;
            _leftItem = 0;
            _rightItem = count - 1;
            _items = items;
            for (int i = 0; i < count; i++)
            {
                _points[i].SetItem(items[i]);
            }
        }

        public void MoveRight()
        {
            if (_rightItem + 1 > _items.Length - 1)
            {
                _rightItem = 0;
                _leftItem = _items.Length - 1;
            }
            else
            {
                _rightItem++;
                _leftItem++;
            }
            SetMove(_items[_rightItem], Vector3.right);
        }
        public void MoveLeft()
        {
            if (_leftItem - 1 < 0)
            {
                _leftItem = _items.Length - 1;
                _rightItem = 0;
            }
            else
            {
                _leftItem--;
                _rightItem--;
            }
            SetMove(_items[_leftItem], Vector3.left);
        }
        public ScrollPoint GetCenterPoint()
        {
            ScrollPoint point = _points[0];
            for (int i = 0; i < _items.Length; i++)
            {
                if (Mathf.Abs(point.Position.x) > Mathf.Abs(_points[i].Position.x))
                {
                    point = _points[i];
                }
            }
            return point;
        }
        private void SetMove(ScrollItem item, Vector3 direction)
        {
            if (!IsReady)
            {
                SetNextPoint(direction, item);
                StartCoroutine(Move(direction));
            }
        }

        private IEnumerator Move(Vector3 direction)
        {
            IsReady = true;
            var steep = _offset / _timeOffset;
            var progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime / _timeOffset;
                SetOffset(direction * steep * Time.deltaTime);
                yield return null;
            }
            SetHidePoint();
            IsReady = false;
        }

        private void SetNextPoint(Vector3 direction, ScrollItem item)
        {
            if (_deactiveScroll.Count > 0)
            {
                var point = _deactiveScroll[0];
                point.SetItem(item);
                point.SetPosition(Vector3.zero - direction * _maxXOffsetRadius);
                _deactiveScroll.Remove(point);
                _points.Add(point);
            }
        }
        private void SetOffset(Vector3 offset)
        {
            foreach (var point in _points)
            {
                point.SetOffset(offset);
            }
        }
        private void SetHidePoint()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (Mathf.Abs(_points[i].Position.x) >= _maxXOffsetRadius)
                {
                    _deactiveScroll.Add(_points[i]);
                    _points.Remove(_points[i]);
                }
            }
        }
    }
}
