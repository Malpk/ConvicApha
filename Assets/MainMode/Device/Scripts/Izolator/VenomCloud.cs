using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class VenomCloud : DetectDeviceSet
    {
        [Header("Damage Setting")]
        [SerializeField] private DamageInfo _damageInfo;
        [Header("Reference")]
        [SerializeField] private Collider2D _triger;
        [SerializeField] private SpriteRenderer _cloudSprite;

        protected override void Awake()
        {
            base.Awake();
            Hide();
        }

        public void SetMode(bool mode)
        {
            _triger.enabled = mode;
        }
        public void Show()
        {
            _cloudSprite.enabled = true;
        }
        public void Hide()
        {
            _cloudSprite.enabled = false;
        }
        protected override void SendMessange(Player player)
        {
            base.SendMessange(player);
            if (izolator.IsActive)
            {
                player.TakeDamage(0, _damageInfo);
            }
        }
    }
}