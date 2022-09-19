using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{   
    [RequireComponent(typeof(Collider2D))]
    public abstract class Item : SmartItem, IPickable, IUseable
    {
        [Header("Setting")]
        [SerializeField] private bool _hideOnAwake;
        [SerializeField] private ItemUseType _typeUse;
        [Header("Reference")]
        [SerializeField] protected Sprite ItemSprite;
        [SerializeField] protected SpriteRenderer _spriteBody;
        
        protected Player user;
        private Collider2D _collider;
        public bool Active { get; private set; }

        public Sprite Sprite => ItemSprite;
        public ItemUseType UseType => _typeUse;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>(); 
            _collider.isTrigger = true;
            if (_hideOnAwake)
                SetMode(false);
            else
                ShowItem();
        }
        private void OnEnable()
        {
            ShowItemAction +=() => SetMode(true);
            HideItemAction += () => SetMode(false);
        }
        private void OnDisable()
        {
            ShowItemAction -= () => SetMode(true);
            HideItemAction -= () => SetMode(false);
        }
        public void Pick(Player player)
        {
            user = player;
            if(IsShow)
                HideItem();
        }
        public abstract void Use();
        private void SetMode(bool mode)
        {
            Active = mode;
            _spriteBody.enabled = mode;
            _collider.enabled = mode;
        }
    }
}