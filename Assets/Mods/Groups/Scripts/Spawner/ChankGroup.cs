using UnityEngine;

namespace MainMode
{
    public class ChankGroup : MonoBehaviour
    {
        [SerializeField] private Transform _chankHolder;

        private Chank[] _chanks;

        private void OnValidate()
        {
            if(_chankHolder)
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
            foreach (var chank in _chanks)
            {
                chank.DeleteScheme();
            }
        }
    }
}