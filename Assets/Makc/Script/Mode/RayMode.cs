using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class RayMode : GameMode
    {
        [Header("Game Setting")]
        [SerializeField] private int _countRay;
        [SerializeField] private float _speedOffset;
        [SerializeField] private float _offset;
        [SerializeField] private float _delay;
        [SerializeField] private float _duration;

        [Header("Scene Setting")]
        [SerializeField] private GameObject _ray;
        [SerializeField] private TrisMode _trisMode;

        private int[] _direction = new int[]
        {
            1,-1
        };
        private bool _status = true;

        public override bool statusWork => _status;

        private void Start()
        {
            var rayList  = CreateRay();
            StartCoroutine(Rotation(rayList));
        }
        private List<RayPoint> CreateRay()
        {
            var list = new List<RayPoint>();
            var steepRotation = 360 / _countRay;
            var lostSteep = 0f;
            for (int i = 0; i < _countRay; i++)
            {
                var ray = Instantiate(_ray).transform;
                ray.parent = transform;
                ray.rotation = Quaternion.Euler(Vector3.forward * lostSteep);
                if (ray.TryGetComponent<RayPoint>(out RayPoint point))
                    list.Add(point);
                lostSteep += steepRotation;
            }
            return list;
        }

        private IEnumerator Rotation(List<RayPoint> rayList)
        {
            var progress = 0f;
            while (progress < 1f)
            {
                var offsetProgress = 0f;
                var index = Random.Range(0, _direction.Length);
                var offset = _offset * _direction[index];
                while (offsetProgress != offset)
                {
                    var previousOffset = offsetProgress;
                    offsetProgress = Mathf.MoveTowards(offsetProgress, offset, _speedOffset*Time.deltaTime);
                    var steep = offsetProgress - previousOffset;
                    transform.rotation *= Quaternion.Euler(Vector3.forward * steep);
                    progress += Time.deltaTime / _duration;
                    yield return null;
                }
                yield return new WaitForSeconds(_delay);
                progress += _delay / _duration;
            }
            _status = false;
        }
    }
}