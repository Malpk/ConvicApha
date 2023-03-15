using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class GroupSpawner : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField] private float _spawnDelays = 1f;
        [Min(5)]
        [SerializeField] private float _spawnDistance = 5f;
        [SerializeField] private Vector2 _mapSize;
        [SerializeField] private GroupPool[] _groups;

        private float _progress = 0f;

        private List<UpGroupSet> _groupAction = new List<UpGroupSet>();

        private void OnValidate()
        {
            if (_spawnDelays <= 0)
                _spawnDelays = Time.fixedDeltaTime;
        }

        private void FixedUpdate()
        {
            _progress += Time.fixedDeltaTime / _spawnDelays;
            if (_progress >= 1f)
            {
                _progress = 0f;
                var group = CreateGroup();
            }
        }

        private Transform CreateGroup()
        {
            var group = _groups[Random.Range(0, _groups.Length)].Create();
            group.OnComplite += DeleteGroup;
            group.transform.position = GetPosition();
            _groupAction.Add(group);
            return group.transform;
        }

        private Vector3 GetPosition()
        {
            var angle = Random.Range(0, 180)/ Mathf.Deg2Rad;
            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);
            return new Vector3(x, y);
        }

        private List<UpGroupSet> GetCloseGroup()
        {
            return null;
        }

        private void DeleteGroup(UpGroupSet group)
        {
            group.OnComplite -= DeleteGroup;
            _groupAction.Remove(group);
        }

    }
}