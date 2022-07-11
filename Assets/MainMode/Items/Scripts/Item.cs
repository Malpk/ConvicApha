using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{   
    [RequireComponent(typeof(Collider2D))]
    public abstract class Item : MonoBehaviour, IPickable, IUseable
    {
        [SerializeField] protected Sprite ItemSprite;
        [SerializeField] protected SpriteRenderer _spriteBody;

        private Collider2D _collider;

        public bool Active { get; private set; }

        protected Player _target;
        public Sprite Sprite => ItemSprite;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
 
            _collider.isTrigger = true;
            SetMode(true);
        }
        public void Pick(Player player)
        {
            _target = player;
            SetMode(false);
        }

        public abstract void Use();


        public void SetMode(bool mode)
        {
            _spriteBody.enabled = mode;
            _collider.enabled = mode;
            Active = mode;
        }
    }
}