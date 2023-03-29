using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [System.Serializable]
    public class SchemePool
    {
        public string Name = "name";

        [SerializeField] private GroupScheme _schemePrefab;

        private List<GroupScheme> _schemes = new List<GroupScheme>();

        public void OnValidate()
        {
            if(_schemePrefab)
                Name = _schemePrefab.name;
        }

        public GroupScheme GetScheme(Transform transform)
        {
            var scheme = Create();
            scheme.OnDeactivate += DeactivateScheme;
            if (transform)
            {
                scheme.transform.parent = transform;
                scheme.transform.localPosition = Vector3.zero;
            }
            return scheme;
        }

        private GroupScheme Create()
        {
            if (_schemes.Count > 0)
            {
                var scheme = _schemes[0];
                scheme.gameObject.SetActive(true);
                _schemes.Remove(scheme);
                return scheme;
            }
            return Object.Instantiate(_schemePrefab.gameObject).GetComponent<GroupScheme>();
        }

        private void DeactivateScheme(GroupScheme scheme)
        {
            scheme.OnDeactivate -= DeactivateScheme;
            _schemes.Add(scheme);
        }
    }
}
