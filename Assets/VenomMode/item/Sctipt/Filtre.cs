using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Filtre : MonoBehaviour
    {
        [SerializeField] private int _filtrationTime;
        [SerializeField] private SpriteRenderer _fitre;

        private BoxCollider2D _collider;

        public int FiltrationTime => _filtrationTime;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = true;
        }

        public void Spawn(Vector2 position)
        {
            transform.position = position;
            SetMode(true);
        }
        private void SetMode(bool mode)
        {
            _fitre.enabled = mode;
            _collider.enabled = mode;
        }

        public void Pick()
        {
            SetMode(false);
        }
    }
}