using UnityEngine.UI;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private ScrollItem _item;

        public ScrollItem ContainItem => _item;

        public void SetItem(ScrollItem item)
        {
            _item = item;
            _icon.sprite = item.ItemImage;
        }
    }
}