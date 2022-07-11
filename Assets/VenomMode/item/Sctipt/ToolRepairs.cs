using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ToolRepairs : MonoBehaviour
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
            _spriteBody.enabled = false;
            _triger.enabled = false;
        }
    }
}