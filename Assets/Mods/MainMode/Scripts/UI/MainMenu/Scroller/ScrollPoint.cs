using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MainMode.GameInteface
{
    public class ScrollPoint : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _iconItem;
        [SerializeField] private RectTransform _rectTransform;

        private ScrollItem _item;
        private Vector3 _startPosition;

        public System.Action<Sprite> OnSelectItem;
        public System.Action OnExit;


        public ScrollItem Content => _item;
        public Vector3 Position => _rectTransform.localPosition;

        private void Awake()
        {
            _startPosition = _rectTransform.localPosition;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSelectItem?.Invoke(_item.Description);
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
        public void MoveTo(Vector3 offseet)
        {
            _rectTransform.localPosition = _startPosition + offseet;
        }
        public void SetStartPosition()
        {
            _startPosition = _rectTransform.localPosition;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke();
        }
    }
}
