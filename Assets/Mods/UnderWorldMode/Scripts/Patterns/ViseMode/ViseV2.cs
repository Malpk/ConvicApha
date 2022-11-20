using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class ViseV2
    {
        private readonly ViseState state;

        private int _steep = -1;
        private int _way;
        private Term[,] _terms;

        private List<Term> _pool = new List<Term>();
        public ViseV2(ViseState state)
        {
            this.state = state;
        }

        private delegate bool GetViseCommand(out List<Term> terms);
        private GetViseCommand GetVise;

        public bool IsComplite => _steep >= _way;

        public void Intializate(Term[,] terms)
        {
            _terms = terms;
            switch (state)
            {
                case ViseState.VerticalMode:
                    _way = _terms.GetLength(1);
                    GetVise = GetVerticalVise;
                    break;
                case ViseState.HorizontalMode:
                    _way = _terms.GetLength(0);
                    GetVise = GetHorizontalVise;
                    break;
            }
        }
        public void Reset()
        {
            _steep = -1;
        }
        public bool Next()
        {
            _steep++;
            return _steep < _way;
        }
        public void ShowVise()
        {
            if (GetVise(out _pool))
            { 
                foreach (var term in _pool)
                {
                    term.Show();
                } 
            }
        }
        public void ActivateVise()
        {
            foreach (var term in _pool)
            {
                term.Activate();
            }
        }
        public void HideVise()
        {
            if (GetVise(out _pool))
            {
                foreach (var term in _pool)
                {
                    term.Deactivate(false);
                    term.Hide();
                }
            }
        }
        #region Get Vise
        private bool GetVerticalVise(out List<Term> list)
        {
            list = new List<Term>();
            for (int j = 0; j < _terms.GetLength(0); j++)
            {
                if (CheakOnCrossHorizontal(_steep, j))
                {
                    list.Add(_terms[j, _steep]);

                    list.Add(_terms[j, _way - _steep - 1]);
                }
            }
            return list.Count > 0;
        }
        private bool GetHorizontalVise(out List<Term> list)
        {
            list = new List<Term>();
            for (int j = 0; j < _terms.GetLength(0); j++)
            {
                var down = _way - _steep - 1;
                if (CheakOnCrossVertical(_steep, j))
                {
                    list.Add(_terms[_steep, j]);
                    list.Add(_terms[down, j]);
                }
            }
            return list.Count > 0;
        }

        private bool CheakOnCrossVertical(int i, int j)
        {
            if (i + 2 < _terms.GetLength(0))
            {
                return !_terms[i + 2, j].IsShow;
            }
            else if (i - 2 > 0)
            {
                return !_terms[i - 2, j].IsShow;
            }
            return false;
        }
        private bool CheakOnCrossHorizontal(int i, int j)
        {
            if (i + 2 < _terms.GetLength(1))
            {
                return !_terms[j, i + 2].IsShow;
            }
            else if (i - 2 > 0)
            {
                return !_terms[j, i - 2].IsShow;
            }
            return false;
        }
        #endregion
    }
}