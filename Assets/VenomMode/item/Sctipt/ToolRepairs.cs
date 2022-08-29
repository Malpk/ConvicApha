using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ToolRepairs : SmartItem
    {
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private SpriteRenderer _spriteBody;

        private Collider2D _triger;

        public Sprite Icon => _itemIcon;

        private void Awake()
        {
            _triger = GetComponent<Collider2D>();
            _triger.isTrigger = true;
            SetMode(false);
        }
        private void OnEnable()
        {
            ShowItemAction += () => SetMode(true);
            HideItemAction += () => SetMode(false);
        }
        private void OnDisable()
        {
            ShowItemAction -= () => SetMode(true);
            HideItemAction -= () => SetMode(false);
        }
        public void Pick()
        {
            HideItem();
        }
        private void SetMode(bool mode)
        {
            _spriteBody.enabled = mode;
            _triger.enabled = mode;
        }
    }
}