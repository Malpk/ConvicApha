using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ToolRepairs : MonoBehaviour,IMapItem
    {
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private SpriteRenderer _spriteBody;

        private Collider2D _triger;

        public Sprite Icon => _itemIcon;

        private void Awake()
        {
            _triger = GetComponent<Collider2D>();
            _triger.isTrigger = true;
        }

        public void Pick()
        {
            SetMode(false);
        }

        public void SetMode(bool mode)
        {
            _spriteBody.enabled = mode;
            _triger.enabled = mode;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}