using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class Chank : MonoBehaviour
    {
        [SerializeField] private bool _spawnOnStart;
        [SerializeField] private bool _rotate;
        [SerializeField] private SchemePool[] _schems;
#if UNITY_EDITOR
        [Header("DrawGismo")]
        [SerializeField] private bool _showSizeChank;
        [SerializeField] private Color _color;
        [SerializeField] private Vector2 _size;
#endif

        private Vector3[] _directions = new Vector3[] { Vector3.up, Vector3.right, Vector3.left, Vector3.down};
        private GroupScheme _activeScheme;
        private SchemePool _prevouslPool;

        public bool IsActive => _activeScheme;

        private void OnValidate()
        {
            foreach (var schene in _schems)
            {
                schene.OnValidate();
            }
        }

        private void Start()
        {
            if(_spawnOnStart)
                Spawn();
        }

        public void Spawn()
        {
            if (!IsActive && _schems.Length > 0)
            {
                _prevouslPool = GetPool();
                _activeScheme = _prevouslPool.GetScheme(transform);
                _activeScheme.Intializate();
                if (_rotate)
                    _activeScheme.transform.up = _directions[Random.Range(0, _directions.Length)];
                else
                    _activeScheme.transform.up = _activeScheme.transform.up * (Random.Range(0, 2) > 1 ? 1 : -1);
                _activeScheme.OnDeactivate += Deactivate;
            }
        }

        public void DeleteScheme()
        {
            if(_activeScheme)
                Deactivate(_activeScheme);
        }

        private void Deactivate(GroupScheme scheme)
        {
            scheme.OnDeactivate -= Deactivate;
            _activeScheme.DeleteZone();
            _activeScheme.gameObject.SetActive(false);
            _activeScheme = null;

        }

        private SchemePool GetPool()
        {
            if (_schems.Length > 1)
            {
                var list = new List<SchemePool>();
                foreach (var pool in _schems)
                {
                    if (_prevouslPool != pool)
                    {
                        list.Add(pool);
                    }
                }
                return list[Random.Range(0, list.Count)];
            }
            else
            {
                return _schems[0];
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                Spawn();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_showSizeChank)
            {
                Gizmos.color = _color;
                Gizmos.DrawCube(transform.position, transform.right * _size.x + transform.up * _size.y);
            }
        }
#endif
    }
}