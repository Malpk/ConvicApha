using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(Animator))]
    public class RingMode : GameMode
    {
        [Header("Game Setting")]
        [SerializeField] private int _countPeriods;
        [SerializeField] private float _delay;

        private int _count = 0;
        private Animator _animator;

        public override bool statusWork => _count < _countPeriods;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void SwitchEvent()
        {
            if (statusWork)
                StartCoroutine(AnimationSwitch());
        }
        private IEnumerator AnimationSwitch()
        {
            yield return new WaitForSeconds(_delay);
            _animator.SetTrigger("next");
            _count++;
        }
    }
}