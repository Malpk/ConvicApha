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
        [SerializeField] private int _countTrident;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _shootDelay;
        [Min(0)]
        [SerializeField] private float _groupShootDelay;
        [SerializeField] private float _warningTime;
        [Header("Reference")]
        [SerializeField] private TridentSetting _tridentSetting;
        [SerializeField] private TridentHolder[] _tridentHolders;
        [AssetReferenceUILabelRestriction("underworld")]
        [SerializeField] private AssetReferenceGameObject _tridentAsset;

        private List<Coroutine> _countCorotine = new List<Coroutine>();
        private bool _isReady = false;

        private Trident _tridentPerfab;
        private Coroutine _runMode;


        public override bool IsReady => _isReady;

        protected override void Awake()
        {
            base.Awake();
        }
        #region Intializate
        public override void Intializate(MapBuilder builder, Player player = null)
        {
            _countTrident = builder.Points.GetLength(0);
        }
        private async Task Intializate()
        {
            if (IsReady)
            {
                var tasks = new List<Task>();
                foreach (var holders in _tridentHolders)
                {
                    tasks.Add(holders.IntializateAsync(_tridentPerfab, _tridentSetting, _countTrident));
                }
                await Task.WhenAll(tasks);
            }
        }
        #endregion
        #region Load Data
        protected async override Task<bool> LoadAsync()
        {
            var loadTrident = _tridentAsset.LoadAssetAsync().Task;
            await loadTrident;
            if (!loadTrident.Result.TryGetComponent(out Trident trident))
                throw new System.NullReferenceException("GameObjec is not component Trident");
            _tridentPerfab = trident;
            _isReady = true;
            return true;
        }

        protected override void Unload()
        {
            _tridentAsset.ReleaseAsset();
            _isReady = false;
        }
        #endregion
        #region Work
        public override bool Activate()
        {
            if (_runMode == null)
            {
                State = ModeState.Play;
                foreach (var holder in _tridentHolders)
                {
                    _countCorotine.Add(StartCoroutine(ShootTrident(holder, _warningTime)));
                }
            }
            return false;
        }
        private IEnumerator ShootTrident(TridentHolder listTrident, float warningTime = 0)
        {
            var task = Intializate();
            yield return WaitTime(warningTime);
            yield return new WaitWhile(() => !task.IsCompleted);
            float progress = 0f;
            while (progress < 1f)
            {
                if (listTrident.GetGroup(out Trident[] tridents))
                {
                    foreach (var trident in tridents)
                    {
                        trident.Activate();
                        progress += _shootDelay / workDuration;
                        yield return new WaitForSeconds(_shootDelay);
                    }
                }
                progress += _groupShootDelay / workDuration;
                yield return new WaitForSeconds(_groupShootDelay);
            }
            _countCorotine.Remove(_countCorotine[0]);
            if (_countCorotine.Count == 0)
            {
                State = ModeState.Stop;
            }
        }
        #endregion
    }
}