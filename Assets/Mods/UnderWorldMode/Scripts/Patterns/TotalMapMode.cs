using System.Collections.Generic;
using UnityEngine;


namespace Underworld
{
    public abstract class TotalMapMode : GeneralMode
    {
        [SerializeField] protected Player player;
        [SerializeField] private MapBuilder _mapBuilder;

        protected Term[,] terms;
        protected TotalMapCompliteState compliteState = new TotalMapCompliteState(0.2f);
        
        private List<Term> _activeTerms = new List<Term>();

        protected virtual void Awake()
        {
            if (_mapBuilder)
            {
                terms = _mapBuilder.Terms;
            }
        }
        protected virtual void OnEnable()
        {
            compliteState.OnCheakComplite += CheakComplite;
        }
        protected virtual void OnDisable()
        {
            compliteState.OnCheakComplite -= CheakComplite;
        }
        protected void Start()
        {
            if (playOnStart)
                Play();
        }
        public override void Intializate(MapBuilder builder, Player player)
        {
            this.player = player;
            _mapBuilder = builder;
            terms = _mapBuilder.Terms;
        }
        protected void ActivateTerms()
        {
            foreach (var term in terms)
            {
                term.Activate();
            }
        }
        protected void ActivateTerms(List<Term> terms)
        {
            foreach (var term in terms)
            {
                term.Show();
                term.Activate();
            }
        }
        protected void DeactivateTerms()
        {
            _activeTerms.Clear();
            foreach (var term in terms)
            {
                if (term.IsActive)
                {
                    term.Deactivate();
                    _activeTerms.Add(term);
                }
                else if (term.IsShow)
                {
                    term.Hide();
                }
            }
        }
        private bool CheakComplite()
        {
            _activeTerms = GetActiveTerm(_activeTerms);
            return _activeTerms.Count == 0;
        }
        private List<Term> GetActiveTerm(List<Term> terms)
        {
            var list = new List<Term>();
            for (int i = 0; i < terms.Count; i++)
            {
                if (terms[i].IsActive)
                {
                    list.Add(terms[i]);
                }
                else
                {
                    terms[i].Hide();
                }
            }
            return list;
        }
    }
}
