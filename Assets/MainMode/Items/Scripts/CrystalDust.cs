using System;
using UnityEngine;

namespace MainMode.Items
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CrystalDust : Artifact
    {
        [SerializeField] private MapGrid _map;
        [SerializeField] private CrystalSheild _wallCrystalPrefab;
        [SerializeField] private SpriteRenderer _sprite;

        private Player _player;
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }
        public override void Pick(Player player)
        {
            _player = player;
            _sprite.enabled = false;
            _collider.enabled = false;
        }

        public override void Use()
        {
            //Instantiate(_wallCrystalPrefab.gameObject,
            //    _map.SearchPoint(_player.Position + (Vector2)_player.transform.up), Quaternion.identity);
            Instantiate(_wallCrystalPrefab.gameObject,
            _player.Position + (Vector2)_player.transform.up, Quaternion.identity);
        }
    }
}
