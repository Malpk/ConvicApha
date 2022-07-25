using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class TridentMode : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private float _speedMovement;
        [SerializeField] private float _moveDistance;
        [SerializeField] private bool _horizontalMode;
        [SerializeField] private bool _verticalMode;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _shootDelay;
        [Min(0)]
        [SerializeField] private float _groupShootDelay;
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _workDuration;
        [Header("Transform Setting")]
        [Min(1)]
        [SerializeField] private int _countTridentInGroup = 1;
        [Min(1)]
        [SerializeField] private int _radius = 1;
        [SerializeField] private Vector2 _unitSize;
        [Header("Required Perfab")]
        [SerializeField] private GameObject _marker;
        [SerializeField] private GameObject _trident;

        private int _countCorotine = 0;
        private Transform _markerHodlder;
        private int[] _verticalAngels = new int[] { 0, 180 };
        private int[] _horizontalAngls = new int[] { -90, 90 };
        private List<Marker> _markersPool = new List<Marker>();
        private List<Trident[]> _vertivalTrident = new List<Trident[]>();
        private List<Trident[]> _horizontalTrident = new List<Trident[]>();

        public bool IsAttackMode => _countCorotine > 0;

        private void Awake()
        {
            ActivateTrident();
            _markerHodlder = GetHolder("MarkerHolder");
        }
        private void ActivateTrident()
        {
#if UNITY_EDITOR
            if(!_horizontalMode && !_verticalMode)
            {
                Debug.LogWarning("Tridentode is not working");
            }
#endif
            var holder = GetHolder("TridentHolder");
            var curretPosition = GetBounds(_unitSize, _radius * _countTridentInGroup);
            if (_horizontalMode)
            {
                _horizontalTrident = CreateTridentPool(Vector2.up, curretPosition.y, _unitSize.y, holder).ToList();
            }
            if (_verticalMode)
            {
                _vertivalTrident = CreateTridentPool(Vector2.right, curretPosition.x, _unitSize.x, holder).ToList();
            }
        }
        private IEnumerable<Trident[]> CreateTridentPool(Vector2 direction,float curretPosition,
            float unitOffset,Transform holder = null) 
        { 
            if (_trident.GetComponent<Trident>() == null)
                throw new System.NullReferenceException($"{_trident.name} is no component \" Trident \" ");
            for (int i = 0; i < _radius * 2; i++)
            {
                var tridentGroup = new Trident[_countTridentInGroup];
                for (int j = 0; j < _countTridentInGroup; j++)
                {
                    tridentGroup[j] = (CreateTrident(_trident, direction * curretPosition, holder));
                    curretPosition -= unitOffset;
                }
                yield return tridentGroup;
            }
        }

        private IEnumerator ShootTrident(List<Trident[]> listTrident, int[] angls, float startDelay = 0)
        {
            _countCorotine++;
            yield return new WaitForSeconds(startDelay);
            float progress = 0f;
            Trident[] curretGroup = null;
            while (progress < 1f)
            {
                curretGroup = GetGroup(listTrident, angls);
                if (curretGroup != null)
                {
                    int index = Random.Range(0, angls.Length);
                    foreach (var trident in curretGroup)
                    {
                        trident.StartMove(_moveDistance, angls[index], GetMarker());
                        progress += _shootDelay / _workDuration;
                        yield return new WaitForSeconds(_shootDelay);
                    }
                }
                progress += _groupShootDelay / _workDuration;
                yield return new WaitForSeconds(_groupShootDelay);
            }
            _countCorotine--;
            if (_countCorotine == 0)
                gameObject.SetActive(false);
            yield return null;
        }
        private Trident[] GetGroup(List<Trident[]> list,int[] angels)
        {
            var listVertival = list.Where<Trident[]>(trident => !trident[0].IsActive).ToList();
            if (listVertival.Count == 0)
                return null;
            var index = Random.Range(0, angels.Length);
            var tridentIndex = Random.Range(0, listVertival.Count);
            return listVertival[tridentIndex];
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
        private Vector2 GetBounds(Vector2 unit, int count)
        {
            return new Vector2(unit.x / 2, unit.y / 2) +
                unit * (count - 1);
        }
        private Transform GetHolder(string name)
        {
            var holder = new GameObject(name).transform;
            holder.parent = transform;
            holder.localPosition = Vector3.zero;
            return holder;
        }
        private Trident CreateTrident(GameObject trident, Vector2 position, Transform parent = null)
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
    }
}