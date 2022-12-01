using UnityEngine;

namespace MainMode.Items
{
    public abstract class Artifact : Item
    {
        [Min(1)]
        [SerializeField] private float _timeReload = 1f;

        private bool _isUse = true;
        private float _progress = 0f;

        public System.Action State;

        public override bool IsUse => _isUse;

        public float Reloading()
        {
            _progress += Time.deltaTime / _timeReload;
            if (_progress >= 1f)
            {
                _isUse = true;
            }
            return _timeReload - _timeReload * _progress;
        }
        public override bool Use()
        {
            if (IsUse)
            {
                _isUse = false;
                _progress = 0f;
                UseArtifact();
                return true;
            }
            return false;
        }
        public override void ResetState()
        {
            _isUse = true;
            _progress = 0f;
        }

        protected abstract void UseArtifact();
    }
}