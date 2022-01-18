using System.Collections;
using UnityEngine;
using Zenject;
using SwitchMode;

namespace Underworld
{
    public class ViseMode : MonoBehaviour,ISequence
    {
        [SerializeField] private GameObject _tile;

        private GameMap _map = null;

        public void Constructor(SwitchMods swictMode)
        {
            if (_map != null)
                return;
            _map = swictMode.map;
            StartCoroutine(RunMode());
        }

        private IEnumerator RunMode()
        {
            var map = _map.Map;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                var size = map.GetLength(1);
                GameObject tile = null; ;
                for (int j = 0; j < size;  j++)
                {
                    Instantiate(_tile, map[i, j].VertixPosition, Quaternion.identity);
                    tile =Instantiate(_tile, map[size - 1 - i, j].VertixPosition, Quaternion.identity);
                }
                yield return new WaitWhile(() => (tile != null));
            }
            Destroy(gameObject);
        }
    }
}