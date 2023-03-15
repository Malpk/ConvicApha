using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class GroupPool : MonoBehaviour
    {
        [SerializeField] private UpGroupSet _prefab;

        private List<UpGroupSet> _groups = new List<UpGroupSet>();

        public Vector2 GroupSize => _prefab.GroupSize; 

        private void OnValidate()
        {
            name = _prefab.name + "Pool";
        }

        public UpGroupSet Create(Transform transform = null)
        {
            var group = GetGroup();
            group.OnComplite += ReturnGroup;
            if(transform)
            {
                group.transform.parent = transform;
            }
            return group;
        }

        private UpGroupSet GetGroup()
        {
            if (_groups.Count > 0)
            {
                var group = _groups[0];
                _groups.Remove(_groups[0]);
                group.gameObject.SetActive(true);
                return group;
            }
            else
            {
                return Instantiate(_prefab).GetComponent<UpGroupSet>();
            }
        }

        public void ReturnGroup(UpGroupSet group)
        {
            _groups.Add(group);
            group.transform.parent = transform;
            group.transform.localPosition = Vector3.zero;
            group.OnComplite -= ReturnGroup;
        }
    }
}