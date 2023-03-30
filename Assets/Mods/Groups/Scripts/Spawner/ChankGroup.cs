using UnityEngine;

namespace MainMode
{
    public class ChankGroup : MonoBehaviour
    {
        [SerializeField] private Transform _chankHolder;
        [SerializeField] private GroupScheme _begin;

        private Chank[] _chanks;

        private void OnValidate()
        {
            if(_chankHolder)
                _chanks = _chankHolder.GetComponentsInChildren<Chank>();
        }

        private void Awake()
        {
            _chanks = _chankHolder.GetComponentsInChildren<Chank>();
        }

        public void SpawnGroup()
        {
            foreach (var chank in _chanks)
            {
                chank.Spawn();
            }
        }

        public void ClearDelete()
        {
            _begin.DeleteZone();
            foreach (var chank in _chanks)
            {
                chank.DeleteScheme();
            }
        }
    }
}