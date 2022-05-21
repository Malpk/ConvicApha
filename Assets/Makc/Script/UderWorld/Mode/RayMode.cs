using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class RayMode : GameMode, IModeForSwitch
    {
        [Header("Count Setting")]
        [Min(1)]
        [SerializeField] private int _countRay = 1;
        [Header("Offset Setting")]
        [SerializeField] private float _offset;
        [SerializeField] private float _speedOffset;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _delay;
        [Min(0)]
        [SerializeField] private float _duration;
        [Header("Ray Setting")]
        [SerializeField] private GameObject _ray;

        private int[] _direction = new int[] { 1, -1 };
        private Point[,] _map = null;
        private List<RayPoint> _points = new List<RayPoint>();
        public bool IsActive => startMode != null;

        private void Awake()
        {
            _points = CreateRay(_countRay).ToList();
        }
        public void Constructor(SwitchMode swictMode)
        {
            if (startMode == null)
            {
                Debug.Log("stasss");
                _map = swictMode.builder.Map;
                startMode = StartCoroutine(Rotation(_points));
                TurnOnPoints(_map);
            }
        }
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
        private IEnumerator Rotation(List<RayPoint> rayList)
        {
            yield return new WaitForSeconds(_warningTime);
            var progress = 0f;
            while (progress < 1f)
            {
                var curretOffset = 0f;
                var offset = _offset * GetDirection();
                while (curretOffset != offset)
                {
                    curretOffset = RotateRays(curretOffset, offset);
                    progress += Time.deltaTime / _duration;
                    yield return null;
                }
                yield return new WaitForSeconds(_delay);
                progress += _delay / _duration;
            }
            var point = TurnOffPoints(_map);
            if(point != null)
                yield return new WaitWhile(() => point.IsActive);
            startMode = null;
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
        private RayPoint CreateRay(float lostSteep)
        {
            var ray = Instantiate(_ray).transform;
            ray.parent = transform;
            ray.rotation = Quaternion.Euler(Vector3.forward * lostSteep);
            return GetComponent<RayPoint>();
        }

        public void SetSetting(string jsonSetting)
        {
            var setting = JsonUtility.FromJson<RaySetting>(jsonSetting);
            _countRay = setting.CountRay;
            _delay = setting.Dealy;
            _duration = setting.Duration;
            _offset = setting.OfssetAngle;
            _speedOffset = setting.SpeedOffset;
            _warningTime = setting.WarningTime;
        }
    }
}