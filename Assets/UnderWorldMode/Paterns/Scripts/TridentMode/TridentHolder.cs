using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TridentHolder : MonoBehaviour,IPause
    {
        [Header("General Setting")]
        [SerializeField] private int _countTridentPoint;
        [SerializeField] private float _delayShoot;
        [SerializeField] private TridentPointConfig _config;
        [Header("Reference")]
        [SerializeField] private TridentPoint _pointPerfabs;

        private bool _isPause = false;
        private bool _isCreate = false;

        private List<TridentPoint> _points = new List<TridentPoint>();

        private Coroutine _start;

        public bool IsActive => _start != null;

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
                var point = Instantiate(_pointPerfabs.gameObject, 
                    transform.right * position, transform.rotation).
                        GetComponent<TridentPoint>();
                point.transform.parent = transform;
                point.Intilizate(_config);
                point.CreateTridents();
                _points.Add(point);
            }
        }
        #endregion
        #region Work Controler
        public void Activate(float timeActive)
        {
#if UNITY_EDITOR
            if (IsActive)
                throw new System.Exception("TridentHolder is already activated");
            else if(!_isCreate)
                throw new System.Exception("TridentPoints is not created");
#endif
            _start = StartCoroutine(Run(timeActive));
        }

        public void Pause()
        {
            _isPause = true;
            foreach (var point in _points)
            {
                point.Pause();
            }
        }

        public void UnPause()
        {
            _isPause = false;
            foreach (var point in _points)
            {
                point.UnPause();
            }
        }
        #endregion
        private IEnumerator Run(float timeActive)
        {
            var progress = 0f;
            while (progress < 1f)
            {
                yield return new WaitWhile(() => _isPause);
                if (GetPoint(out TridentPoint point))
                {
                    point.Activate();
                    yield return new WaitForSeconds(_delayShoot);
                    progress += _delayShoot / timeActive;
                }
                else
                {
                    progress += Time.deltaTime/ timeActive;
                    yield return null;
                }
            }
            _start = null;
        }
        private bool GetPoint(out TridentPoint point)
        {
            if (_points.Count > 0)
            {
                point = _points[Random.Range(0, _points.Count)];
                _points.Remove(point);
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
            point.CompliteAction -= ReturnPoint;
        }
    }
}