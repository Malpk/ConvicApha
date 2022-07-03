using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TransSphere : Artifact
    {
        [Header("Setting")]
        [SerializeField] private int _unitDistance;
        [SerializeField] private SpriteRenderer _spriteBody;
        [SerializeField] private TransSpherePoint _point;

        private Collider2D _collider;
        private Player _player;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = true;
        }
        public override void Pick(Player player)
        {
            _player = player;
            _spriteBody.enabled = false;
            _collider.enabled = false;
        }

        public override void Use()
        {
            var point = Instantiate(_point.gameObject, _player.transform.position,Quaternion.identity).GetComponent<TransSpherePoint>();
            point.Run(_player.transform, _player.transform.position + 
                    _player.transform.up * _unitDistance);
        }
    }
}