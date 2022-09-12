using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MainMode.GameInteface
{
    public class ScrollPoint : MonoBehaviour
    {
        [SerializeField] private Image _iconItem;
        [SerializeField] private RectTransform _rectTransform;

        private ScrollItem _item;

        public ScrollItem Item => _item;
        public Vector3 Position => _rectTransform.localPosition;

        public void SetItem(ScrollItem item)
        {
            _item = item; 
            _iconItem.sprite = item.ItemImage;
        }
        public void SetPosition(Vector3 position)
        {
            _rectTransform.localPosition = position;
        }
        public void SetOffset(Vector3 offset)
        {
            _rectTransform.localPosition += offset;
        }
    }
}
