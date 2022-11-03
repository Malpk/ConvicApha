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
        [SerializeField] private bool _showOnStart;
        [SerializeField] private ItemUseType _typeUse;
        [Header("Reference")]
        [SerializeField] protected Sprite ItemSprite;
        [SerializeField] protected SpriteRenderer _spriteBody;

        private Collider2D _collider;

        public bool Active { get; private set; }

        protected Player user;

        protected System.Action UseAction;
        protected System.Action ResetAction;

        public abstract string Name { get; }
        public bool IsInfinity => _isInfinity;
        public Sprite Sprite => ItemSprite;
        public ItemUseType UseType => _typeUse;

        public int Count { get; private set; }

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteBody = GetComponent<SpriteRenderer>();
            _collider.isTrigger = true;
            Count = _count;
            SetMode(false);
        }

        protected virtual void OnEnable()
        {
            ShowItemAction += () => SetMode(true);
            HideItemAction += () => SetMode(false);
        }
        protected virtual void OnDisable()
        {
            ShowItemAction -= () => SetMode(true);
            HideItemAction -= () => SetMode(false);
        }
        private void Start()
        {
            if (_showOnStart)
                ShowItem();
        }
        public void Pick(Player player)
        {
            user = player;
            SetMode(false);
        }

        public bool Use()
        {
            if (Count > 0)
            {
                if (!IsInfinity)
                    Count--;
                if (UseAction != null)
                    UseAction();
            }
            return Count > 0;
        }
        public void Reset()
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