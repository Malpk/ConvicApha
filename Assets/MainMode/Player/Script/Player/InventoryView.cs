using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MainMode
{
    public class InventoryView : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private Image _imageItem;
        [SerializeField] private TextMeshProUGUI _countItems;
        [SerializeField] private TextMeshProUGUI _hotKey;

        public event Action ClickView;

        private void Awake()
        {
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
                _imageItem.enabled = false;

            if (countItem <= 0)
                _countItems.text = "";
            else
                _countItems.text = countItem.ToString();
        }
        public void DisplayEmpty() 
        {
            _imageItem.enabled = false;
            _countItems.text = "";
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

        public void OnPointerDown(PointerEventData eventData) => 
            ClickView?.Invoke();
    }
}
