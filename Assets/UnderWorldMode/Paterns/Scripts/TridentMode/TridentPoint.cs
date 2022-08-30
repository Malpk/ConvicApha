using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TridentPoint : MonoBehaviour, IPause
    {
        [Header("GeneralMode")]
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _playOnStart;
        [Min(0)]
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Color _markerColor;
        [SerializeField] private TridentPointConfig _config;
        [Header("Reference")]
        [SerializeField] private Trident _tridentPerfab;
        [SerializeField] private SpriteRenderer _marker;

        private bool _isPause;
        private bool _isActive;
        private bool _isCreate;

        private List<Trident> _tridents = new List<Trident>();

        private int[] _angls = new int[] { 0, 180 };

        public bool IsActvate => _isActive;
        public float WidthTrident => _config.WidthOneTrident;

        public delegate void Complite(TridentPoint point);
        public event Complite CompliteAction;

        private void Awake()
        {
            _marker.color = Vector4.zero;
            if (_playOnAwake)
                CreateTridents();
        }
        private void Start()
        {
            if (_playOnStart)
                Activate();
        }
        public void Intilizate(TridentPointConfig config)
        {
            _config = config;
        }
        public void Activate()
        {
#if UNITY_EDITOR
            if (!_isCreate)
                throw new System.Exception("Tridents is not created");
#endif
            StartCoroutine(Work());
        }
        public void Pause()
        {
            _isPause = true;
            foreach (var trident in _tridents)
            {
                trident.Pause();
            }
        }

        public void UnPause()
        {
            _isPause = false;
            foreach (var trident in _tridents)
            {
                trident.UnPause();
            }
        }
        private IEnumerator Work()
        {
            _isActive = true;
            yield return WarningAnimation();
            yield return ActiveTridents();
            yield return WaitDactivationTridents();
            if(CompliteAction != null)
                CompliteAction(this);
            _isActive = false;
        }
        private IEnumerator WarningAnimation()
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return new WaitWhile(() => _isPause);
                progress += Time.deltaTime;
                _marker.color = new Color(_markerColor.r, _markerColor.g, _markerColor.b, _markerColor.a * progress);
                yield return null;
            }
            _marker.color = Vector4.zero;
        }
        public IEnumerator ActiveTridents()
        {
            var angle = _angls[Random.Range(0, _angls.Length)];
            foreach (var trident in _tridents)
            {
                trident.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
                trident.SetDistances(_config.DistanceFromCenter);
                trident.Activate();
                yield return new WaitForSeconds(_delay);
            }
        }
        private IEnumerator WaitDactivationTridents()
        {
            var list = new List<Trident>();
            list.AddRange(_tridents);
            while (list.Count > 0)
            {
                yield return new WaitForSeconds(0.2f);
                var activeList = new List<Trident>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsActive)
                        activeList.Add(list[i]);
                }
                list.Clear();
                list = activeList;
            }
        }
        public void CreateTridents()
        {
#if UNITY_EDITOR
            if (_isCreate)
                throw new System.Exception("Tridents is already created");
#endif
            _isCreate = true;
            _marker.size = new Vector2(_config.CountTrident * _config.WidthOneTrident, _marker.size.y);
            var upLimit = transform.position + transform.right * 
                ((_config.CountTrident - 1) * _config.WidthOneTrident) / 2;
            for (int i = 0; i < _config.CountTrident; i++)
            {
                var position = upLimit - transform.right * i * _config.WidthOneTrident;
                var trident = Instantiate(_tridentPerfab.gameObject,
                    position, transform.rotation).GetComponent<Trident>();
                trident.transform.parent = transform;
                trident.SetConfig(_config.TridentConfig);
                _tridents.Add(trident);
            }
        }
    }
}