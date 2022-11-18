using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class SunMode : TotalMapMode
    {
        [Header("General Setting")]

        [Min(1)]
        [SerializeField] private int _countRay = 1;
        [SerializeField] private float _offset;
        [Header("Work Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime = 0;
        [Min(1)]
        [SerializeField] private int _countOffset = 1;
        [Min(0.2f)]
        [SerializeField] private float _durationOffset = 0.5f;
        [Min(0)]
        [SerializeField] private float _delay;
        [Header("Ray Setting")]
        [SerializeField] private RayPoint _center;
        [SerializeField] private RayPoint _rayPerfab;
        [SerializeField] private Transform _rayHolder;

        private int _diraction;
        private Quaternion _startAngle;
        private List<RayPoint> _rays = new List<RayPoint>();

        private BasePatternState _curretState;
        private PatternIdleState _startState;
        private PatternIdleState _warningState;
        private PatternIdleState _endState;
        private SunModeRotationState _rotationState;

        protected override void Awake()
        {
            base.Awake();
            _rays = CreateRay(_countRay);
            _rays.Add(_center);
            _startState = new PatternIdleState(0.5f);
            _warningState = new PatternIdleState(_warningTime);
            _endState = new PatternIdleState(1);
            _rotationState = new SunModeRotationState(_countOffset, _durationOffset, _delay);
            _startState.SetNextState(_warningState);
            _warningState.SetNextState(_rotationState);
            _rotationState.SetNextState(_endState);
            _endState.SetNextState(compliteState);
            enabled = false;
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is SunModeConfig sunModeConfig)
            {
                _countRay = sunModeConfig.CountRay;
                _offset = sunModeConfig.AngleOffet;
                _warningTime = sunModeConfig.WarningTime;
                _delay = sunModeConfig.Delay;
            }
            else
            {
                throw new System.NullReferenceException("SunModeConfig is null");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _startState.OnComplite += ShowTerms;
            _warningState.OnComplite += ActivateRays;
            _rotationState.OnUpdate += RotateRays;
            _rotationState.OnReset += SetStartAngle;
            _endState.OnComplite += DeactivateTerms;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _startState.OnComplite -= ShowTerms;
            _warningState.OnComplite -= ActivateRays;
            _rotationState.OnUpdate -= RotateRays;
            _rotationState.OnReset -= SetStartAngle;
            _endState.OnComplite -= DeactivateTerms;
        }
        protected override void PlayMode()
        {
            enabled = true;
            _curretState = _startState;
        }
        protected override void StopMode()
        {
            enabled = false;
        }
        private void Update()
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
        #region Work Mode
        private void ShowTerms()
        {
            _center.ShowRay();
            foreach (var ray in _rays)
            {
                ray.ShowRay();
            }
        }
        private void ActivateRays()
        {
            _center.Activate();
            foreach (var ray in _rays)
            {
                ray.Activate();
            }
        }
        private void DeactivateRays()
        {
            _center.Deactivate();
            foreach (var ray in _rays)
            {
                ray.Deactivate();
            }
        }
        private void SetStartAngle()
        {
            _startAngle = _rayHolder.rotation;
            var diractions = new int[] { -1, 1 };
            _diraction = diractions[Random.Range(0, diractions.Length)];
        }
        private void RotateRays(float progress)
        {
            _rayHolder.rotation = _startAngle * Quaternion.Euler(Vector3.forward
                * _diraction *  _offset * progress);
        }
        #endregion
        #region Create Ray for Mode
        private List<RayPoint> CreateRay(int count)
        {
            var list = new List<RayPoint>();
            var lostSteep = 0f;
            var steepRotation = 360 / count;
            for (int i = 0; i < count; i++)
            {
                if (CreateRay(lostSteep, out RayPoint ray))
                    list.Add(ray);
                lostSteep += steepRotation;
            }
            return list;
        }
        private bool CreateRay(float lostSteep, out RayPoint ray)
        {
            ray = Instantiate(_rayPerfab.gameObject).GetComponent<RayPoint>();
            ray.SetCenter(_center);
            ray.transform.parent = _rayHolder;
            ray.transform.rotation = Quaternion.Euler(Vector3.forward * lostSteep);
            return ray;
        }
        #endregion
    }
}