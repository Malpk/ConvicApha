﻿using System.Collections;
using UnityEngine;
using PlayerSpace;

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

        private void Start()
        {
            StartCoroutine(Work());
        }
        protected override IEnumerator Work()
        {
            yield return new WaitForSeconds(_warningTime);
            _animator.SetInteger("State", 1);
            _fireInstiate = InstatiateFire(_fire);
            _state = TernState.Fire;
            yield return new WaitUntil(() => (_fireInstiate == null));
            Destroy(gameObject);
        }

        protected override void Damage(Player player)
        {
            if (_state == TernState.Fire)
                player.Term();
        }
    }
}