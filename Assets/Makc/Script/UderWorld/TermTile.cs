using System.Collections;
using UnityEngine;
using PlayerComponent;

namespace Underworld
{
    [RequireComponent(typeof(Collider2D))]
    public class TermTile : TernBase
    {
        [Header("Game Setting")]
        [SerializeField] private float _warningTime;

        [Header("Pefab Setting")]
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _fire;

        private TernState _state = TernState.Warning;
        private GameObject _fireInstiate;

        public override TernState state => _state;

        private void Start()
        {
            StartCoroutine(Work());
        }
        protected override IEnumerator Work()
        {
            yield return new WaitForSeconds(_warningTime);
            _animator.SetInteger("State", 1);
            _fireInstiate = InstatiateFire(_fire);
            yield return new WaitForSeconds(0.1f);
            _state = TernState.Fire;
            yield return new WaitUntil(() => (_fireInstiate == null));
            Destroy(gameObject);
        }

        protected override void Damage(Player player)
        {
            if (_state == TernState.Fire)
            {
                player.Incineration();
            }
        }

        protected override void Intializate()
        {
        }
    }
}