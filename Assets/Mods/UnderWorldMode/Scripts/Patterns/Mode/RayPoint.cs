using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class RayPoint : MonoBehaviour
    {
        private RayPoint _center;
        private List<Term> _terms = new List<Term>();

        public bool IsActive { get; private set; } = false;

        public void SetCenter(RayPoint center)
        {
            _center = center;
        }
        public bool Containe(Term term)
        {
            return _terms.Contains(term);
        }

        #region Work ray
        public void ShowRay()
        {
            foreach (var term in _terms)
            {
                if (!term.IsShow)
                    term.Show();
            }
        }
        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                ClearFromCenterPoints();
                for (int i = 0; i < _terms.Count; i++)
                {
                    if (!_terms[i].IsActive)
                        _terms[i].Activate(FireState.Start);
                }
            }
        }
        public void Deactivate()
        {
            IsActive = false;
        }
        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                if (!CheakContainCenter(term))
                    ActivateTerm(term);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                if (!CheakContainCenter(term))
                    DeactivateTerm(term);
            }
        }
        private bool CheakContainCenter(Term term)
        {
            if (_center)
            {
                var value = _center.Containe(term);
                return value;
            }
            else
            {
                return false;
            }
        }
        private void ActivateTerm(Term term)
        {
            _terms.Add(term);
            if (IsActive)
            {
                term.Show();
                term.Activate(FireState.Start);
            }
        }
        private void DeactivateTerm(Term term)
        {
            _terms.Remove(term);
            if (IsActive)
            {
                term.Deactivate(false);
                term.Hide();
            }
        }
        private void ClearFromCenterPoints()
        {
            if (_center)
            {
                var list = new List<Term>();
                for (int i = 0; i < _terms.Count; i++)
                {
                    if (!_center.Containe(_terms[i]))
                        list.Add(_terms[i]);
                }
                _terms.Clear();
                _terms = list;
            }
        }
    }
}