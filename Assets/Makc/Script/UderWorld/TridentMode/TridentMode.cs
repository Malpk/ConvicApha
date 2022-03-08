using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;
using System.Linq;

namespace Underworld
{
    public class TridentMode : MonoBehaviour, IModeForSwitch
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedMovement;
        [SerializeField] private float _moveDistance;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _shootDelay;
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _workDuration;
        [Header("Transform Setting")]
        [Min(1)]
        [SerializeField] private int _radius = 1;
        [SerializeField] private Vector2 _unitSize;
        [Header("Required Perfab")]
        [SerializeField] private GameObject _marker;
        [SerializeField] private GameObject _trident;

        private Coroutine _startWork = null;
        private Transform _markerHodlder;
        private int[] _verticalAngels = new int[] { 0, 180 };
        private int[] _horizontalAngls = new int[] { -90, 90 };
        private List<Trident> _vertivalTrident = new List<Trident>();
        private List<Trident> _horizontalTrident = new List<Trident>();
        private List<Marker> _markersPool = new List<Marker>();

        public bool IsAttackMode => _startWork != null;

        private void Awake()
        {
            Constructor(null);
            CreateTridentPool();
            _markerHodlder = GetHolder("MarkerHolder");
        }
        public void Constructor(SwitchMode swictMode)
        {
            if (_startWork != null)
                return;
            _startWork = StartCoroutine(RunMode());
        }
        private void CreateTridentPool()
        {
            var curretPosition = new Vector2(_unitSize.x / 2, _unitSize.y / 2) +
             _unitSize * (_radius - 1);
            if (_trident.GetComponent<Trident>() == null)
                throw new System.NullReferenceException($"{_trident.name} is no component \" Trident \" ");
            var holder = GetHolder("TridentHolder");
            for (int i = 0; i < _radius * 2; i++)
            {
                _vertivalTrident.Add(CreateTrident(_trident, Vector2.right * curretPosition.x, holder));
                _horizontalTrident.Add(CreateTrident(_trident, Vector2.up * curretPosition.y, holder));
                curretPosition -= _unitSize;
            }
        }
        private Transform GetHolder(string name)
        {
            var holder = new GameObject(name).transform;
            holder.parent = transform;
            holder.localPosition = Vector3.zero;
            return holder;
        }
        private Trident CreateTrident(GameObject trident, Vector2 position,Transform parent = null)
        {
            var instateTrident = Instantiate(trident, position, Quaternion.identity);
            var tridentComponent = instateTrident.GetComponent<Trident>();
            tridentComponent.Constructor(_speedMovement, _warningTime);
            instateTrident.transform.parent = parent;
            return tridentComponent;
        }
        private Marker CreateMarker(GameObject marker)
        {
            var markerInstate = Instantiate(marker, Vector3.zero, Quaternion.identity);
            var markerComponent = markerInstate.GetComponent<Marker>();
            markerInstate.transform.parent = _markerHodlder;
            if (markerComponent == null)
                throw new System.NullReferenceException($"{marker.name} is no component \" Marker \" ");
            else
                return markerComponent;
        }
        private IEnumerator RunMode()
        {
            float progress = 0f;
            while (progress < 1f)
            {
                ShootTrident(_vertivalTrident, _verticalAngels);
                yield return new WaitForSeconds(_shootDelay);
                ShootTrident(_horizontalTrident, _horizontalAngls);
                yield return new WaitForSeconds(_shootDelay);
                progress += _shootDelay * 2 / _workDuration;
            }
            yield return null;
        }
        private void ShootTrident(List<Trident> list,int[] angels)
        {
            var listVertival = list.Where<Trident>(trident => !trident.IsActive).ToList();
            if (listVertival.Count == 0)
                return;
            var index = Random.Range(0, angels.Length);
            var tridentIndex = Random.Range(0, listVertival.Count);
            listVertival[tridentIndex].StartMove(_moveDistance, angels[index], GetMarker());
        }
        private Marker GetMarker()
        {
            foreach (var marker in _markersPool)
            {
                if (!marker.IsActive)
                    return marker;
            }
            var newMarker = CreateMarker(_marker);
            _markersPool.Add(newMarker);
            return newMarker;
        }
    }
}