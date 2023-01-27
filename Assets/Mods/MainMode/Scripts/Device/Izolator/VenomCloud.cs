using UnityEngine;

namespace MainMode
{
    public class VenomCloud : MonoBehaviour
    {
        [Header("Damage Setting")]
        [SerializeField] private DamageInfo _damageInfo;
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _cloudSprite;

        protected void Awake()
        {
            Hide();
        }
        public void Show()
        {
            _cloudSprite.enabled = true;
        }
        public void Hide()
        {
            _cloudSprite.enabled = false;
        }

        public void SetDamage(Player player)
        {
            if (player && _cloudSprite.enabled)
            {
                player.TakeDamage(0, _damageInfo);
            }
        }
    }
}