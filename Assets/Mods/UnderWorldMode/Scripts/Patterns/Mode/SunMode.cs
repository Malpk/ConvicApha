using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class SunMode : TotalMapMode
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private int _countRay = 1;
        [SerializeField] private float _offset;
        [SerializeField] private float _speedOffset;
        [Header("Work Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _delay;
        [Header("Ray Setting")]
        [SerializeField] private RayPoint _center;
        [SerializeField] private RayPoint _rayPerfab;
        [SerializeField] private Transform _rayHolder;

        private int[] _direction = new int[] { 1, -1 };
        private Coroutine _runMode;
        private List<RayPoint> _rays = new List<RayPoint>();

        protected void Awake()
        {
            _rays = CreateRay(_countRay);
            _rays.Add(_center);
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is SunModeConfig sunModeConfig)
            {
                _countRay = sunModeConfig.CountRay;
                _offset = sunModeConfig.AngleOffet;
                _speedOffset = sunModeConfig.SpeedOffset;
                _warningTime = sunModeConfig.WarningTime;
                _delay = sunModeConfig.Delay;
            }
            else
            {
                throw new System.NullReferenceException("SunModeConfig is null");
            }
        }
        public override bool Play()
        {
            if (_runMode == null)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(Rotate(_rays));
                return false;
            }
            return false;
        }
        #region Work Mode
        private IEnumerator Rotate(List<RayPoint> rays)
        {
            yield return null;
            State = ModeState.Play;
            _center.ShowRay();
            foreach (var ray in rays)
            {
                ray.ShowRay();
            }
            yield return WaitTime(_warningTime);
            _center.Activate();
            foreach (var ray in rays)
            {
                ray.Activate();
            }
            var progress = 0f;
            while (progress < 1f)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                var curretOffset = 0f;
                var offset = _offset * GetDirection();
                while (curretOffset != offset)
                {
                    yield return new WaitWhile(() => State == ModeState.Pause);
                    curretOffset = RotateRays(curretOffset, offset);
                    progress += Time.deltaTime / workDuration;
                    yield return null;
                }
                yield return new WaitForSeconds(_delay);
                progress += _delay / workDuration;
            }
            var list = new List<Term>();
            foreach (var ray in _rays)
            {
                list.AddRange(ray.Deactivate());
            }
            yield return TrakingDeactiveTerms(list);
            State = ModeState.Stop;
            _runMode = null;
        }
        private int GetDirection()
        {
            var index = Random.Range(0, _direction.Length);
            return _direction[index];
        }
        private float RotateRays(float previsious,float ofsset)
        {
            var curretOffset = Mathf.MoveTowards(previsious, ofsset, _speedOffset * Time.deltaTime);
            var steep = curretOffset - previsious;
            _rayHolder.rotation *= Quaternion.Euler(Vector3.forward * steep);
            return curretOffset;
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