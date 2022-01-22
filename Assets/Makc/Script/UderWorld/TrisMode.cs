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
        private TernBase _term = null;
        private GameObject _lastInstate = null;
        private List<TermPatern> _termPaterns = new List<TermPatern>();

        public TernState state => _term == null ? TernState.Deactive : _term.state;

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
                _lastInstate = Instantiate(_tile, tile.VertixPosition, Quaternion.identity);
                _lastInstate.transform.parent = transform.parent;
                if (_lastInstate.TryGetComponent<TermPatern>(out TermPatern ternPattern))
                    _termPaterns.Add(ternPattern);
            }
            _term = _lastInstate.GetComponent<TernBase>();
            StartCoroutine(RunMode());
        }
        private IEnumerator RunMode()
        {
            yield return new WaitWhile(() => _paternMode.statusWork);
            foreach (var tile in _termPaterns)
            {
                tile.SetTurnMode(false);
            }
            yield return new WaitWhile(() => _lastInstate !=null);
            Destroy(gameObject);
        }
    }
}