using UnityEngine;

namespace MainMode.GameInteface
{
    public class ItemScroller : MonoBehaviour
    {
        [Header("ScrollSetting")]
        [SerializeField] private float _moveOffset;
        [SerializeField] private float _timeScroll;
        [Header("Reference")]
        [SerializeField] private ScrollItem[] _scrollItems;
        [SerializeField] private ScrollPoint[] _points;

        private float _progress = 0f;
        private Vector3 _curretOffset;

        public System.Action<string, string> OnSelectItem;

        private void Awake()
        {
            for (int i = 0; i < _scrollItems.Length && i < _points.Length; i++)
            {
                _points[i].SetItem(_scrollItems[i]);
            }
            foreach (var point in _points)
            {
                point.OnSelectItem += (string text, string name) => OnSelectItem?.Invoke(text, name);
            }
        }
        private void OnDestroy()
        {
            foreach (var point in _points)
            {
                point.OnSelectItem -= (string text, string name) => OnSelectItem?.Invoke(text, name);
            }
        }

        private void Start()
        {
            enabled = false;
        }
        private void Update()
        {
            _progress = Mathf.Clamp01(_progress + Time.deltaTime / _timeScroll);
            MovePoint(_progress);
            if (_progress >= 1)
            {
                _progress = 0f;
                enabled = false;
            }
        }

        private void MovePoint(float progress)
        {
            foreach (var point in _points)
            {
                point.MoveTo(_curretOffset * progress);
            }
        }

        #region Controllers
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
            if (!enabled)
            {
                enabled = true;
                _curretOffset = Vector3.right * _moveOffset;
                OffsetRight(_scrollItems);
                IntializateHidePoint(_scrollItems[0]);
                IntializatePoint();
            }
        }
        public void MoveLeft()
        {
            if (!enabled)
            {
                enabled = true;
                _curretOffset = Vector3.left * _moveOffset;
                OffsetLeft(_scrollItems);
                IntializateHidePoint(GetLeftItem(), false);
                IntializatePoint();
            }
        }
        #endregion

        #region Intializate Points
        private void IntializatePoint()
        {
            foreach (var point in _points)
            {
                point.SetStartPosition();
            }
        }
        private void IntializateHidePoint(ScrollItem item, bool rigthMove = true)
        {
            var hidePoint = _points[0];
            for (int i = 1; i < _points.Length; i++)
            {
                if (Mathf.Abs(_points[i].Position.x) > Mathf.Abs(hidePoint.Position.x))
                    hidePoint = _points[i];
            }
            var direction = rigthMove ? -1 : 1;
            hidePoint.SetItem(item);
            hidePoint.SetPosition(new Vector3(Mathf.Abs(hidePoint.Position.x) * direction,
                hidePoint.Position.y, hidePoint.Position.z));
        }
        #endregion

        #region AroundArray
        private void OffsetLeft(ScrollItem[] scrollItems)
        {
            var hideElement = scrollItems[0];
            for (int i = 1; i < scrollItems.Length; i++)
            {
                scrollItems[i - 1] = scrollItems[i];
            }
            scrollItems[scrollItems.Length - 1] = hideElement;
        }
        private void OffsetRight(ScrollItem[] scrollItems)
        {
            var index = scrollItems.Length - 1;
            var hideElement = scrollItems[index];
            for (int i = index; i > 0 ; i--)
            {
                scrollItems[i] = scrollItems[i - 1];
            }
            scrollItems[0] = hideElement;
        }
        #endregion
        private ScrollItem GetLeftItem()
        {
            return _points.Length > 2 ?  _scrollItems[_scrollItems.Length - 1] : _scrollItems[0];
        }
    }
}
