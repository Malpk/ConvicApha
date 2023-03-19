using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class SpawnZone : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Scene Setting")]
        [SerializeField] private bool _showGismo;
        [SerializeField] private Color _gismoColor;
#endif
        [Header("Zone Setting")]
        [Min(0)]
        [SerializeField] private float _minSpawnDistance = 1;
        [SerializeField] private MapGrid _mapGrid;
        [SerializeField] private MapSpawner _mapSpawn;
        [SerializeField] private GroupPool[] _pools;
        [SerializeField] private BoxCollider2D _collider;

        private Player _player;
        private GroupPool _previusGroup;
        private List<UpGroupSet> _activeGroup = new List<UpGroupSet>();

        public event System.Action OnActivate;
        public event System.Action OnDectivate;

        public bool IsActive => _activeGroup.Count > 0;
        public Vector2 Size => _collider.size;

        private void Awake()
        {
            enabled = _player;
        }
        private void OnValidate()
        {
            if(_mapGrid)
                _mapGrid.SetSize(new Vector2Int((int)_collider.size.x, (int)_collider.size.y));
        }
        private void Update()
        {
            var distance = Vector2.Distance(_player.transform.position, transform.position);
            if (distance < _minSpawnDistance)
            {
                Spawn();
                enabled = false;
            }
        }

        public void Spawn()
        {
            if (_activeGroup.Count < 2)
            {
                var pool = GetPool();
                if (GetLengh() + pool.GroupSize.x <= _collider.size.x)
                {
                    _previusGroup = pool;
                    var group = _previusGroup.Create();
                    group.transform.parent = transform;
                    group.transform.localPosition = GetPosition(group.GroupSize.x);
                    group.transform.rotation = transform.rotation;
                    group.OnComplite += DeactivateGroup;
                    _activeGroup.Add(group);
                    _mapSpawn.Stop();
                    _mapSpawn.Clear();
                }
            }
        }

        private Vector2 GetPosition(float offset)
        {
            if (_activeGroup.Count > 0)
            {
                var position = _activeGroup[0].transform.localPosition;
                var direction = position.x > 0 ? Vector3.left : Vector3.right;
                position += direction * (_activeGroup[0].GroupSize.x + offset);
                return position;
            }
            return Vector3.zero;
        }

        private void DeactivateGroup(UpGroupSet group)
        {
            group.OnComplite -= DeactivateGroup;
            _activeGroup.Remove(group);
        }

        private GroupPool GetPool()
        {
            if (_pools.Length > 1)
            {
                var list = new List<GroupPool>();
                foreach (var item in _pools)
                {
                    if (_previusGroup != item)
                        list.Add(item);
                }
                return list[Random.Range(0, list.Count)];
            }
            return _pools[0];
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_showGismo)
            {
                Gizmos.color = _gismoColor;
                Gizmos.DrawCube(transform.position + (Vector3)_collider.offset,
                        transform.up * Size.y + transform.right * Size.x);
            }
        }
#endif
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                enabled = true;
                _player = player;
                _mapSpawn.Construct(player);
                if (!IsActive)
                    _mapSpawn.Play();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!IsActive && !_mapSpawn.enabled)
                _mapSpawn.Play();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                enabled = false;
                _player = null;
                _mapSpawn.Stop();
            }
        }


        private float GetLengh()
        {
            var amount = 0f;
            foreach (var group in _activeGroup)
            {
                amount += group.GroupSize.x;
            }
            return amount;
        }
    }
}
