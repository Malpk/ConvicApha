using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerSpace
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speedMovement = 7f;
        [SerializeField] private float _speedRotation;
        [SerializeField] private PCController _pcCpntroller;

        private IGameController _controller;

        public GameObject DieMenu;

        private bool _isDead;
        private Animator _animator;
        private IMovement _movement;
        private IRotate _rotate;

        private void Awake()
        {
            _controller = _pcCpntroller;
            _isDead = false;
            _animator = GetComponent<Animator>();
            var rigidbody = GetComponent<Rigidbody2D>();
            _movement = new PhysicMovement(rigidbody, _speedMovement);
            _rotate = new PlayerRotate(_speedRotation);
        }

        void Start()
        {
            Time.timeScale = 1;
        }
        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
            if (hit)
            {
                if (hit.transform.TryGetComponent<ITileType>(out ITileType tile))
                    DefineTile(tile);
            }
        }
        private void  DefineTile(ITileType tile)
        {
            switch (tile.tileType)
            {
                case TypeTile.TernTile:
                    Term();
                    return;
                default:
                    break;
            }
        }
        void FixedUpdate()
        {
            if (!_isDead)
            {
                var direction = _controller.MovementInput;
                transform.rotation *= Quaternion.Inverse(_rotate.Rotate(_controller.MovementInput));
                _movement.Move(direction);
            }
            else
            {
                _movement.Move(Vector2.zero);
            }

        }

        public void Term()
        {
            if (_isDead)
                return;
            _isDead = true;
            _animator.SetTrigger("dead");
        }
        public void Dead()
        {
            DieMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}