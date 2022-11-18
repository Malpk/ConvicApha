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

        private List<Term> _activateTerms = new List<Term>();
        private List<Term> _deactiveTerms = new List<Term>();

        private BasePatternState _curretState = null;
        private PatternIdleState _startState;
        private PatternIdleState _warningState;
        private PatternIdleState _endState;
        private RotationPaternState _rotateState;

        protected override void Awake()
        {
            base.Awake();
            _startState = new PatternIdleState(0.5f);
            _rotateState = new RotationPaternState(_speedRotation, workDuration);
            _warningState = new PatternIdleState(_warningTime);
            _endState = new PatternIdleState(1);
            _startState.SetNextState(_warningState);
            _rotateState.SetNextState(_endState);
            _warningState.SetNextState(_rotateState);
            _endState.SetNextState(compliteState);
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
        protected override void OnEnable()
        {
            base.OnEnable();
            _startState.OnComplite += ShowMap;
            _rotateState.OnUpdate += SetAngle;
            _endState.OnComplite += DeactivateTerms;
            _warningState.OnComplite += () => ActivateTerms(_activateTerms);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _startState.OnComplite -= ShowMap;
            _rotateState.OnUpdate -= SetAngle;
            _endState.OnComplite -= DeactivateTerms;
            _warningState.OnComplite -= () => ActivateTerms(_activateTerms);
        }
        protected override void PlayMode()
        {
            _curretState = _startState;
            transform.rotation *= Quaternion.Euler(Vector3.forward *
                 DefineStartAngel(player.transform.position) * Time.deltaTime);
        }
        protected override void StopMode()
        {
        }
        private void Update()
        {
            if (_curretState != null)
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
                    Stop();
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
                if (IsPlay)
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