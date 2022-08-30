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
        [SerializeField] private MapBuilder _builder;
        [SerializeField] private Term _handTermPerfab;

        protected Term[,] termArray;
        protected List<Term> terms = new List<Term>();

        private bool _isCreate;

        public override bool IsReady => _isCreate;

        protected virtual void Start()
        {
            StartCoroutine(CreateMap());
            if (playOnStart)
                Activate();
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
        #region Deactive Term
        protected IEnumerator WaitDeactivateMap()
        {
            for (int i = 0; i < terms.Count; i++)
            {
                if(terms[i].IsActive)
                    terms[i].Deactivate(true);
            }
            yield return TrakingDeactiveTerms(terms);
        }
        protected IEnumerator TrakingDeactiveTerms(List<Term> activeTerms)
        {
            while (activeTerms.Count > 0)
            {
                yield return WaitTime(0.2f);
                activeTerms = GetActiveTerm(activeTerms);
            }
        }
        protected IEnumerator WaitHideMap()
        {
            yield return WaitDeactivateMap();
            for (int i = 0; i < terms.Count; i++)
            {
                if(terms[i].IsShow)
                    terms[i].HideItem();
            }
            yield return TrakinHideMap(terms);
        }
        #endregion
        #region Hide Term
        protected List<Term> GetActiveTerm(List<Term> activeTerms)
        {
            var list = new List<Term>();
            for (int i = 0; i < activeTerms.Count; i++)
            {
                if (activeTerms[i].IsDamageMode)
                {
                    list.Add(activeTerms[i]);
                }
            }
            return list;
        }
        protected IEnumerator TrakinHideMap(List<Term> activeTerms)
        {
            while (activeTerms.Count > 0)
            {
                yield return WaitTime(0.2f);
                activeTerms = GetShowTerm(activeTerms);
            }
        }
        protected List<Term> GetShowTerm(List<Term> activeTerms)
        {
            var list = new List<Term>();
            for (int i = 0; i < activeTerms.Count; i++)
            {
                if (activeTerms[i].IsShow)
                {
                    list.Add(activeTerms[i]);
                }
                else
                {
                    Destroy(activeTerms[i].gameObject);
                }
            }
            return list;
        }
        #endregion
        protected IEnumerator CreateMap()
        {
            var points = _builder.Points;
            termArray = new Term[points.GetLength(0), points.GetLength(1)];
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    var term = Instantiate(_handTermPerfab.gameObject).GetComponent<Term>();
                    term.transform.parent = _builder.transform;
                    termArray[i, j] = term;
                    terms.Add(termArray[i, j]);
                    points[i, j].SetItem(termArray[i, j]);
                }
                yield return null;
            }
            _isCreate = true;
        }
    }
}
