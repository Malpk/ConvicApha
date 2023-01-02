using UnityEngine;
using System.Collections.Generic;

namespace MainMode.Room
{
    public class FireAndIceRoom : RoomBehaviour
    {
        [SerializeField] private MapGrid _mapGrid;
        [SerializeField] private MainSpawner _mapSpawner;
        [SerializeField] private UpPlatform _prefabRocketLaunch;

        private bool _isPlay;
        private List<UpPlatform> _assets = new List<UpPlatform>();
        private Vector2Int[] _spawnMask = new Vector2Int[]
            {Vector2Int.zero, Vector2Int.one, Vector2Int.right, Vector2Int.up };

        public override bool IsPlay => _isPlay;

        public override void Play()
        {
            if (!_isPlay)
            {
                _isPlay = true;
                foreach (var mask in _spawnMask)
                {
                    var asset = Instantiate(_prefabRocketLaunch.gameObject,
                        transform).GetComponent<UpPlatform>();
                    asset.UpDevice();
                    _assets.Add(asset);
                    _mapGrid.Points[mask.x * (_mapGrid.Points.GetLength(0) - 1),
                        mask.y * (_mapGrid.Points.GetLength(1) - 1)].SetItem(asset);
                }
                _mapSpawner.Play();
            }
        }

        public override void Stop()
        {
            _isPlay = false;
            _mapSpawner.Stop();
            while (_assets.Count > 0)
            {
                var delete = _assets[0];
                _assets.Remove(delete);
                Destroy(delete.gameObject);
            }
        }
    }
}
