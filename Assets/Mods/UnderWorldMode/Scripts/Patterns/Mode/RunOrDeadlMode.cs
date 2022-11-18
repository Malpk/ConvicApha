using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class RunOrDeadlMode : TotalMapMode
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedRotation;
        [Min(0)]
        [SerializeField] private float _warningTime;

        private bool _isPlay = false;
        private List<Term> _activateTerms = new List<Term>();
        private List<Term> _deactiveTerms = new List<Term>();

        private BasePatternState _curretState = null;
        private PatternIdleState _startState;
        private PatternIdleState _warningState;
        private RotationPaternState _rotateState;
        private Coroutine _runMode;

        private void Awake()
        {
            _startState = new PatternIdleState(0.5f);
            _rotateState = new RotationPaternState(_speedRotation, workDuration);
            _warningState = new PatternIdleState(_warningTime);
            _startState.SetNextState(_warningState);
            _rotateState.SetNextState(compliteState);
            _warningState.SetNextState(_rotateState);
            _rotateState.SetNextState(compliteState);
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is RunOrDeadConfig runOrDeadConfig)
            {
                workDuration = runOrDeadConfig.WorkDuration;
                _warningTime = runOrDeadConfig.WarningTime;
                _speedRotation = runOrDeadConfig.SpeedRotation;
            }
            else
            {
                throw new System.NullReferenceException("RunOrDeadConfig is null");
            }
        }
        private void OnEnable()
        {
            _startState.OnComplite += ShowMap;
            _rotateState.OnUpdate += SetAngle;
            _warningState.OnComplite += ActivateMap;
        }
        private void OnDisable()
        {
            _startState.OnComplite -= ShowMap;
            _rotateState.OnUpdate -= SetAngle;
            _warningState.OnComplite -= ActivateMap;
        }
        public override bool Play()
        {
            if (_runMode == null)
            {
                State = ModeState.Play;
                _curretState = _startState;
                _isPlay = true;
                return true;
            }
            return false;
        }
        public void Stop()
        {
            _isPlay = false;
        }
        private void Update()
        {
            if (_curretState != default(BasePatternState))
                UpdateState();
        }

        private void UpdateState()
        {
            if (_curretState.IsComplite)
            {
                if (_curretState.GetNextState(out BasePatternState nextState))
                {
                    _curretState = nextState;
                    _curretState.Start();
                }
                else
                {
                    _curretState = default(BasePatternState);
                }
            }
            else
            {
                _curretState.Update();
            }
        }    
        private void SetAngle(Quaternion angle)
        {
            transform.rotation *= angle;
        }
        private void ShowMap()
        {
            _activateTerms = GetActiveTerms(_deactiveTerms);
            foreach (var term in _activateTerms)
            {
                term.Show();
            }
        }
        private void ActivateMap()
        {
            foreach (var term in _activateTerms)
            {
                term.Activate(FireState.Stay);
            }
            transform.rotation *= Quaternion.Euler(Vector3.forward *
                DefineStartAngel(player.transform.position) * Time.deltaTime);
        }
        private float DefineStartAngel(Vector2 player)
        {
            var angel = Vector2.Angle(transform.right, player);
            if (player.y < transform.position.y)
                return -angel;
            return angel;
        }

        private List<Term> GetActiveTerms(List<Term> deactiveTerms)
        {
            var list = new List<Term>();
            foreach (var term in terms)
            {
                if (!deactiveTerms.Contains(term))
                {
                    list.Add(term);
                }
            }
            return list;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                _deactiveTerms.Add(term);
                if (_isPlay)
                {
                    if (term.IsActive)
                        term.Deactivate(false);
                    if (term.IsShow)
                        term.Hide();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                _deactiveTerms.Remove(term);
                term.Show();
                term.Activate(FireState.Stay);
            }
        }
    }
}