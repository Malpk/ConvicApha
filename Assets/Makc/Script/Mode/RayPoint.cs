using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class RayPoint : GameMode
    {
        [Header("Game Setting")]
        [SerializeField] private float _period;
        [SerializeField] private Vector3[] _amplituds;

        private Coroutine _coroutine = null;

        public override bool statusWork => true;

        public bool StartScaleAnimation()
        {
            if (_coroutine != null)
                return false;
            _coroutine = StartCoroutine(RunAnimation());
            return true;
        }

        private IEnumerator RunAnimation()
        {
            while (true)
            {
                foreach (var target in _amplituds)
                {
                    while (transform.localScale != target)
                    {
                        transform.localScale = Vector3.MoveTowards(transform.localScale, target, _period);
                        yield return null;
                    }
                }
                yield return null;
            }
        }
    }
}