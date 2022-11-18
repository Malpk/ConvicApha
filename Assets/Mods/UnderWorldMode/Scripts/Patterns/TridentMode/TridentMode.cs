using System.Collections;
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
        private void Start()
        {
            if (playOnStart)
                Play();
        }
        #endregion
        #region Work
        protected override void PlayMode()
        {
            var activeList = new List<TridentHolder>();
            foreach (var holder in _holders)
            {
                holder.Activate(workDuration);
                activeList.Add(holder);
            }
            //_waitComplite = StartCoroutine(WaitComplite(activeList));
        }

        protected override void StopMode()
        {
            throw new System.NotImplementedException();
        }
        //private IEnumerator WaitComplite(List<TridentHolder> active)
        //{
        //    while (active.Count > 0)
        //    {
        //        yield return WaitTime(0.2f);
        //        var list = new List<TridentHolder>();
        //        for (int i = 0; i < active.Count; i++)
        //        {
        //            if (active[i].IsActive)
        //                list.Add(active[i]);
        //        }
        //        active.Clear();
        //        active = list;
        //    }
        //    _waitComplite = null;
        //}
        #endregion
    }
}