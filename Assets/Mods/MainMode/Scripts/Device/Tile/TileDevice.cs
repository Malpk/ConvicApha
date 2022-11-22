using UnityEngine;

namespace MainMode
{
    public abstract class TileDevice : DeviceV2
    {
        [Min(1)]
        [SerializeField] private float _workTime = 1f;

        private float _progress = 0f;

        private void OnEnable()
        {
            OnActivate += ActivateTile;
        }
        private void OnDisable()
        {
            OnActivate -= ActivateTile;
        }

        private void ActivateTile()
        {
            _progress = 0f;
        }
        private void Update()
        {
            _progress += Time.deltaTime / _workTime;
            if (_progress >= 1)
                Deactivate();
        }
    }
}
