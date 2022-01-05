using UnityEngine;
using Zenject;

namespace Underworld
{
    public class TrisMode : MonoBehaviour
    {
        [SerializeField] private GameObject _tile;

        [Inject] private GameMap _map;

        private void Start()
        {
            var map = _map.Map;
            foreach (var tile in map)
            {
                Instantiate(_tile, tile.VertixPosition, Quaternion.identity);
            }
        }
    }
}