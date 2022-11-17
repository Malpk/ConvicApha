using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TotalMapCompliteState : IPatternState
    {
        private readonly float cheakDelay;
        private readonly Term[,] terms;

        private float progress = 0f;
        private List<Term> _termActive;

        public System.Action OnComplite;

        public TotalMapCompliteState(Term[,] terms, float cheakDelay)
        {
            this.terms = terms;
            this.cheakDelay = cheakDelay;
        }

        public bool IsComplite => _termActive.Count == 0;

        public void Start()
        {
            progress = 0f;
            _termActive = new List<Term>();
            foreach (var term in terms)
            {
                if (term.IsActive)
                {
                    term.Deactivate();
                    _termActive.Add(term);
                }
            }
        }

        public bool Update()
        {
            progress += Time.deltaTime / cheakDelay;
            if (progress >= 1)
            {
                progress = 0f;
                _termActive = GetActiveTerm(_termActive);
            }
            return _termActive.Count > 0; 
        }
        public bool SwitchState(out IPatternState nextState)
        {
            nextState = default(IPatternState);
            return false;
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