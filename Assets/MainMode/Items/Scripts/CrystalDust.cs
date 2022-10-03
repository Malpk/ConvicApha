using UnityEngine;

namespace MainMode.Items
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CrystalDust : ConsumablesItem
    {
        [SerializeField] private CrystalSheild _wallCrystalPrefab;
        [SerializeField] private SpriteRenderer _sprite;

        private CircleCollider2D _collider;

        public override void Use()
        {
            Instantiate(_wallCrystalPrefab.gameObject,
            user.Position + (Vector2)user.transform.up, Quaternion.identity);
        }
    }
}
