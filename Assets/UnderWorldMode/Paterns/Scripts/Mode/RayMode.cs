using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class RayMode : TotalMapMode
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
        [Min(0)]
        [SerializeField] private float _duration;
        [Header("Ray Setting")]
        [SerializeField] private RayPoint _rayPerfab;

        private int[] _direction = new int[] { 1, -1 };
        private Point[,] _map = null;
        private Coroutine _runMode;
        private List<RayPoint> _rays = new List<RayPoint>();

        protected override void Awake()
        {
            base.Awake();
            _rays = CreateRay(_countRay).ToList();
        }
        public override bool Activate()
        {
            if (_runMode == null && IsReady)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(Rotate(_rays));
                return false;
            }
            return false;
        }
        #region Work Mode
        private IEnumerator Rotate(List<RayPoint> rayList)
        {
            yield return WaitTime(_warningTime);
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
                    progress += Time.deltaTime / _duration;
                    yield return null;
                }
                yield return new WaitForSeconds(_delay);
                progress += _delay / _duration;
            }
            DeactivateMap(out HandTermTile term);
            yield return new WaitWhile(() => term.IsActive);
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
            transform.rotation *= Quaternion.Euler(Vector3.forward * steep);
            return curretOffset;
        }
        #endregion
        #region Create Ray for Mode
        private IEnumerable<RayPoint> CreateRay(int count)
        {
            var lostSteep = 0f;
            var steepRotation = 360 / count;
            for (int i = 0; i < count; i++)
            {
                var ray = CreateRay(lostSteep);
                if (ray != null)
                    yield return ray;
                lostSteep += steepRotation;
            }
        }
        private RayPoint CreateRay(float lostSteep)
        {
            var ray = Instantiate(_rayPerfab.gameObject).transform;
            ray.parent = transform;
            ray.rotation = Quaternion.Euler(Vector3.forward * lostSteep);
            return GetComponent<RayPoint>();
        }
        #endregion
    }
}