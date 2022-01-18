using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchMode;

namespace Underworld
{
    public class TrisMode : MonoBehaviour, ISequence
    {
        [SerializeField] private GameObject _tile;
        [SerializeField] private GameMode _paternMode;

        private GameMap _map = null;
  
        private List<TermPatern> _termPaterns = new List<TermPatern>();

        public void Constructor(SwitchMods swictMode)
        {
            if (_map != null)
                return;
            _map = swictMode.map;
            StartMode();
        }

        private void StartMode()
        {
            var map = _map.Map;
            foreach (var tile in map)
            {
                var instateTile = Instantiate(_tile, tile.VertixPosition, Quaternion.identity);
                instateTile.transform.parent = transform.parent;
                if (instateTile.TryGetComponent<TermPatern>(out TermPatern ternPattern))
                    _termPaterns.Add(ternPattern);
            }
            StartCoroutine(RunMode());
        }
        IEnumerator RunMode()
        {
            yield return new WaitWhile(() => _paternMode.statusWork);
            foreach (var tile in _termPaterns)
            {
                tile.SetTurnMode(false);
            }
            Destroy(gameObject);
        }
    }
}