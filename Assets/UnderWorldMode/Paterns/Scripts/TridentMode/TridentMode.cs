using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Underworld
{
    public class TridentMode : GeneralMode
    {
        [Header("TridentMode Setting")]
        [SerializeField] private int _countPointsInHolder;
        [SerializeField] private TridentPointConfig _config;

        private Coroutine _waitComplite;
        private TridentHolder[] _holders;

        public override bool IsReady => _holders != null;
        public bool IsActive => _waitComplite != null;

        #region Intializate
        private void Awake()
        {
            _holders = GetComponentsInChildren<TridentHolder>();
        }
        public override void Intializate(MapBuilder builder, Player player = null)
        {
        }
        private void Start()
        {
            if (playOnStart)
                Activate();
        }
        #endregion
        #region Work
        public override bool Activate()
        {
#if UNITY_EDITOR
            if (IsActive)
                throw new System.Exception("Patern is already activated");
#endif
            var activeList = new List<TridentHolder>();
            foreach (var holder in _holders)
            {
                holder.Intilizate(_config, _countPointsInHolder);
                holder.CreatePoints();
                holder.Activate(workDuration);
                activeList.Add(holder);
            }
            _waitComplite = StartCoroutine(WaitComplite(activeList));
            return false;
        }
        private IEnumerator WaitComplite(List<TridentHolder> active)
        {
            State = ModeState.Play;
            while (active.Count > 0)
            {
                yield return WaitTime(0.2f);
                var list = new List<TridentHolder>();
                for (int i = 0; i < active.Count; i++)
                {
                    if (active[i].IsActive)
                        list.Add(active[i]);
                }
                active.Clear();
                active = list;
            }
            State = ModeState.Stop;
            _waitComplite = null;
        }
#endregion
    }
}