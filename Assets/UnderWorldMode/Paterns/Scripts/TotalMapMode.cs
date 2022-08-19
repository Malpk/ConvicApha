using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Underworld
{
    public abstract class TotalMapMode : GeneralMode
    {
        [SerializeField] protected Player player;
        [AssetReferenceUILabelRestriction("term")]
        [SerializeField] protected AssetReferenceGameObject handTermAsset;

        [SerializeField]private MapBuilder _builder;

        private HandTermTile _handTerm;

        protected HandTermTile[,] termArray;
        protected List<HandTermTile> terms = new List<HandTermTile>();

        private bool _isLoad;
        private bool _isCreate;

        public override bool IsReady => _isLoad && _isCreate;

        protected virtual void Start()
        {
            StartCoroutine(CreateMap());
        }

        protected async override Task<bool> LoadAsync()
        {
            if (handTermAsset == null)
                throw new System.NullReferenceException();
            if (!_handTerm)
            {
                var load = handTermAsset.LoadAssetAsync().Task;
                await load;
                if (load.Result.TryGetComponent(out HandTermTile term))
                {
                    _handTerm = term;
                    _isLoad = true;
                    return true;
                }
                else
                {
                    throw new System.NullReferenceException("Gameobject is not component HandTermTile");
                }
            }
            return false;
        }
        protected override void Unload()
        {
            handTermAsset.ReleaseAsset();
            _isLoad = false;
        }
        public override void Intializate(MapBuilder builder, Player player)
        {
            this.player = player;
            _builder = builder;

        }
        public override void Pause()
        {
            base.Pause();
            foreach (var term in terms)
            {
                term.Pause();
            }
        }
        public override void UnPause()
        {
            base.UnPause();
            foreach (var term in terms)
            {
                term.UnPause();
            }
        }
        protected void ActivateMap(FireState state)
        {
            foreach (var term in terms)
            {
                term.Activate(state);
            }
        }
        protected void DeactivateMap(out HandTermTile compliteTerm)
        {
            compliteTerm = terms[0];
            for (int i = 0; i < terms.Count; i++)
            {
                terms[i].SetMode(false);
                if (terms[i].IsActive)
                    compliteTerm = terms[i];
            }
        }
        protected IEnumerator CreateMap()
        {
            yield return new WaitWhile(() => !_isLoad);
            var points = _builder.Points;
            termArray = new HandTermTile[points.GetLength(0), points.GetLength(1)];
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    var term = Instantiate(_handTerm.gameObject).GetComponent<HandTermTile>();
                    term.transform.parent = transform.parent;
                    termArray[i, j] = term;
                    term.Deactivate(false);
                    terms.Add(termArray[i, j]);
                    points[i, j].SetItem(termArray[i, j]);
                }
                yield return null;
            }
            _isCreate = true;
        }
    }
}
