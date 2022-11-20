using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TridentHolder : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private int _countTridentPoint;
        [SerializeField] private float _delayShoot;
        [SerializeField] private TridentPointConfig _config;
        [Header("Reference")]
        [SerializeField] private TridentPoint _pointPerfabs;

        private float _timeActive;
        private float _progress;
        private float _progressDelay = 0f;
        private float _progressCheak = 0f;
        private bool _isCreate = false;
        private List<TridentPoint> _points = new List<TridentPoint>();
        private List<TridentPoint> _activePoint = new List<TridentPoint>();

        private System.Action _task;

        public event System.Action OnComplite;
        

        public bool IsActive => enabled;

        #region Intilizate
        public void Intilizate(TridentPointConfig setting ,int countTrindentPoint)
        {
            _config = setting;
            _countTridentPoint = countTrindentPoint;
        }

        public void CreatePoints()
        {
#if UNITY_EDITOR
            if (_isCreate)
                throw new System.Exception("Point is already created");
#endif
            _isCreate = true;
            var widthPoint = _pointPerfabs.WidthTrident * _config.CountTrident;
            var startPosition = (widthPoint * (_countTridentPoint - 1)) / 2;
            for (int i = 0; i < _countTridentPoint; i++)
            {
                var position = startPosition - widthPoint * i;
                var point = Instantiate(_pointPerfabs.gameObject, transform.right * position, transform.rotation).GetComponent<TridentPoint>();
                point.transform.parent = transform;
                point.Intilizate(_config);
                point.CreateTridents();
                point.transform.parent = transform;
                _points.Add(point);
            }
        }
        #endregion
        #region Work Controler
        public void Activate(float timeActive)
        {
#if UNITY_EDITOR
            if(!_isCreate)
                throw new System.Exception("TridentPoints is not created");
#endif
            _timeActive = timeActive;
            _progress = 0f;
            _progressDelay = 0f;
            _progressCheak = 0f;
            _task = Shooting;
            enabled = true;
        }
        public void Deactivate()
        {
            enabled = false;
            OnComplite?.Invoke();
        }
        private void Update()
        {
            _task();
        }

        private void Shooting()
        {
            _progress += Time.deltaTime / _timeActive;
            _progressDelay += Time.deltaTime / _delayShoot;
            if (_progressDelay >= 1)
            {
                _progressDelay = 0f;
                ShootTrident();
                if (_progress >= 1f)
                {
                    _task = Compliting;
                }
            }
        }
        private void Compliting()
        {
            _progressCheak += Time.deltaTime / 0.2f;
            if (_progressCheak >= 1f)
            {
                _progressCheak = 0f;
                if (CheakComplite())
                {
                    Deactivate();
                }
            }
        }
        private bool CheakComplite()
        {
            var termp = new List<TridentPoint>();
            for (int i = 0; i < _activePoint.Count; i++)
            {
                if (_activePoint[i].IsActvate)
                    termp.Add(_activePoint[i]);
            }
            _activePoint = termp;
            return termp.Count == 0;
        }
        #endregion
        private void ShootTrident()
        {
            if (GetPoint(out TridentPoint point))
            {
                point.Activate();
            }
        }
        private bool GetPoint(out TridentPoint point)
        {
            if (_points.Count > 0)
            {
                point = _points[Random.Range(0, _points.Count)];
                _points.Remove(point);
                _activePoint.Add(point);
                point.CompliteAction += ReturnPoint;
                return point;
            }
            else
            {
                point = null;
                return false;
            }
        }
        private void ReturnPoint(TridentPoint point)
        {
            _points.Add(point);
            _activePoint.Remove(point);
            point.CompliteAction -= ReturnPoint;
        }
    }
}