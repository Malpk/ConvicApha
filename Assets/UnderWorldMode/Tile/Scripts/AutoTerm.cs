using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public sealed class AutoTerm : SmartItem,IPause
    {
        [Header("General Setting")]
        [SerializeField] private float _safeTime;
        [SerializeField] private float _activeTime;
        [Header("Reference")]
        [SerializeField] private Term _term;

        private bool _isPause = false;
        private Coroutine _autoMode;

        private void OnEnable()
        {
            ShowItemAction += ShowTerm;
            HideItemAction += HideTerm;
        }
        private void OnDisable()
        {
            ShowItemAction -= ShowTerm;
            HideItemAction -= HideTerm;
        }
        private void ShowTerm()
        {
            _term.ShowItem();
        }
        private void HideTerm()
        {
            if (_autoMode != null)
#if UNITY_EDITOR
                throw new System.Exception("You can't hide an while run AutoMode");
#endif
            _term.HideItem();
        }
        public void StartAutoMode()
        {
#if UNITY_EDITOR
            if (_autoMode != null)
                throw new System.Exception("Term is already start autoMode");
#endif
            _autoMode = StartCoroutine(AutoMode());
        }
        public void Pause()
        {
            _isPause = true;
            _term.Pause();
        }

        public void UnPause()
        {
            _isPause = false;
            _term.UnPause(); ;
        }

        private IEnumerator AutoMode()
        {
            yield return WaitTime(_safeTime);
            _term.Activate();
            yield return WaitTime(_activeTime);
            _term.Deactivate();
            yield return _term.HideByDeactivation();
            _autoMode = null;
            HideTerm();
        }
        private IEnumerator WaitTime(float waitTime)
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return new WaitWhile(() => _isPause);
                yield return null;
                progress += Time.deltaTime / waitTime;
            }
        }
    }
}