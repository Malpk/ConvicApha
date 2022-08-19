using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class DefautTermTile : TermTile
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] private float _warningTime;
        [SerializeField] private float _timeActive;

        private Coroutine _run;

        private void Start()
        {
            if (_playOnStart)
                Activate();
        }
        public void Activate()
        {
            if (_run == null)
            {
                _run = StartCoroutine(Run());
            }
        }
        private IEnumerator Run()
        {
            yield return WaitTime(_warningTime);
            fire.Activate(FireState.Start);
            tileAnimator.SetBool("Activate", true);
            yield return WaitTime(_timeActive);
            Deactivate();
            yield return new WaitWhile(()=> isDangerMode);
            OffItem();
            _run = null;
        }

        private IEnumerator WaitTime(float waitTime)
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return new WaitWhile(() => isPause);
                yield return null;
                progress += Time.deltaTime / waitTime;
            }
        }
    }
}