
using UnityEngine;

namespace MainMode.Items
{   
    [RequireComponent(typeof(Collider2D))]
    public abstract class Item : MonoBehaviour, IPickable, IUseable
    {
        [Header("Setting")]
        [SerializeField] private bool _showOnStart;
        [SerializeField] private bool _isShoot;
        [Header("Reference")]
        [SerializeField] protected Sprite ItemSprite;
        [SerializeField] protected SpriteRenderer _spriteBody;

        private Collider2D _collider;

        public bool Active { get; private set; }

        protected Player user;

        protected System.Action ResetAction;

        public Sprite Sprite => ItemSprite;

        public bool IsShoot => _isShoot;
        public abstract bool IsUse { get; } 
        public abstract string Name { get; }

        private void Start()
        {
            if (_showOnStart)
                ShowItem();
            else
                HideItem();
        }
        public void Pick(Player player)
        {
            user = player;
            HideItem();
        }

        public abstract bool Use();
        public abstract void ResetState();
        public void ShowItem()
        {
            gameObject.SetActive(true);
        }
        public void HideItem()
        {
            gameObject.SetActive(false);
        }
    }
}