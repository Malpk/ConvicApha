using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class ViseModeV2 : TotalMapMode
    {
        [Header("General")]
        [SerializeField] private Vector2 _activeTime;
        [SerializeField] private Vector2 _warningTime;
        [SerializeField] private ViseState _mode = ViseState.HorizontalMode;

        private int _countViseActive = 0;
        private bool _isActivate = false;
        private bool _isOrderActivate = true;
        private List<ViseV2> _vises = new List<ViseV2>();
        private List<ViseV2> _swithcStack = new List<ViseV2>();

        private void Awake()
        {
            foreach (var state in GetViseState(_mode))
            {
                var vise = new ViseV2(state);
                _vises.Add(vise);
            }
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is ViseModeConfig viseModeConfig)
            {
                workDuration = viseModeConfig.WorkDuration;
                _activeTime = viseModeConfig.ActiveTime;
                _warningTime = viseModeConfig.WarningTime;
            }
            else
            {
                throw new System.NullReferenceException("ViseModeConfig is null");
            }
        }
        private ViseState[] GetViseState(ViseState state)
        {
            if (_mode == ViseState.GeneralMode)
            {
                return new ViseState[] {
                    ViseState.HorizontalMode, ViseState.VerticalMode };
            }
            else
            {
                return new ViseState[] { _mode };
            }
        }
        public override bool Play()
        {
            if (!_isActivate)
            {
                _isActivate = true;
                foreach (var vise in _vises)
                {
                    StartCoroutine(MoveVise(vise));
                }
                StartCoroutine(CelearVise());
                return true;
            }
            return false;
        }
        private IEnumerator MoveVise(ViseV2 vise)
        {
            State = ModeState.Play;
            _countViseActive++;
            vise.Intializate(terms);
            var timeWarning = Random.Range(_warningTime.x, _warningTime.y);
            var timeActive = Random.Range(_activeTime.x, _activeTime.y);
            while (vise.Next())
            {
                vise.Show();
                yield return WaitTime(timeWarning);
                yield return new WaitWhile(() => !_isOrderActivate);
                vise.Activate();
                yield return WaitTime(timeActive);
                _swithcStack.Add(vise);
                yield return TrakingFromDelete(vise);
            }
            //yield return WaitHideMap();
            _countViseActive--;
            _isActivate = _countViseActive > 0;
            State = ModeState.Stop;
        }
        #region Delete
        private IEnumerator TrakingFromDelete(ViseV2 vise)
        {
            while (_swithcStack.Contains(vise))
            {
                yield return WaitTime(0.2f);
            }
        }
        private IEnumerator CelearVise()
        {
            while (_isActivate)
            {
                yield return new WaitWhile(() => _swithcStack.Count == 0);
                var vise = _swithcStack[0];
                yield return vise.Deactive(GetCrossroadTerm(vise));
                _swithcStack.Remove(vise);
                yield return new WaitWhile(() => vise.TermVises.Count == 0 && !vise.IsComplite);
            }
        }
        private List<Term> GetCrossroadTerm(ViseV2 vise)
        {
            if (_vises.Count > 1)
            {
                var crossroads = new List<Term>();
                var deactive = new List<Term>();
                for (int i = 0; i < _vises.Count; i++)
                {
                    var crossroad = GetCrossroads(vise, _vises[i]);
                    crossroads.AddRange(crossroad);
                    if (!_vises[i].IsActive)
                        deactive.AddRange(crossroad);
                }
                if(deactive.Count > 0)
                    StartCoroutine(SmoothVise(deactive));
                return crossroads;
            }
            else
            {
                return null;
            }
        }
        private List<Term> GetCrossroads(ViseV2 vise1, ViseV2 vuse2)
        {
            var list = new List<Term>();
            for (int j = 0; j < vise1.TermVises.Count && vise1 != vuse2; j++)
            {
                if (vuse2.TermVises.Contains(vise1.TermVises[j]))
                {
                    list.Add(vise1.TermVises[j]);
                }
            }
            return list;
        }
        #endregion
        private IEnumerator SmoothVise(List<Term> terms)
        {
            _isOrderActivate = false;
            foreach (var term in terms)
            {
                term.Deactivate(true);
            }
            //yield return TrakingDeactiveTerms(terms);
            yield return null;
            _isOrderActivate = true;
        }
    }
    public enum ViseState
    {
        GeneralMode,
        VerticalMode,
        HorizontalMode
    }
}
