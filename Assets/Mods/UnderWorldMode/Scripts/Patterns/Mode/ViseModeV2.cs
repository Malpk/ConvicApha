using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class ViseModeV2 : TotalMapMode
    {
        [Header("General")]
        [SerializeField] private Vector2 _activeTime;
        [SerializeField] private Vector2 _warningTime;
        [SerializeField] private ViseState _mode = ViseState.HorizontalMode;

        private List<ViseMoveState> _vises = new List<ViseMoveState>();

        protected override void Awake()
        {
            base.Awake();
            foreach (var orientation in GetViseState(_mode))
            {
                _vises.Add(new ViseMoveState(
                    new ViseV2(orientation)));
            }
            enabled = false;
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is ViseModeConfig viseModeConfig)
            {
                workDuration = viseModeConfig.WorkDuration;
                _activeTime = viseModeConfig.ActiveTime;
                _warningTime = viseModeConfig.WarningTime;
            }
            else
            {
                throw new System.NullReferenceException("ViseModeConfig is null");
            }
        }
        private ViseState[] GetViseState(ViseState state)
        {
            if (_mode == ViseState.GeneralMode)
            {
                return new ViseState[] {
                    ViseState.HorizontalMode, ViseState.VerticalMode };
            }
            else
            {
                return new ViseState[] { _mode };
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            foreach (var vise in _vises)
            {
                vise.OnComplite += Compliting;
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var vise in _vises)
            {
                vise.OnComplite -= Compliting;
            }
        }
        protected override void PlayMode()
        {
            for (int i = 0; i < _vises.Count; i++)
            {
                var timeWarning = Random.Range(_warningTime.x, _warningTime.y);
                var timeActive = Random.Range(_activeTime.x, _activeTime.y);
                _vises[i].Intializate(timeWarning, timeActive);
                _vises[i].SetMap(terms);
                _vises[i].Start();
            }
            enabled = true;
        }
        protected override void StopMode()
        {
            enabled = false;
        }
        private void Update()
        {
            for (int i = 0; i < _vises.Count; i++)
            {
                if(!_vises[i].IsComplite)
                    _vises[i].Update();
            }
        }

        private void Compliting()
        {
            foreach (var vise in _vises)
            {
                if (!vise.IsComplite)
                    return;
            }
            Stop();
        }
    }
    public enum ViseState
    {
        GeneralMode,
        VerticalMode,
        HorizontalMode
    }
}
