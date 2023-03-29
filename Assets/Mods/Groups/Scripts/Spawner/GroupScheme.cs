using System;
using UnityEngine;

namespace MainMode
{
    public class GroupScheme : MonoBehaviour
    {
        [SerializeField] private bool _showSizeChank;
        [SerializeField] private float _size;
        [SerializeField] private Color _color;

        private int _countActiveZone;
        private SpawnZone[] _zones;

        public event Action<GroupScheme> OnDeactivate;

        public bool isActive => _countActiveZone > 0;

        public GameObject PoolItem => gameObject;

        private void Awake()
        {
            _zones = GetComponentsInChildren<SpawnZone>();
        }

        private void OnEnable()
        {
            foreach (var zone in _zones)
            {
                zone.OnActivate += ActivateZone;
                zone.OnDectivate += DeactivateZone;
            }
        }

        private void OnDisable()
        {
            foreach (var zone in _zones)
            {
                zone.OnActivate -= ActivateZone;
                zone.OnDectivate -= DeactivateZone;
            }
        }

        public void Intializate()
        {
            foreach (var zone in _zones)
            {
                zone.Initializate();
            }
        }

        private void ActivateZone()
        {
            _countActiveZone++;
        }
        public void DeleteZone()
        {
            foreach (var zone in _zones)
            {
                if (zone.IsActive)
                    zone.Stop();
            }
        }
        private void DeactivateZone()
        {
            _countActiveZone = Mathf.Clamp(_countActiveZone--, 0, _countActiveZone);
            if (_countActiveZone == 0)
            {
                OnDeactivate?.Invoke(this);
            }
        }

        private void OnDrawGizmos()
        {
            if (_showSizeChank)
            {
                Gizmos.color = _color;
                Gizmos.DrawCube(transform.position, Vector2.one * _size);
            }
        }
    }
}
