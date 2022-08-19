using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ToolRepairs : SpawnItem
    {
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private SpriteRenderer _spriteBody;

        private bool _isMode;
        private Collider2D _triger;

        public Sprite Icon => _itemIcon;

        public override bool IsShow => _isMode;

        private void Awake()
        {
            _triger = GetComponent<Collider2D>();
            _triger.isTrigger = true;
        }

        public void Pick()
        {
            SetMode(false);
        }

        public override void SetMode(bool mode)
        {
            _isMode = mode;
            _spriteBody.enabled = mode;
            _triger.enabled = mode;
        }

        public override void OffItem()
        {
            Destroy(gameObject);
        }
    }
}