using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{   
    [RequireComponent(typeof(Collider2D))]
    public abstract class Item : SmartItem, IPickable, IUseable
    {
        [Header("Setting")]
        [Min(1)]
        [SerializeField] private int _count = 1;
        [SerializeField] private bool _isInfinity;
        [SerializeField] private bool _hideOnAwake;
        [SerializeField] private ItemUseType _typeUse;
        [Header("Reference")]
        [SerializeField] protected Sprite ItemSprite;
        [SerializeField] protected SpriteRenderer _spriteBody;
        
        protected Player user;
        private Collider2D _collider;

        protected System.Action UseAction;
        protected System.Action ResetAction;

        public bool Active { get; private set; }
        public bool IsInfinity => _isInfinity;
        public Sprite Sprite => ItemSprite;
        public ItemUseType UseType => _typeUse;

        public int Count { get; private set; }

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
            if (IsShow)
                HideItem();
        }
        public bool Use()
        {
            if (Count > 0)
            {
                if(!IsInfinity)
                    Count--;
                if (UseAction != null)
                    UseAction();
            }
            return Count > 0;
        }
        public void SetDefoutValue()
        {
            Count = _count;
            if (ResetAction != null)
                ResetAction();
        }
        private void SetMode(bool mode)
        {
            Active = mode;
            _spriteBody.enabled = mode;
            _collider.enabled = mode;
        }
    }
}