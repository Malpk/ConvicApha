using System.Collections;
using UnityEngine;
using PlayerSpace;
using Underworld;

    public class TTile : MonoBehaviour
    {
        [Header("Game Setting")]
        [SerializeField] private TileSettimg _fire;
        [Header("Component Setting")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _firePosition;

        private TernState _state = TernState.Warning;

        private void Start()
        {
            StartCoroutine(Fire());
        }

        private IEnumerator Fire()
        {
            while (true)
            {
                _animator.SetInteger("State", 1);
                var fire = Instantiate(_fire.tile, transform);
                fire.transform.parent = transform;
                fire.transform.localPosition = _firePosition;
                _state = TernState.Fire;
                yield return new WaitUntil(() => (fire == null));
                _state = TernState.Warning;
                _animator.SetInteger("State", 0);
                yield return new WaitForSeconds(_fire.timeActive);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<Player>();
            if (player != null && _state == TernState.Fire)
                player.Term();
        }
    }
