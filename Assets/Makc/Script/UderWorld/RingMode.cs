using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchMode;
using TileSpace;

namespace Underworld
{
    public class RingMode : GameMode,ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private float _swichDelay;
        [SerializeField] private GameObject _tile;
        [SerializeField] private List<Sprite> _maps = new List<Sprite>();

        private IVertex[,] _map;

        private MapReader _reader = new MapReader();
        private TermPatern[,] _terns;

        public override bool statusWork => 0 == 0;

        public void Constructor(SwitchMods swictMode)
        {
            _map = swictMode.map.Map;
            StartMode();
        }
        private void StartMode()
        {
            int x = _map.GetLength(1);
            int y = _map.GetLength(0);
            _terns = new TermPatern[y,x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    var instiate = Instantiate(_tile, _map[i, j].VertixPosition, Quaternion.identity);
                    instiate.transform.parent = transform;
                    _terns[i, j] = instiate.GetComponent<TermPatern>();
                    _terns[i, j].SetState(false);
                }
            }
            StartCoroutine(RunMode());
        }
        private IEnumerator RunMode()
        {
            foreach (var map in _maps)
            {
                var masks = _reader.ReadMap(map.texture);
                for (int i = 0; i < _terns.GetLength(0); i++)
                {
                    for (int j = 0; j < _terns.GetLength(1); j++)
                    {
                        if (masks.Contains(new Vector2Int(j, i)))
                            _terns[i, j].SetState(true);
                        else 
                            _terns[i, j].SetState(false);
                    }
                }
                yield return new WaitForSeconds(_swichDelay);
            }
        }
    }
}