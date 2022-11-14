using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class DefoutCompliteState : IPatternState
    {
        private readonly float delay;
        private readonly PoolTerm pool;

        private float _progres;

        private List<AutoTerm> _terms = new List<AutoTerm>();

        public DefoutCompliteState(PoolTerm pool,float delay)
        {
            this.delay = delay;
            this.pool = pool;
        }

        public bool IsComplite => _terms.Count == 0;

        public void Start()
        {
            _progres = 0f;
            _terms.AddRange(pool.Active);
        }

        public bool Update()
        {
            _progres += Time.deltaTime / delay;
            if (_progres >= 1f)
            {
                _terms = GetActiveTerms(_terms);
            }
            return _terms.Count > 0;
        }

        private List<AutoTerm> GetActiveTerms(List<AutoTerm> terms)
        {
            var list = new List<AutoTerm>();
            for (int i = 0; i < terms.Count; i++)
            {
                if (terms[i].IsShow)
                {
                    list.Add(_terms[i]);
                }
            }
            return list;
        }
        public bool SwitchState(out IPatternState nextState)
        {
            nextState = default(IPatternState);
            return false;
        }

    }
}