using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class Chank : MonoBehaviour
    {
        [SerializeField] private SchemePool[] _schems;

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

        private void Awake()
        {
            Spawn();
        }

        public void Spawn()
        {
            if (!IsActive)
            {
                _prevouslPool = GetPool();
                _activeScheme = _prevouslPool.GetScheme(transform);
                _activeScheme.transform.up = _directions[Random.Range(0, _directions.Length)];
                _activeScheme.OnDeactivate += Deactivate;
            }
        }

        private void Deactivate(GroupScheme scheme)
        {
            scheme.OnDeactivate -= Deactivate;
            _activeScheme.gameObject.SetActive(false);
            _activeScheme = null;
        }

        private SchemePool GetPool()
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                Spawn();
            }
        }
    }
}