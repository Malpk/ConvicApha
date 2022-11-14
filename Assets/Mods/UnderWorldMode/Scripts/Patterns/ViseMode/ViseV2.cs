using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class ViseV2
    {
        private readonly ViseState state;

        private int _steep = 0;
        private int _way;
        private bool _isActive;
        private bool _isDeleteMode = false;
        private Term[,] _mapTerms;
        private List<Term> _terms = new List<Term>();


        public ViseV2(ViseState state)
        {
            this.state = state;
        }

        private delegate bool GetViseCommand(out List<Term> terms);
        private GetViseCommand GetVise;

        public bool IsDeleteMode => _isDeleteMode;
        public bool IsActive => _isActive;
        public bool IsComplite => _steep >= _way;
        public List<Term> TermVises => _terms;

        public void Intializate(Term[,] terms)
        {
            _mapTerms = terms;
            switch (state)
            {
                case ViseState.VerticalMode:
                    _way = _mapTerms.GetLength(1);
                    GetVise = GetVerticalVise;
                    break;
                case ViseState.HorizontalMode:
                    _way = _mapTerms.GetLength(0);
                    GetVise = GetHorizontalVise;
                    break;
            }
        }

        public bool Next()
        {
            if (_steep < _way)
            {
                if (!_isDeleteMode)
                {
                    GetVise(out _terms);
                    _steep++;
                    return true;
                }
                else
                {
                    OutputDeleteExeption();
                    return false;
                }
            }
            else
            {
                _steep = 0;
                return false;
            }
        }
        public void Show()
        {
            if (!IsDeleteMode)
            {
                for (int i = 0; i < _terms.Count; i++)
                {
                    if (!_terms[i].IsShow)
                        _terms[i].Show();
                }
            }
            else
            {
                OutputDeleteExeption();
            }
        }
        public void Activate()
        {
            if (!IsDeleteMode)
            {
                _isActive = true;
                for (int i = 0; i < _terms.Count; i++)
                {
                    if (!_terms[i].IsActive)
                        _terms[i].Activate(FireState.Start);
                }
            }
            else
            {
                OutputDeleteExeption();
            }
        }
        #region Deactive Vise
        public IEnumerator Deactive(List<Term> dontDeactive = null)
        {
            if (dontDeactive != null)
            {
                Clear(dontDeactive);
            }
            _isDeleteMode = true;
            foreach (var term in _terms)
            {
                if (term.IsActive)
                {
                    term.Deactivate();
                }
            }
            yield return Traking();
            _isDeleteMode = false;
            _isActive = false;
        }
        private IEnumerator Traking()
        {
            var activeList = new List<Term>();
            while (_terms.Count > 0)
            {
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < _terms.Count; i++)
                {
                    if (_terms[i].IsShow)
                        activeList.Add(_terms[i]);
                }
                _terms.Clear();
                _terms.AddRange(activeList);
                activeList.Clear();
            }
        }
        private void Clear(List<Term> dontDeactive)
        {
            for (int i = 0; i < dontDeactive.Count; i++)
            {
                if (_terms.Contains(dontDeactive[i]))
                    _terms.Remove(dontDeactive[i]);
            }
        }
        #endregion
        #region Get Vise
        private bool GetVerticalVise(out List<Term> list)
        {
            list = new List<Term>();
            for (int j = 0; j < _mapTerms.GetLength(0); j++)
            {
                list.Add(_mapTerms[j, _steep]);
                list.Add(_mapTerms[j, _way - _steep - 1]);
            }
            return list.Count > 0;
        }
        private bool GetHorizontalVise(out List<Term> list)
        {
            list = new List<Term>();
            for (int j = 0; j < _mapTerms.GetLength(0); j++)
            {
                list.Add(_mapTerms[_steep, j]);
                list.Add(_mapTerms[_way - _steep - 1, j]);
            }
            return list.Count > 0;
        }
        #endregion
        private void OutputDeleteExeption()
        {
            throw new System.Exception("Call during delete");
        }
    }
}