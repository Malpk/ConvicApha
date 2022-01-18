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

        public override bool statusWork => true;

        private void Start()
        {

            StartCoroutine(RunAnimation());
        }

        private IEnumerator RunAnimation()
        {
            while (true)
            {
                foreach (var target in _amplituds)
                {
                    var progress = 0f;
                    while (transform.localScale != target)
                    {
                        transform.localScale = Vector3.MoveTowards(transform.localScale, target, _period);
                        yield return null;
                    }
                }
                yield return null;
            }
        }

        protected override void ModeUpdate()
        {
         
        }
    }
}