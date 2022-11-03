using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Underworld
{
    public abstract class TotalMapMode : GeneralMode
    {
        [SerializeField] protected Player player;
        [SerializeField] private MapBuilder _builder;

        protected Term[,] terms;

        private void Awake()
        {
            if (_builder)
            {
                terms = _builder.Terms;
            }
        }

        protected virtual void Start()
        {
            if (playOnStart)
                Activate();
        }

        public override void Intializate(MapBuilder builder, Player player)
        {
            this.player = player;
            _builder = builder;
            terms = _builder.Terms;
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
            var deactive = new List<Term>();
            foreach (var term in terms)
            {
                if (term.IsActive)
                {
                    term.Deactivate(true);
                    deactive.Add(term);
                }
            }
            yield return TrakingDeactiveTerms(deactive);
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
            var list = new List<Term>();
            foreach (var term in terms)
            {
                if (term.IsShow)
                {
                    term.HideItem();
                    list.Add(term);
                }
            }
            yield return TrakinHideMap(list);
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
                    activeTerms[i].HideItem();
                }
            }
            return list;
        }
        #endregion
    }
}
