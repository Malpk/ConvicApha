using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MainMode.GameInteface
{
    public class ScrollPoint : MonoBehaviour,IPointerEnterHandler
    {
        [SerializeField] private Image _iconItem;
        [SerializeField] private RectTransform _rectTransform;

        private Vector3 _target;
        private ScrollItem _item;

        public System.Action<string,string> OnSelectItem;

        public ScrollItem Content => _item;
        public Vector3 Position => _rectTransform.localPosition;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSelectItem?.Invoke(_item.Description, _item.Name);
        }

        public void SetItem(ScrollItem item)
        {
            _item = item; 
            _iconItem.sprite = item.ItemImage;
        }
        public void SetPosition(Vector3 position)
        {
            _rectTransform.localPosition = position;
        }
        public void SetTarget(Vector3 offset)
        {
            _target = _rectTransform.localPosition + offset;
        }

        public bool UpdatePosition(float move)
        {
            _rectTransform.localPosition = Vector3.MoveTowards(_rectTransform.localPosition, _target, move);
            return _rectTransform.localPosition.x != _target.x;
        }
    }
}
