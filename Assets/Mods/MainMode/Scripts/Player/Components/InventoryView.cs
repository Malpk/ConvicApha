using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MainMode.GameInteface
{
    public class InventoryView : MonoBehaviour,IPointerDownHandler
    {

        [SerializeField] private TypeItem _typeCell;
        [SerializeField] private Color _colorDeactivate;
        [Header("Reference")]
        [SerializeField] private Image _imageItem;
        [SerializeField] private TextMeshProUGUI _countItems;
        [SerializeField] private TextMeshProUGUI _hotKey;

        private Color _colorBase;

        public delegate void Click();
        public event Click ClickAction;

        public TypeItem CellType => _typeCell;

        private void Awake()
        {
            _colorBase = _imageItem.color;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                _hotKey.gameObject.SetActive(false);
        }
        public void Display(Sprite spriteItem, int countItem)
        {
            if (spriteItem != null)
            {
                _imageItem.enabled = true;
                _imageItem.sprite = spriteItem;
            }
            else
            {
                _imageItem.enabled = false;
            }
            if (countItem <= 0)
                _countItems.text = "";
            else
                _countItems.text = countItem.ToString();
        }
        public void DisplayInfinity(Sprite spriteItem) 
        {
            if (spriteItem!=null)
            {
                _imageItem.enabled = true;
                _imageItem.sprite = spriteItem;
            }else
            {
                _imageItem.enabled=false;
            }
            _countItems.text = "~";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (ClickAction != null)
                ClickAction();
        }
    }
}
