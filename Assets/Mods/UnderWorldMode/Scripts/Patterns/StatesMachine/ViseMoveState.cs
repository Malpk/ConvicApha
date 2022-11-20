using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class ViseMoveState
    {
        private readonly ViseV2 vise;

        private float _warningTime;
        private float _activeTime;

        private float _progressWarning = 0f;
        private float _progressActive = 0f;

        private System.Action _task;

        public event System.Action OnComplite;

        public ViseMoveState(ViseV2 vise)
        {
            this.vise = vise;
        }

        public bool IsComplite => vise.IsComplite;

        public void SetMap(Term[,] terms)
        {
            vise.Intializate(terms);
        }

        public void Intializate(float warningTime, float activeTime)
        {
            _warningTime = warningTime;
            _activeTime = activeTime;
        }

        public void Start()
        {
            _progressWarning = 0;
            _progressActive = 0;
            vise.Reset();
            Move();
            _task = WarningUpdate;
        }

        public bool Update()
        {
            _task();
            return !vise.IsComplite;
        }

        private void WarningUpdate()
        {
            _progressWarning += Time.deltaTime / _warningTime;
            if (_progressWarning >= 1)
            {
                _progressWarning = 0f;
                vise.ActivateVise();
                _task = ActiveUpdate;
            }
        }
        private void ActiveUpdate()
        {
            _progressActive += Time.deltaTime / _activeTime;
            if (_progressActive >= 1)
            {
                vise.HideVise();
                Move();
                _progressActive = 0;
                _task = WarningUpdate;
            }
        }
        private void Move()
        {
            if (vise.Next())
                vise.ShowVise();
            else
                OnComplite?.Invoke();
        }

    }
}
