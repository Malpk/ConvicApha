using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TridentMode : GeneralMode
    {
        [Header("TridentMode Setting")]
        [SerializeField] private int _countPointsInHolder;
        [SerializeField] private TridentPointConfig _config;

        private Coroutine _waitComplite;
        private TridentHolder[] _holders;

        public bool IsActive => _waitComplite != null;

        #region Intializate
        private void Awake()
        {
            _holders = GetComponentsInChildren<TridentHolder>();
            foreach (var holder in _holders)
            {
                holder.Intilizate(_config, _countPointsInHolder);
                holder.CreatePoints();
            }
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is TridentModeConfig tridentModeConfig)
            {
                workDuration = tridentModeConfig.WorkDuration;
                _config = tridentModeConfig.PointConfig;
            }
            else
            {
                throw new System.NullReferenceException("TridentModeConfig is null");
            }
        }
        public override void Intializate(MapBuilder builder, Player player = null)
        {
        }
        private void OnEnable()
        {
            foreach (var holder in _holders)
            {
                holder.OnComplite += Compliting;
            }
        }
        private void OnDisable()
        {
            foreach (var holder in _holders)
            {
                holder.OnComplite -= Compliting;
            }
        }
        private void Start()
        {
            if (playOnStart)
                Play();
        }
        #endregion
        #region Work
        protected override void PlayMode()
        {
            enabled = true;
            var activeList = new List<TridentHolder>();
            foreach (var holder in _holders)
            {
                holder.Activate(workDuration);
                activeList.Add(holder);
            }
        }

        protected override void StopMode()
        {
        }
        private void Compliting()
        {
            foreach (var holder in _holders)
            {
                if (holder.IsActive)
                    return;
            }
            Stop();
        }
        #endregion
    }
}