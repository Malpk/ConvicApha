using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class PoolTerm
    {
        private readonly GameObject prefab;

        private List<AutoTerm> _poolActive = new List<AutoTerm>();
        private List<AutoTerm> _poolDeactive = new List<AutoTerm>();

        public IReadOnlyList<AutoTerm> Active => _poolActive;

        public PoolTerm(AutoTerm prefab)
        {
            this.prefab = prefab.gameObject;
        }

        public AutoTerm GetTerm(Transform parent)
        {
            PoolUpdate();
            return GetDeacitveTerm(out AutoTerm term) ? term : Create(parent);
        }
        public void ClearPool()
        {
            ClearPool(_poolActive);
            ClearPool(_poolDeactive);
        }
        private AutoTerm Create(Transform parent)
        {
            var term = Object.Instantiate(prefab).GetComponent<AutoTerm>();
            term.transform.parent = parent;
            _poolActive.Add(term);
            return term;
        }

        private bool GetDeacitveTerm(out AutoTerm term)
        {
            term = null;
            if (_poolDeactive.Count > 0)
            {
                term = _poolDeactive[0];
                _poolDeactive.Remove(term);
                _poolActive.Add(term);
            }
            return term;
        }

        private void PoolUpdate()
        {
            UpdateActiveePool();
            UpdateDeactivePool();
        }
        private void UpdateDeactivePool()
        {
            var deactiveTerms = new List<AutoTerm>();
            for (int i = 0; i < _poolActive.Count; i++)
            {
                if (!_poolActive[i].IsShow)
                {
                    deactiveTerms.Add(_poolActive[i]);
                }
            }
            for (int i = 0; i < deactiveTerms.Count; i++)
            {
                _poolDeactive.Add(deactiveTerms[i]);
                _poolActive.Remove(deactiveTerms[i]);
            }
        }
        private void UpdateActiveePool()
        {
            var activeTerms = new List<AutoTerm>();
            for (int i = 0; i < _poolDeactive.Count; i++)
            {
                if (_poolDeactive[i].IsShow)
                {
                    activeTerms.Add(_poolDeactive[i]);
                }
            }
            for (int i = 0; i < activeTerms.Count; i++)
            {
                _poolActive.Add(activeTerms[i]);
                _poolDeactive.Remove(activeTerms[i]);
            }
        }
        private void ClearPool(List<AutoTerm> pool)
        {
            while (pool.Count > 0)
            {
                var term = pool[0];
                pool.Remove(term);
                Object.Destroy(term.gameObject);
            }
        }
    }
}