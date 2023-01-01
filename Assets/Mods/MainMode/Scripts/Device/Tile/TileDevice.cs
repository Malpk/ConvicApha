using UnityEngine;

namespace MainMode
{
    public abstract class TileDevice : DeviceV2
    {
        [Min(1)]
        [SerializeField] private float _workTime = 1f;

        private float _progress = 0f;

        protected override void ActivateDevice()
        {
            _progress = 0f;
        }
        protected override void DeactivateDevice()
        {
        }
        private void Update()
        {
            _progress += Time.deltaTime / _workTime;
            if (_progress >= 1)
                Deactivate();
        }
    }
}
