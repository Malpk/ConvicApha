using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public sealed class AutoTerm : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] private float _safeTime;
        [SerializeField] private float _activeTime;
        [Header("Reference")]
        [SerializeField] private Term _term;

        public bool IsShow => _term.IsShow;

        private void Start()
        {
            if (_playOnStart)
            {
                StartAutoMode();
            }
        }
        public void StartAutoMode()
        {
            if (!_term.IsShow)
            {
                _term.Show();
                StartCoroutine(AutoMode());
            }
        }
        private IEnumerator AutoMode()
        {
            yield return WaitTime(_safeTime);
            _term.Activate();
            yield return WaitTime(_activeTime);
            _term.Deactivate();
            yield return new WaitWhile(()=> _term.IsActive);
            _term.Hide();
        }
        private IEnumerator WaitTime(float waitTime)
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return null;
                progress += Time.deltaTime / waitTime;
            }
        }
    }
}