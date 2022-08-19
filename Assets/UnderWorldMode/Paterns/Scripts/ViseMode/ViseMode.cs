using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Underworld
{
    public class ViseMode : GeneralMode
    {
        [SerializeField] private float _warningTime;
        [Header("Mods Setting")]
        [SerializeField] private float _minTimeOffset;
        [SerializeField] private float _maxTimeOffset;
        [Header("Reference")]
        [SerializeField] private MapBuilder _builder;
        [AssetReferenceUILabelRestriction("term")]
        [SerializeField] private AssetReferenceGameObject _handTernAsset;

        private bool _isReady = false;
        private HandTermTile _handTermPerfab;

        private ViseTypeWork[] _workTypeVise = new ViseTypeWork[] { ViseTypeWork.Vertical, ViseTypeWork.Horizontal };

        private List<Vise> _vises = new List<Vise>();
        private List<Coroutine> _runMods = new List<Coroutine>();

        public override bool IsReady => _isReady && _builder && _vises.Count > 0;

        protected async override Task<bool> LoadAsync()
        {
            if (!IsReady)
            {
                var load = _handTernAsset.LoadAssetAsync().Task;
                await load;
                if (load.Result.TryGetComponent(out HandTermTile term))
                    throw new System.NullReferenceException("GameObject is not component HandTermTile");
                _handTermPerfab = term;
                _isReady = true;
                return true;
            }
            return false;
        }
        protected override void Unload()
        {
            _handTernAsset.ReleaseAsset();
            _isReady = false;
        }
        #region Intilizate
        public override void Intializate(MapBuilder builder, Player player)
        {
            _builder = builder;
            if (_vises.Count > 0)
                DeleteVise();
            foreach (var work in _workTypeVise)
            {
                _vises.Add(GetVise(_handTermPerfab, work));
            }
        }
        private Vise GetVise(HandTermTile handterm, ViseTypeWork type)
        {
            var vise = GetHolder("ViseHolder").gameObject.AddComponent<Vise>();
            vise.CreateVise(handterm.gameObject, _builder.Points, type);
            return vise;
        }
        private Transform GetHolder(string name)
        {
            var holder = new GameObject(name).transform;
            holder.parent = transform;
            return holder;
        }
        private void DeleteVise()
        {
            foreach (var vise in _vises)
            {
                Destroy(vise.gameObject);
            }
            _vises.Clear();
        }
        #endregion
        #region Work Vise
        public override bool Activate()
        {
            if (_runMods.Count == 0)
            {
                foreach (var vise in _vises)
                {
                    _runMods.Add(StartCoroutine(MoveVise(vise, Random.Range(_minTimeOffset, _maxTimeOffset))));
                }
                return true;
            }
            return false;
        }

        private IEnumerator MoveVise(Vise vise,float timeOffset)
        {
            while (vise.IsMoveVise && State == ModeState.Stop)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                yield return StartCoroutine(vise.Deactivate());
                yield return WaitTime(_warningTime);
                vise.Activate();
                yield return WaitTime(timeOffset * 2);
            }
            _runMods.Remove(_runMods[0]);
            if (_runMods.Count == 0)
                State = ModeState.Stop;
            yield return null;
        }
        #endregion
        public override void Pause()
        {
            base.Pause();
            foreach (var vise in _vises)
            {
                vise.Pause();
            }
        }
        public override void UnPause()
        {
            base.UnPause();
            foreach (var vise in _vises)
            {
                vise.UnPause();
            }
        }
    }
}
